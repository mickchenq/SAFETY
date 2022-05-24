using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYService;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;

namespace SAFETY.Areas.Shipping.API
{
    [Area("Shipping")]
    [Route("api/[area]/[controller]/[action]")]
    public class ShippingApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _sFilePath = "";
        /// <summary>
        /// 檔案服務
        /// </summary>
        private readonly FileService _fileService;

        public ShippingApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor, IWebHostEnvironment webHostEnvironment, FileService fileService)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            //上傳的固定父目錄
            _sFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Upload");
        }

        #region 出貨通知單       
        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryShipping([FromBody] QueryShipping model)
        {
            var res = (from t1 in _SAFETYContext.ShippingOrder
                       join t2 in _SAFETYContext.Customer on t1.CustomerId equals t2.CustomerId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.DataCenter on t1.DcId equals t3.DcId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       select new
                       {
                           t1.OrderId,
                           t1.OrderNo,
                           t1.DcId,
                           t1.CustomerId,
                           t1.EstimatedShippingDate,
                           t1.EstimatedReceiveDate,
                           t1.OperationType,
                           t1.ArriveType,
                           t1.ContactName,
                           t1.ContactPhone,
                           t1.ZipCode,
                           t1.ContactAddress,
                           t1.ShippingStatus,
                           t2.CustomerName,
                           t3.DcName,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate
                       }
                );

            if (model.CustomerId != null && model.CustomerId.ToString() != "0")
            {
                res = res.Where(x => x.CustomerId == model.CustomerId);
            }
            if (!string.IsNullOrEmpty(model.OrderNo))
            {
                res = res.Where(x => x.OrderNo == model.OrderNo);
            }            
            if (model.DcId != null && model.DcId.ToString() != "0")
            {
                res = res.Where(x => x.DcId == model.DcId);
            }
            if (model.ShippingStatus.ToString() != "" && model.ShippingStatus.ToString() != "0")
            {
                res = res.Where(x => x.ShippingStatus == model.ShippingStatus);
            }
            if (model.EstimatedShippingStart.HasValue)
            {
                res = res.Where(x => x.EstimatedShippingDate >= model.EstimatedShippingStart.Value);
            }
            if (model.EstimatedShippingEnd.HasValue)
            {
                res = res.Where(x => x.EstimatedShippingDate <= model.EstimatedShippingEnd.Value);
            }

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteShipping([FromBody] ShippingOrder model)
        {
            //檢查是否可刪除
            var isUsed = _SAFETYContext.ShippingOrder.Where(x => x.OrderId == model.OrderId && x.ShippingStatus != 1).Select(x => x.OrderId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已出貨，故不可刪除資料"]);

            _SAFETYContext.Database.BeginTransaction();
            //主檔
            _SAFETYContext.ShippingOrder.Remove(model);
            //明細
            var detail = _SAFETYContext.ShippingDetail.Where(x => x.OrderId == model.OrderId);
            _SAFETYContext.ShippingDetail.RemoveRange(detail);
            //附件
            var up = _SAFETYContext.UploadFiles.Where(x => x.FormId == model.OrderId && x.FormKind == 11).ToList();
            _SAFETYContext.UploadFiles.RemoveRange(up);
            // 刪除實體檔案
            up.ForEach(item =>
            {
                var del = _fileService.DeleteFile(Path.Combine(_sFilePath, item.FilePath, item.FileName));
            });

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
               ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        /// <summary>
        /// 儲存出貨通知單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IActionResult> SaveShipping([FromBody] FullShipping model)
        {
            if (model.ShippingOrder.OrderId == 0 || model.ShippingOrder.OrderId.ToString() == "")
                return InsertShipping(model);     //新增
            else
                return UpdateShipping(model);     //修改
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertShipping([FromBody] FullShipping model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.ShippingOrder.CreateId = _user.SysUser.UserId;
            model.ShippingOrder.CreateDate = DateTime.Now;
            model.ShippingOrder.ModifyId = _user.SysUser.UserId;
            model.ShippingOrder.ModifyDate = DateTime.Now;
            //系統產生單號 NOyyyyMMddnnnn,DA:固定, yyyy:西元年, MM月份(補零), dd(日), nnnn(流水號)
            var NoKey = "SH" + DateTime.Today.ToString("yyyyMMdd");
            //抓db裡同一天最大序號者
            var maxResult = _SAFETYContext.ShippingOrder.Where(x => x.OrderNo.Substring(0, 10) == NoKey && x.OrderNo.Length > 10)
                .GroupBy(x => x.OrderNo.Substring(0, 10))
                .Select((b) => new
                {
                    maxNo = b.Max(bn => bn.OrderNo)
                }).ToList();
            if (maxResult != null && maxResult.Count() > 0)
            {
                int iValue = 0;
                if (maxResult[0].maxNo.Length >= 14)
                    int.TryParse(maxResult[0].maxNo.Substring(10, 4), out iValue);
                model.ShippingOrder.OrderNo = NoKey + (iValue + 1).ToString().PadLeft(4, '0');
            }
            else
                model.ShippingOrder.OrderNo = NoKey + "0001";

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.ShippingOrder.Add(model.ShippingOrder);
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.ShippingDetail.Select(x => { x.OrderId = model.ShippingOrder.OrderId; return x; }).ToList();

            //明細檔
            _SAFETYContext.ShippingDetail.AddRange(model.ShippingDetail);

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();          
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["新增成功"], model.ShippingOrder.OrderId)
                : WriteJsonErr(_localizer["新增失敗"]);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateShipping([FromBody] FullShipping model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.ShippingOrder.ModifyId = _user.SysUser.UserId;
            model.ShippingOrder.ModifyDate = DateTime.Now;

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.ShippingOrder.Update(model.ShippingOrder);
            //(先刪除，後新增)
            var shippingDetail = _SAFETYContext.ShippingDetail.Where(x => x.OrderId == model.ShippingOrder.OrderId);
            _SAFETYContext.ShippingDetail.RemoveRange(shippingDetail);

            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.ShippingDetail.Select(x => { x.OrderId = model.ShippingOrder.OrderId; x.DetailId = 0; return x; }).ToList();
            //明細檔
            _SAFETYContext.ShippingDetail.AddRange(model.ShippingDetail);

            //移除附件
            var upfile = model.UploadFiles.Where(x => x.FormId == -99).ToList();
            _SAFETYContext.UploadFiles.RemoveRange(upfile);
            // 刪除實體檔案
            upfile.ForEach(item =>
            {
                var del = _fileService.DeleteFile(Path.Combine(_sFilePath, item.FilePath, item.FileName));
            });

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["修改成功"], model.ShippingOrder.OrderId)
                : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 新增出貨通知 主檔附件
        /// </summary>
        /// <param name="orderid">出貨通知單id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadShippingNewFile(int orderid, List<ShippingNewFile> model)
        {
            if (model == null || model.Count <= 0) return WriteJsonOk(_localizer["上傳附件成功"]);
            string sMsg = "";
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            _SAFETYContext.Database.BeginTransaction();
            model.ForEach(item =>
            {
                // 上傳檔案             
                var upResult = _fileService.UploadFile(_sFilePath, "Shipping\\" + orderid.ToString(), "", item.UP_FILE);
                if (upResult.Result.Success)
                {
                    var fileInfo = new UploadFiles
                    {
                        FormKind = 11,
                        FormId = orderid,
                        FileName = upResult.Result.FileName,
                        FilePath = upResult.Result.FilePath,
                        CreateId = _user.SysUser.UserId,
                        CreateDate = DateTime.Now,
                        ModifyId = _user.SysUser.UserId,
                        ModifyDate = DateTime.Now
                    };
                    _SAFETYContext.UploadFiles.Add(fileInfo);
                }
                else
                {
                    sMsg += upResult.Result.Message + "<br />";
                }
            });

            if (sMsg.Trim() != "")
            {
                return WriteJsonErr(_localizer["上傳附件失敗"]+"：" + sMsg.Trim());
            }

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
               ? WriteJsonOk(_localizer["上傳附件成功"])
               : WriteJsonErr(_localizer["上傳附件失敗"]);
        }

        /// <summary>
        /// 讀取出貨通知單資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetShippingData(int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");
            //主檔
            var shippingOrder = _SAFETYContext.ShippingOrder.FirstOrDefault(x => x.OrderId == orderid);
            if (shippingOrder == null)
                return WriteJsonOk("");

            //明細
            var shippingDetail = (from t1 in _SAFETYContext.ShippingDetail.Where(x => x.OrderId == orderid)
                                      join t2 in _SAFETYContext.ProductPackage on t1.Unit equals t2.PackageId into ps
                                      from t2 in ps.DefaultIfEmpty()
                                      select new
                                      {
                                          t1.DetailId,
                                          t1.OrderId,
                                          t1.ProductId,
                                          t1.ProductLotNo,
                                          t1.ExpirationDate,
                                          t1.Unit,
                                          t1.Quantity,
                                          t1.ProductStatus,
                                          t2.PackageName
                                      }
            ).OrderBy(x => x.DetailId).ToList();

            //附件
            var uploadFiles = _SAFETYContext.UploadFiles.Where(x => x.FormId == orderid && x.FormKind == 11).OrderBy(x => x.UploadId).ToList();
            //是否可修改 驗收完成則不可修改
            var isView = shippingOrder.ShippingStatus <= 1 ? false : true;
            var result = new { shippingOrder, shippingDetail, uploadFiles, isView };
            return WriteJsonOk("", result);
        }
        #endregion

        #region 出庫單
        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryStockOut([FromBody] QueryStockOut model)
        {
            var res = (from t1 in _SAFETYContext.StockOutOrder
                       join t4 in _SAFETYContext.ShippingOrder on t1.RelatedId equals t4.OrderId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       join t2 in _SAFETYContext.Customer on t4.CustomerId equals t2.CustomerId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.DataCenter on t4.DcId equals t3.DcId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       select new
                       {
                           t1.OrderId,
                           t1.OrderNo,
                           t1.RelatedId,
                           t1.OrderDate,
                           t1.StockUserId,
                           t1.StockStatus,
                           t4.DcId,
                           t4.CustomerId,
                           ShippingOrderNo=t4.OrderNo,
                           t2.CustomerName,
                           t3.DcName,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate
                       }
                );

            if (model.CustomerId != null && model.CustomerId.ToString() != "0")
            {
                res = res.Where(x => x.CustomerId == model.CustomerId);
            }
            if (!string.IsNullOrEmpty(model.OrderNo))
            {
                res = res.Where(x => x.OrderNo == model.OrderNo);
            }
            if (model.DcId != null && model.DcId.ToString() != "0")
            {
                res = res.Where(x => x.DcId == model.DcId);
            }
            if (!string.IsNullOrEmpty(model.ShippingOrderNo))
            {
                res = res.Where(x => x.ShippingOrderNo == model.ShippingOrderNo);
            }
            if (model.StockUserId != null && model.StockUserId.ToString() != "0")
            {
                res = res.Where(x => x.StockUserId == model.StockUserId);
            }
            if (model.StockStatus.ToString() != "" && model.StockStatus.ToString() != "0")
            {
                res = res.Where(x => x.StockStatus == model.StockStatus);
            }
            if (model.OrderDateStart.HasValue)
            {
                res = res.Where(x => x.OrderDate >= model.OrderDateStart.Value);
            }
            if (model.OrderDateEnd.HasValue)
            {
                res = res.Where(x => x.OrderDate <= model.OrderDateEnd.Value);
            }

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteStockOut([FromBody] StockOutOrder model)
        {
            //檢查是否可刪除
            var isUsed = _SAFETYContext.StockOutOrder.Where(x => x.OrderId == model.OrderId && x.StockStatus != 1).Select(x => x.OrderId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已出貨，故不可刪除資料"]);

            _SAFETYContext.Database.BeginTransaction();
            //主檔
            _SAFETYContext.StockOutOrder.Remove(model);
            //變更出貨通知單狀態 1:待出庫
            var shippingorder = _SAFETYContext.ShippingOrder.Where(x => x.OrderId == model.RelatedId).FirstOrDefault();
            if (shippingorder != null)
            { 
                shippingorder.ShippingStatus = 1;
                _SAFETYContext.ShippingOrder.Update(shippingorder);
            }
            //明細
            var detail = _SAFETYContext.StockOutDetail.Where(x => x.OrderId == model.OrderId);
            _SAFETYContext.StockOutDetail.RemoveRange(detail);

            //出庫單下架資料及出入庫資料
            detail.ToList().ForEach(item =>
            {
                var loc = _SAFETYContext.StockOutLocationDetail.Where(x => x.OrderDetailId == item.OrderDetailId).ToList();
                _SAFETYContext.StockOutLocationDetail.RemoveRange(loc);
                if (loc != null && loc.Count > 0)
                {
                    var inv = _SAFETYContext.Inventory.Where(x => x.InventoryKind == "O" && loc.Select(b => b.LocationDetailId).ToList().Contains(x.LocationDetailId)).ToList();
                    _SAFETYContext.Inventory.RemoveRange(inv);
                }
            });

            //附件
            var up = _SAFETYContext.UploadFiles.Where(x => x.FormId == model.OrderId && x.FormKind == 13).ToList();
            _SAFETYContext.UploadFiles.RemoveRange(up);
            // 刪除實體檔案
            up.ForEach(item =>
            {
                var del = _fileService.DeleteFile(Path.Combine(_sFilePath, item.FilePath, item.FileName));
            });

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
               ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        /// <summary>
        /// 抓取待出庫的通知單資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        public IActionResult GetShippingOrder([FromBody] QueryShipping model)
        {
            var res = _SAFETYContext.ShippingOrder.Where(x => (x.ShippingStatus == 1 || x.OrderId==model.OrderId) && x.CustomerId == model.CustomerId && x.DcId == model.DcId).ToList();
            return Ok(res);
        }

        /// <summary>
        /// 抓取待出庫的通知單明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        public IActionResult GetShippingDetail(int? orderid)
        {
            var res = (from t1 in _SAFETYContext.ShippingDetail.Where(x => x.OrderId == orderid)
                       join t2 in _SAFETYContext.Product on t1.ProductId equals t2.ProductId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.ProductPackage on t1.Unit equals t3.PackageId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       select new
                       {
                           t1.OrderId,
                           t1.DetailId,
                           t1.ProductId,
                           t1.ProductLotNo,                           
                           t1.ExpirationDate,
                           t1.Unit,
                           t1.Quantity,
                           t1.ProductStatus,
                           t2.ProductCode,
                           t2.ProductName,
                           t2.Barcode,
                           t3.PackageName
                       }).OrderBy(x=>x.DetailId).ToList();

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 抓取待出庫商品所在儲位資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        public IActionResult GetStockOutLocationData(int CustomerId, int ProductId, int Unit, int DcId, string ProductLotNo, DateTime? ExpirationDate, int ProductStatus, int Qty, int LocationId)
        {
            //var result = _SAFETYContext.Inventory.Where(x => x.CustomerId == CustomerId && x.ProductId == ProductId && x.Unit == Unit && x.InvDate<=InvDate.Value).ToList();
            //要排除待上架者
            var result = _SAFETYContext.Inventory.Where(x => x.CustomerId == CustomerId && x.ProductId == ProductId && x.Unit == Unit && x.DcId == DcId && x.ProductLotNo== ProductLotNo && x.ExpirationDate==ExpirationDate.Value && x.ProductStatus==ProductStatus && x.InventoryStatus != 1).ToList();
            //同客戶商品及單位仍有庫存的儲位(或之前已選的儲位)
            var gpdata = result.GroupBy(x => new { x.CustomerId, x.ProductId, x.Unit, x.ProductLotNo, x.ExpirationDate, x.ProductStatus, x.LocationId })
                .Select(b => new
                {
                    //locationIdlist = b.Select(bn => bn.LocationId).ToList(),
                    CustomerId = b.Key.CustomerId,
                    ProductId = b.Key.ProductId,
                    Unit = b.Key.Unit,
                    ProductLotNo=b.Key.ProductLotNo,
                    ExpirationDate= b.Key.ExpirationDate,
                    ProductStatus=b.Key.ProductStatus,
                    LocationId = b.Key.LocationId,
                    Quantity = b.Select(bn =>bn.LocationQuantity * (bn.InventoryKind == "O" ? -1 : 1)).Sum()
                    //Quantity = b.Select(bn => 1 * (bn.InventoryKind == "O" ? -1 : 1)).Sum()
                }).Where(x => x.Quantity >= Qty || x.LocationId== LocationId).Distinct().ToList();
           
            if (gpdata != null && gpdata.Count>0)
            { 
                var res = _SAFETYContext.Location.Where(x => gpdata.Select(bn => bn.LocationId).ToList().Contains(x.LocationId)).Distinct().ToList();
                return Ok(res);
            }
            else
                return Ok(null);
        }

        /// <summary>
        /// 儲存出庫單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IActionResult> SaveStockOut([FromBody] FullStockOut model)
        {
            if (model.StockOutOrder.OrderId == 0 || model.StockOutOrder.OrderId.ToString() == "")
                return InsertStockOut(model);     //新增
            else
                return UpdateStockOut(model);     //修改
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertStockOut([FromBody] FullStockOut model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.StockOutOrder.CreateId = _user.SysUser.UserId;
            model.StockOutOrder.CreateDate = DateTime.Now;
            model.StockOutOrder.ModifyId = _user.SysUser.UserId;
            model.StockOutOrder.ModifyDate = DateTime.Now;

            var shippingOrder = _SAFETYContext.ShippingOrder.Where(x => x.OrderId == model.StockOutOrder.RelatedId && x.ShippingStatus == 1).FirstOrDefault();
            if (shippingOrder==null)
            {
                return WriteJsonErr(_localizer["新增失敗：該出貨通知單號已出庫"]);
            }

            //系統產生單號 NOyyyyMMddnnnn,DA:固定, yyyy:西元年, MM月份(補零), dd(日), nnnn(流水號)
            var NoKey = "SO" + DateTime.Today.ToString("yyyyMMdd");
            //抓db裡同一天最大序號者
            var maxResult = _SAFETYContext.StockOutOrder.Where(x => x.OrderNo.Substring(0, 10) == NoKey && x.OrderNo.Length > 10)
                .GroupBy(x => x.OrderNo.Substring(0, 10))
                .Select((b) => new
                {
                    maxNo = b.Max(bn => bn.OrderNo)
                }).ToList();
            if (maxResult != null && maxResult.Count() > 0)
            {
                int iValue = 0;
                if (maxResult[0].maxNo.Length >= 14)
                    int.TryParse(maxResult[0].maxNo.Substring(10, 4), out iValue);
                model.StockOutOrder.OrderNo = NoKey + (iValue + 1).ToString().PadLeft(4, '0');
            }
            else
                model.StockOutOrder.OrderNo = NoKey + "0001";

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockOutOrder.Add(model.StockOutOrder);
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.StockOutDetail.Select(x => { x.OrderId = model.StockOutOrder.OrderId; return x; }).ToList();
            //明細檔
            _SAFETYContext.StockOutDetail.AddRange(model.StockOutDetail);
            var resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            //出庫單下架資料
            int inx = 0;
            List<StockOutLocationDetail> locDetail = new List<StockOutLocationDetail>();
            model.StockOutDetail.ForEach(item =>
            {
                var locInfo = new StockOutLocationDetail
                {
                    OrderDetailId =item.OrderDetailId,
                    LocationId = model.StockOutLocationDetail[inx].LocationId,
                    Unit = item.Unit,
                    LocationQuantity = model.StockOutLocationDetail[inx].LocationQuantity,
                    Remarks = model.StockOutLocationDetail[inx].Remarks,
                    DetailStatus = model.StockOutLocationDetail[inx].DetailStatus
                };
                inx++;
                locDetail.Add(locInfo);
            });
            _SAFETYContext.StockOutLocationDetail.AddRange(locDetail);
            resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            };

            //出入庫資料
            locDetail.ForEach(item =>
            {
                var tmp = model.StockOutDetail.Where(x => x.OrderDetailId == item.OrderDetailId).FirstOrDefault();
                var inv = new Inventory
                {
                    InventoryKind = "O",
                    InventoryStatus = 3,
                    DcId = shippingOrder.DcId,
                    InvDate = model.StockOutOrder.OrderDate,
                    LocationDetailId = item.LocationDetailId,
                    CustomerId = shippingOrder.CustomerId,
                    ProductId = tmp.ProductId,
                    LocationId = item.LocationId,
                    LocationQuantity = item.LocationQuantity,
                    Unit = item.Unit,
                    ProductLotNo = tmp.ProductLotNo,
                    ExpirationDate = tmp.ExpirationDate,
                    ProductStatus = tmp.ProductStatus,
                    CreateId = _user.SysUser.UserId,
                    CreateDate = DateTime.Now,
                    ModifyId = _user.SysUser.UserId,
                    ModifyDate = DateTime.Now,
                };
                _SAFETYContext.Inventory.Add(inv);
            });

            //變更出貨通知單狀態為 2::揀貨中
            shippingOrder.ShippingStatus = 2;
            _SAFETYContext.ShippingOrder.Update(shippingOrder);

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["新增成功"], model.StockOutOrder.OrderId)
                : WriteJsonErr(_localizer["新增失敗"]);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateStockOut([FromBody] FullStockOut model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.StockOutOrder.ModifyId = _user.SysUser.UserId;
            model.StockOutOrder.ModifyDate = DateTime.Now;

            var oldStockOutOrder = _SAFETYContext.StockOutOrder.Where(x => x.OrderId == model.StockOutOrder.OrderId).FirstOrDefault();
            if (oldStockOutOrder == null)
            {
                return WriteJsonErr(_localizer["修改失敗：單號不存在"]);
            }
            
            var shippingOrder = _SAFETYContext.ShippingOrder.Where(x => (x.OrderId == model.StockOutOrder.RelatedId && x.ShippingStatus == 1) || x.OrderId==oldStockOutOrder.RelatedId).FirstOrDefault();
            if (shippingOrder == null)
            {
                return WriteJsonErr(_localizer["修改失敗：該出貨通知單號已出庫"]);
            }

            //把之前抓到的同一key值實體的EntityState設為獨立的
            _SAFETYContext.Entry(oldStockOutOrder).State = EntityState.Detached;
            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockOutOrder.Update(model.StockOutOrder);
            //(先刪除，後新增)           
            var stockOutDetail = _SAFETYContext.StockOutDetail.Where(x => x.OrderId == model.StockOutOrder.OrderId);           
            _SAFETYContext.StockOutDetail.RemoveRange(stockOutDetail);
        
            stockOutDetail.ToList().ForEach(item =>
            {
                var tmp = _SAFETYContext.StockOutLocationDetail.Where(x => x.OrderDetailId == item.OrderDetailId).ToList();
                _SAFETYContext.StockOutLocationDetail.RemoveRange(tmp);
                if (tmp != null && tmp.Count > 0)
                {
                    var inv = _SAFETYContext.Inventory.Where(x => x.InventoryKind == "O" && tmp.Select(b => b.LocationDetailId).ToList().Contains(x.LocationDetailId)).ToList();
                    _SAFETYContext.Inventory.RemoveRange(inv);
                }
            });

            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.StockOutDetail.Select(x => { x.OrderId = model.StockOutOrder.OrderId; x.OrderDetailId = 0; return x; }).ToList();
            //明細檔
            _SAFETYContext.StockOutDetail.AddRange(model.StockOutDetail);
            var resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            //出庫單下架資料
            int inx = 0;
            List<StockOutLocationDetail> locDetail = new List<StockOutLocationDetail>();
            model.StockOutDetail.ForEach(item =>
            {
                var locInfo = new StockOutLocationDetail
                {
                    OrderDetailId = item.OrderDetailId,
                    LocationId = model.StockOutLocationDetail[inx].LocationId,
                    Unit = item.Unit,
                    LocationQuantity = model.StockOutLocationDetail[inx].LocationQuantity,
                    Remarks = model.StockOutLocationDetail[inx].Remarks,
                    DetailStatus = model.StockOutLocationDetail[inx].DetailStatus
                };
                inx++;
                locDetail.Add(locInfo);
            });
            _SAFETYContext.StockOutLocationDetail.AddRange(locDetail);
            resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            };

            //出入庫資料
            locDetail.ForEach(item =>
            {
                var tmp = model.StockOutDetail.Where(x => x.OrderDetailId == item.OrderDetailId).FirstOrDefault();
                var inv = new Inventory
                {
                    InventoryKind = "O",
                    InventoryStatus = 3,
                    DcId = shippingOrder.DcId,
                    InvDate = model.StockOutOrder.OrderDate,
                    LocationDetailId = item.LocationDetailId,
                    CustomerId = shippingOrder.CustomerId,
                    ProductId = tmp.ProductId,
                    LocationId = item.LocationId,
                    LocationQuantity = item.LocationQuantity,
                    Unit = item.Unit,
                    ProductLotNo = tmp.ProductLotNo,
                    ExpirationDate = tmp.ExpirationDate,
                    ProductStatus = tmp.ProductStatus,
                    CreateId = _user.SysUser.UserId,
                    CreateDate = DateTime.Now,
                    ModifyId = _user.SysUser.UserId,
                    ModifyDate = DateTime.Now,
                };
                _SAFETYContext.Inventory.Add(inv);
            });

            //變更出貨通知單狀態為 2:揀貨中
            if (model.StockOutOrder.RelatedId != oldStockOutOrder.RelatedId)
            {
                shippingOrder = _SAFETYContext.ShippingOrder.Where(x => x.OrderId == model.StockOutOrder.RelatedId).FirstOrDefault();
                shippingOrder.ShippingStatus = 2;
                _SAFETYContext.ShippingOrder.Update(shippingOrder);
                //原來的那筆再改回1:待出庫
                var so = _SAFETYContext.ShippingOrder.Where(x => x.OrderId == oldStockOutOrder.RelatedId).FirstOrDefault();
                so.ShippingStatus = 1;
                _SAFETYContext.ShippingOrder.Update(so);
            }

            //移除附件
            var upfile = model.UploadFiles.Where(x => x.FormId == -99).ToList();
            _SAFETYContext.UploadFiles.RemoveRange(upfile);
            // 刪除實體檔案
            upfile.ForEach(item =>
            {
                var del = _fileService.DeleteFile(Path.Combine(_sFilePath, item.FilePath, item.FileName));
            });

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["修改成功"], model.StockOutOrder.OrderId)
                : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 新增出庫單 主檔附件
        /// </summary>
        /// <param name="orderid">出庫單id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadStockOutNewFile(int orderid, List<StockOutNewFile> model)
        {
            if (model == null || model.Count <= 0) return WriteJsonOk(_localizer["上傳附件成功"]);
            string sMsg = "";
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            _SAFETYContext.Database.BeginTransaction();
            model.ForEach(item =>
            {
                // 上傳檔案             
                var upResult = _fileService.UploadFile(_sFilePath, "StockOut\\" + orderid.ToString(), "", item.UP_FILE);
                if (upResult.Result.Success)
                {
                    var fileInfo = new UploadFiles
                    {
                        FormKind = 13,
                        FormId = orderid,
                        FileName = upResult.Result.FileName,
                        FilePath = upResult.Result.FilePath,
                        CreateId = _user.SysUser.UserId,
                        CreateDate = DateTime.Now,
                        ModifyId = _user.SysUser.UserId,
                        ModifyDate = DateTime.Now
                    };
                    _SAFETYContext.UploadFiles.Add(fileInfo);
                }
                else
                {
                    sMsg += upResult.Result.Message + "<br />";
                }
            });

            if (sMsg.Trim() != "")
            {
                return WriteJsonErr(_localizer["上傳附件失敗"]+"：" + sMsg.Trim());
            }

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
               ? WriteJsonOk(_localizer["上傳附件成功"])
               : WriteJsonErr(_localizer["上傳附件失敗"]);
        }

        /// <summary>
        /// 讀取出庫單資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetStockOutData(int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");
            //主檔
            var stockOutOrder = (from t1 in _SAFETYContext.StockOutOrder.Where(x => x.OrderId == orderid)
                    join t2 in _SAFETYContext.ShippingOrder on t1.RelatedId equals t2.OrderId into ps
                    from t2 in ps.DefaultIfEmpty()
                    select new
                    {
                        t1.OrderId,
                        t1.OrderNo,
                        t1.RelatedId,
                        t1.OrderDate,
                        t1.StockUserId,
                        t1.StockStatus,
                        t1.CreateId,
                        t1.CreateDate,
                        t1.ModifyId,
                        t1.ModifyDate,
                        t2.CustomerId,
                        t2.DcId
                    }
                ).FirstOrDefault(); 

            if (stockOutOrder == null)
                return WriteJsonOk("");

            //明細
            var stockOutDetail = (from t1 in _SAFETYContext.StockOutDetail.Where(x => x.OrderId == orderid)
                                  join t2 in _SAFETYContext.StockOutLocationDetail on t1.OrderDetailId equals t2.OrderDetailId into ps
                                  from t2 in ps.DefaultIfEmpty()
                                  join t3 in _SAFETYContext.Product on t1.ProductId equals t3.ProductId into ps3
                                  from t3 in ps3.DefaultIfEmpty()
                                  join t4 in _SAFETYContext.ProductPackage on t2.Unit equals t4.PackageId into ps4
                                  from t4 in ps4.DefaultIfEmpty()
                                  join t5 in _SAFETYContext.Location on t2.LocationId equals t5.LocationId into ps5
                                  from t5 in ps5.DefaultIfEmpty()
                                  select new
                                  {
                                      t1.OrderDetailId,
                                      t1.OrderId,
                                      t1.ProductId,
                                      t1.ProductLotNo,
                                      t1.ProductStatus,
                                      t1.ExpirationDate,
                                      t2.Unit,
                                      t1.Quantity,
                                      t2.LocationId,
                                      oldLocationId= t2.LocationId,
                                      t2.LocationQuantity,
                                      t2.Remarks,
                                      t2.DetailStatus,
                                      t3.ProductCode,
                                      t3.ProductName,
                                      t3.Barcode,
                                      t4.PackageName,
                                      t5.LocationCode
                                  }
            ).OrderBy(x => x.OrderDetailId).ToList();

            //附件
            var uploadFiles = _SAFETYContext.UploadFiles.Where(x => x.FormId == orderid && x.FormKind == 13).OrderBy(x => x.UploadId).ToList();
            //是否可修改 出庫則不可修改
            var isView = stockOutOrder.StockStatus <= 1 ? false : true;
            var result = new { stockOutOrder, stockOutDetail, uploadFiles, isView };
            return WriteJsonOk("", result);
        }
        #endregion


        //end class
    }
}
