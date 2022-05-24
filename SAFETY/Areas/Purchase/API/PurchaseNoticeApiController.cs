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
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.Purchase.API
{
    [Area("Purchase")]
    [Route("api/[area]/[controller]/[action]")]
    public class PurchaseNoticeApiController : BaseController
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
        public PurchaseNoticeApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor, IWebHostEnvironment webHostEnvironment, FileService fileService)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            //上傳的固定父目錄
            _sFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Upload");
        }
        /// <summary>
        /// 取商品選項
        /// </summary>
        /// <returns></returns>
        //public IActionResult GetProds() => WriteJsonOk("", _SAFETYContext.Product.Where(x => true));
        /// <summary>
        /// 取客戶選項
        /// </summary>
        /// <returns></returns>
        //public IActionResult GetCust() => WriteJsonOk("", _SAFETYContext.Customer.Where(x => true));

        #region 進貨通知維護     
        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryNotice([FromBody] QueryNotice model)
        {
            var res = (from t1 in _SAFETYContext.NotificationOrder
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
                           t1.CustomerOrderNo,
                           t1.EstimatedReceiveDate,
                           t1.ActualReceiveDate,
                           t1.ReceiveType,
                           t1.OperateType,
                           t1.IsUrgentOrder,
                           t1.ContactName,
                           t1.ContactPhone,
                           t1.ContactAddress,
                           t1.NotificationStatus,
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
            if (!string.IsNullOrEmpty(model.CustomerOrderNo))
            {
                res = res.Where(x => x.CustomerOrderNo == model.CustomerOrderNo);
            }
            if (model.DcId != null && model.DcId.ToString() != "0")
            {
                res = res.Where(x => x.DcId == model.DcId);
            }
            if (model.NotificationStatus.ToString() != "" && model.NotificationStatus.ToString() != "0")
            {
                res = res.Where(x => x.NotificationStatus == model.NotificationStatus);
            }
            if (model.ReceiveDateStart.HasValue)
            {
                res = res.Where(x => x.EstimatedReceiveDate >= model.ReceiveDateStart.Value);
            }
            if (model.ReceiveDateEnd.HasValue)
            {
                res = res.Where(x => x.EstimatedReceiveDate <= model.ReceiveDateEnd.Value);
            }

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteNotice([FromBody] NotificationOrder model)
        {
            //檢查是否可刪除
            var isUsed = _SAFETYContext.NotificationOrder.Where(x => x.OrderId == model.OrderId && x.NotificationStatus != 1).Select(x => x.OrderId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["進貨通知已驗收完成，故不可刪除資料"]);

            _SAFETYContext.Database.BeginTransaction();
            //主檔
            _SAFETYContext.NotificationOrder.Remove(model);
            //明細
            var detail = _SAFETYContext.NotificationDetail.Where(x => x.OrderId == model.OrderId);
            _SAFETYContext.NotificationDetail.RemoveRange(detail);
            //附件
            var up = _SAFETYContext.UploadFiles.Where(x => x.FormId == model.OrderId && x.FormKind == 10).ToList();
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
        /// 儲存進貨通知單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IActionResult> SaveNotice([FromBody] FullNotice model)
        {
            if (model.NotificationOrder.OrderId == 0 || model.NotificationOrder.OrderId.ToString() == "")
                return InsertNotice(model);     //新增
            else
                return UpdateNotice(model);     //修改
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertNotice([FromBody] FullNotice model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.NotificationOrder.CreateId = _user.SysUser.UserId;
            model.NotificationOrder.CreateDate = DateTime.Now;
            model.NotificationOrder.ModifyId = _user.SysUser.UserId;
            model.NotificationOrder.ModifyDate = DateTime.Now;
            //系統產生單號 NOyyyyMMddnnnn,DA:固定, yyyy:西元年, MM月份(補零), dd(日), nnnn(流水號)
            var NoKey = "NO" + DateTime.Today.ToString("yyyyMMdd");
            //抓db裡同一天最大序號者
            var maxResult = _SAFETYContext.NotificationOrder.Where(x => x.OrderNo.Substring(0, 10) == NoKey && x.OrderNo.Length > 10)
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
                model.NotificationOrder.OrderNo = NoKey + (iValue + 1).ToString().PadLeft(4, '0');
            }
            else
                model.NotificationOrder.OrderNo = NoKey + "0001";

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.NotificationOrder.Add(model.NotificationOrder);
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.NotificationDetail.Select(x => { x.OrderId = model.NotificationOrder.OrderId; return x; }).ToList();

            //明細檔
            _SAFETYContext.NotificationDetail.AddRange(model.NotificationDetail);

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else

                _SAFETYContext.Database.RollbackTransaction();
            return res.Key
                ? WriteJsonOk(_localizer["新增成功"], model.NotificationOrder.OrderId)
                : WriteJsonErr(_localizer["新增失敗"]);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateNotice([FromBody] FullNotice model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.NotificationOrder.ModifyId = _user.SysUser.UserId;
            model.NotificationOrder.ModifyDate = DateTime.Now;

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.NotificationOrder.Update(model.NotificationOrder);
            //(先刪除，後新增)
            var notificationDetail = _SAFETYContext.NotificationDetail.Where(x => x.OrderId == model.NotificationOrder.OrderId);
            _SAFETYContext.NotificationDetail.RemoveRange(notificationDetail);

            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr("新增失敗");
            }
            // 更新主檔id
            //明細檔
            model.NotificationDetail.Select(x => { x.OrderId = model.NotificationOrder.OrderId; x.DetailId = 0; return x; }).ToList();
            //明細檔
            _SAFETYContext.NotificationDetail.AddRange(model.NotificationDetail);

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
                ? WriteJsonOk(_localizer["修改成功"], model.NotificationOrder.OrderId)
                : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 新增進貨通知 主檔附件
        /// </summary>
        /// <param name="orderid">進貨通知單id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadNoticeNewFile(int orderid, List<NoticeNewFile> model)
        {
            if (model == null || model.Count <= 0) return WriteJsonOk(_localizer["上傳附件成功"]);
            string sMsg = "";
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            _SAFETYContext.Database.BeginTransaction();
            model.ForEach(item =>
            {
                // 上傳檔案             
                var upResult = _fileService.UploadFile(_sFilePath, "Purchase\\" + orderid.ToString(), "", item.UP_FILE);
                if (upResult.Result.Success)
                {
                    var fileInfo = new UploadFiles
                    {
                        FormKind = 10,
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
                return WriteJsonErr(_localizer["上傳附件失敗"] +"：" + sMsg.Trim());
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
        /// 讀取進貨通知單資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetNoticeData(int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");
            //主檔
            var notificationOrder = _SAFETYContext.NotificationOrder.FirstOrDefault(x => x.OrderId == orderid);
            if (notificationOrder == null)
                return WriteJsonOk("");

            //明細
            var notificationDetail = (from t1 in _SAFETYContext.NotificationDetail.Where(x => x.OrderId == orderid)
                                      join t2 in _SAFETYContext.ProductPackage on t1.Unit equals t2.PackageId into ps
                                      from t2 in ps.DefaultIfEmpty()
                                      select new
                                      {
                                          t1.DetailId,
                                          t1.OrderId,
                                          t1.ProductId,
                                          t1.ProductLotNo,
                                          t1.Unit,
                                          t1.Quantity,
                                          t1.MakeDate,
                                          t1.ExpirationDate,
                                          t1.Remarks,
                                          t1.ProductStatus,
                                          t2.PackageName
                                      }
            ).OrderBy(x => x.DetailId).ToList();

            //附件
            var uploadFiles = _SAFETYContext.UploadFiles.Where(x => x.FormId == orderid && x.FormKind == 10).OrderBy(x => x.UploadId).ToList();
            //是否可修改 待入庫則不可修改
            var isView = notificationOrder.NotificationStatus <= 2 ? false : true;
            var result = new { notificationOrder, notificationDetail, uploadFiles, isView };
            return WriteJsonOk("", result);
        }

        /// <summary>
        /// 通知明細
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // public IActionResult QueryNoticeDt(int id) => WriteJsonOk("", _SAFETYContext.NotificationDetail.Where(x => x.OrderId==id));
        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /* public IActionResult UpdateNoticeDt([FromBody] NotificationDetail model)
         {
             if (!ModelState.IsValid)
             {
                 return ModelValidate();
             }
             if (model.OrderId == 0)
             {
                 _SAFETYContext.NotificationDetail.Add(model);
             }
             else
             {
                 _SAFETYContext.NotificationDetail.Update(model);
             }
             var res = _SAFETYContext.SaveChanges();
             var upTxt = model.DetailId == 0 ? "新增" : "編輯";

             return res > 0
                 ? WriteJsonOk($"{upTxt}成功")
                 : WriteJsonErr($"{upTxt}失敗");
         }*/
        #endregion

        #region 上架儲位指派(入庫)
        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryStockIn([FromBody] QueryStockInOrder model)
        {
            var res = (from t1 in _SAFETYContext.StockInOrder
                       join t4 in _SAFETYContext.NotificationOrder on t1.RelatedId equals t4.OrderId into ps4
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
                           NotificationOrderNo = t4.OrderNo,
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
            if (!string.IsNullOrEmpty(model.NotificationOrderNo))
            {
                res = res.Where(x => x.NotificationOrderNo == model.NotificationOrderNo);
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
        public async Task<IActionResult> DeleteStockIn([FromBody] StockInOrder model)
        {
            //檢查是否可刪除
            var isUsed = _SAFETYContext.StockInOrder.Where(x => x.OrderId == model.OrderId && x.StockStatus != 1).Select(x => x.OrderId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已入庫，故不可刪除資料"]);

            _SAFETYContext.Database.BeginTransaction();
            //主檔
            _SAFETYContext.StockInOrder.Remove(model);
            //變更進貨通知單狀態 2:待入庫
            var notificationOrder = _SAFETYContext.NotificationOrder.Where(x => x.OrderId == model.RelatedId).FirstOrDefault();
            if (notificationOrder != null)
            {
                notificationOrder.NotificationStatus = 2;
                _SAFETYContext.NotificationOrder.Update(notificationOrder);
            }

            //明細
            var detail = _SAFETYContext.StockInDetail.Where(x => x.OrderId == model.OrderId);
            _SAFETYContext.StockInDetail.RemoveRange(detail);

            //入庫單上架資料及出入庫資料
            detail.ToList().ForEach(item =>
            {
                var loc = _SAFETYContext.StockInLocationDetail.Where(x => x.OrderDetailId == item.OrderDetailId).ToList();
                _SAFETYContext.StockInLocationDetail.RemoveRange(loc);
                if (loc != null && loc.Count > 0)
                {
                    var inv = _SAFETYContext.Inventory.Where(x => x.InventoryKind == "I" && loc.Select(b => b.LocationDetailId).ToList().Contains(x.LocationDetailId)).ToList();
                    _SAFETYContext.Inventory.RemoveRange(inv);
                }
            });

            //附件
            var up = _SAFETYContext.UploadFiles.Where(x => x.FormId == model.OrderId && x.FormKind == 12).ToList();
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
        /// 抓取待入庫的通知單資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        public IActionResult GetNotificationOrder([FromBody] QueryNotice model)
        {
            var res = _SAFETYContext.NotificationOrder.Where(x => (x.NotificationStatus == 2 || x.OrderId == model.OrderId) && x.CustomerId == model.CustomerId && x.DcId == model.DcId).Select(x => new { x.OrderId, x.OrderNo }).Distinct().ToList();
            if (model.OrderType == 2)
                res = _SAFETYContext.ReturnedOrder.Where(x => (x.ReturnStatus == 2 || x.OrderId == model.OrderId) && x.CustomerId == model.CustomerId && x.DcId == model.DcId).Select(x => new { x.OrderId, x.OrderNo }).Distinct().ToList();
            return Ok(res);
        }

        /// <summary>
        /// 抓取待入庫的通知單明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        public IActionResult GetNotificationDetail(int? orderid, int OrderType = 1)
        {
            var res = (from t1 in _SAFETYContext.NotificationDetail.Where(x => x.OrderId == orderid)
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
                           t1.Unit,
                           t1.Quantity,
                           t1.MakeDate,
                           t1.ExpirationDate,
                           t1.ProductStatus,
                           t2.ProductCode,
                           t2.ProductName,
                           t2.Barcode,
                           t3.PackageName
                       }).OrderBy(x => x.DetailId).ToList();

            if (OrderType == 2)
                res = (from t1 in _SAFETYContext.ReturnedDetail.Where(x => x.OrderId == orderid)
                       join t2 in _SAFETYContext.Product on t1.ProductId equals t2.ProductId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.ProductPackage on t1.Unit equals t3.PackageId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       select new
                       {
                           t1.OrderId,
                           DetailId = t1.ReturnedDetailId,
                           t1.ProductId,
                           t1.ProductLotNo,
                           t1.Unit,
                           t1.Quantity,
                           t1.MakeDate,
                           t1.ExpirationDate,
                           t1.ProductStatus,
                           t2.ProductCode,
                           t2.ProductName,
                           t2.Barcode,
                           t3.PackageName
                       }).OrderBy(x => x.DetailId).ToList();

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 抓取可上架的儲位資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        public IActionResult GetStockkInLocationData(int DcId, int LocationId)
        {
            //所屬物流中心的儲位(或之前已選的儲位)
            var data = (from t1 in _SAFETYContext.Room.Where(x => x.DcId == DcId)
                        join t2 in _SAFETYContext.Area on t1.RoomId equals t2.RoomId into ps2
                        from t2 in ps2.DefaultIfEmpty()
                        join t3 in _SAFETYContext.Shelf on t2.AreaId equals t3.AreaId into ps3
                        from t3 in ps3.DefaultIfEmpty()
                        join t4 in _SAFETYContext.Layer on t3.ShelfId equals t4.ShelfId into ps4
                        from t4 in ps4.DefaultIfEmpty()
                        join t5 in _SAFETYContext.Location on t4.LayerId equals t5.LayerId into ps5
                        from t5 in ps5.DefaultIfEmpty()
                        select new
                        {
                            t5.LocationId,
                            t5.LocationCode,
                            t5.IsStop
                        }).Where(x => x.LocationId.ToString() != null || x.LocationId == LocationId).Distinct().ToList();

            //已被使用的儲位
            //var result = _SAFETYContext.Inventory.Where(x => x.LocationId == LocationId).ToList();          
            var gpdata = _SAFETYContext.Inventory.ToList().GroupBy(x => new { x.LocationId })
                .Select(b => new
                {
                    LocationId = b.Key.LocationId,
                    //Quantity = b.Select(bn => 1 * (bn.InventoryKind == "O" ? -1 : 1)).Sum(),
                    Quantity = b.Select(bn => bn.LocationQuantity * (bn.InventoryKind == "O" ? -1 : 1)).Sum()
                }).Where(x => x.Quantity > 0).Distinct().ToList();

            //所有該物流中心下的儲位扣除已被使用的
            if (data != null && data.Count > 0)
            {
                var res = data.Where(x => gpdata.All(x2 => x.LocationId != x2.LocationId || x.LocationId == LocationId)).Distinct().ToList();
                return Ok(res);
            }
            else
                return Ok(null);
        }

        /// <summary>
        /// 儲存入庫單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IActionResult> SaveStockIn([FromBody] FullStockIn model)
        {
            if (model.StockInOrder.OrderId == 0 || model.StockInOrder.OrderId.ToString() == "")
                return InsertStockIn(model);     //新增
            else
                return UpdateStockIn(model);     //修改
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertStockIn([FromBody] FullStockIn model)
        {
            /*if (!ModelState.IsValid)
            {
                return ModelValidate();
            }*/
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.StockInOrder.CreateId = _user.SysUser.UserId;
            model.StockInOrder.CreateDate = DateTime.Now;
            model.StockInOrder.ModifyId = _user.SysUser.UserId;
            model.StockInOrder.ModifyDate = DateTime.Now;

            var notificationOrder = _SAFETYContext.NotificationOrder.Where(x => x.OrderId == model.StockInOrder.RelatedId && x.NotificationStatus == 2).FirstOrDefault();
            if (model.StockInOrder.OrderType == 1 && notificationOrder == null)
            {
                return WriteJsonErr(_localizer["新增失敗：該進貨通知單號已出庫"]);
            }

            var returnOrder = _SAFETYContext.ReturnedOrder.Where(x => x.OrderId == model.StockInOrder.RelatedId && x.ReturnStatus == 2).FirstOrDefault();

            //系統產生單號 NOyyyyMMddnnnn,DA:固定, yyyy:西元年, MM月份(補零), dd(日), nnnn(流水號)
            var NoKey = "SI" + DateTime.Today.ToString("yyyyMMdd");
            //抓db裡同一天最大序號者
            var maxResult = _SAFETYContext.StockInOrder.Where(x => x.OrderNo.Substring(0, 10) == NoKey && x.OrderNo.Length > 10)
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
                model.StockInOrder.OrderNo = NoKey + (iValue + 1).ToString().PadLeft(4, '0');
            }
            else
                model.StockInOrder.OrderNo = NoKey + "0001";

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockInOrder.Add(model.StockInOrder);
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.StockInDetail.Select(x => { x.OrderId = model.StockInOrder.OrderId; return x; }).ToList();
            //明細檔
            _SAFETYContext.StockInDetail.AddRange(model.StockInDetail);
            var resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            //入庫單下架資料
            int inx = 0;
            List<StockInLocationDetail> locDetail = new List<StockInLocationDetail>();
            model.StockInDetail.ForEach(item =>
            {
                var locInfo = new StockInLocationDetail
                {
                    OrderDetailId = item.OrderDetailId,
                    LocationId = model.StockInLocationDetail[inx].LocationId,
                    Unit = item.Unit,
                    LocationQuantity = model.StockInLocationDetail[inx].LocationQuantity,
                    Remarks = model.StockInLocationDetail[inx].Remarks,
                    DetailStatus = model.StockInLocationDetail[inx].DetailStatus
                };
                inx++;
                locDetail.Add(locInfo);
            });
            _SAFETYContext.StockInLocationDetail.AddRange(locDetail);
            resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            };

            //出入庫資料
            locDetail.ForEach(item =>
            {
                var tmp = model.StockInDetail.Where(x => x.OrderDetailId == item.OrderDetailId).FirstOrDefault();
                var inv = new Inventory
                {
                    InventoryKind = "I",
                    InventoryStatus = 1,
                    DcId = model.StockInOrder.OrderType == 1? notificationOrder.DcId : returnOrder.DcId,
                    InvDate = model.StockInOrder.OrderDate,
                    LocationDetailId = item.LocationDetailId,
                    CustomerId = model.StockInOrder.OrderType == 1 ? notificationOrder.CustomerId : returnOrder.CustomerId,
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

            //變更進貨通知單狀態為 3:入庫中
            if (model.StockInOrder.OrderType == 1)
            {
                notificationOrder.NotificationStatus = 3;
                _SAFETYContext.NotificationOrder.Update(notificationOrder);
            }
            if (model.StockInOrder.OrderType == 2)
            {
                returnOrder.ReturnStatus = 3;
                _SAFETYContext.ReturnedOrder.Update(returnOrder);
            }

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["新增成功"], model.StockInOrder.OrderId)
                : WriteJsonErr(_localizer["新增失敗"]);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateStockIn([FromBody] FullStockIn model)
        {
            /*if (!ModelState.IsValid)
            {
                return ModelValidate();
            }*/
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.StockInOrder.ModifyId = _user.SysUser.UserId;
            model.StockInOrder.ModifyDate = DateTime.Now;

            var oldStockInOrder = _SAFETYContext.StockInOrder.Where(x => x.OrderId == model.StockInOrder.OrderId).FirstOrDefault();
            if (oldStockInOrder == null)
            {
                return WriteJsonErr(_localizer["修改失敗：單號不存在"]);
            }

            var notificationOrder = _SAFETYContext.NotificationOrder.Where(x => (x.OrderId == model.StockInOrder.RelatedId && x.NotificationStatus == 2) || x.OrderId == oldStockInOrder.RelatedId).FirstOrDefault();
            if (notificationOrder == null)
            {
                return WriteJsonErr(_localizer["修改失敗：該進貨通知單號已出庫"]);
            }

            //把之前抓到的同一key值實體的EntityState設為獨立的
            _SAFETYContext.Entry(oldStockInOrder).State = EntityState.Detached;
            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockInOrder.Update(model.StockInOrder);
            //(先刪除，後新增)           
            var stockInDetail = _SAFETYContext.StockInDetail.Where(x => x.OrderId == model.StockInOrder.OrderId);
            _SAFETYContext.StockInDetail.RemoveRange(stockInDetail);

            stockInDetail.ToList().ForEach(item =>
            {
                var tmp = _SAFETYContext.StockInLocationDetail.Where(x => x.OrderDetailId == item.OrderDetailId).ToList();
                _SAFETYContext.StockInLocationDetail.RemoveRange(tmp);
                if (tmp != null && tmp.Count > 0)
                {
                    var inv = _SAFETYContext.Inventory.Where(x => x.InventoryKind == "I" && tmp.Select(b => b.LocationDetailId).ToList().Contains(x.LocationDetailId)).ToList();
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
            model.StockInDetail.Select(x => { x.OrderId = model.StockInOrder.OrderId; x.OrderDetailId = 0; return x; }).ToList();
            //明細檔
            _SAFETYContext.StockInDetail.AddRange(model.StockInDetail);
            var resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            //入庫單下架資料
            int inx = 0;
            List<StockInLocationDetail> locDetail = new List<StockInLocationDetail>();
            model.StockInDetail.ForEach(item =>
            {
                var locInfo = new StockInLocationDetail
                {
                    OrderDetailId = item.OrderDetailId,
                    LocationId = model.StockInLocationDetail[inx].LocationId,
                    Unit = item.Unit,
                    LocationQuantity = model.StockInLocationDetail[inx].LocationQuantity,
                    Remarks = model.StockInLocationDetail[inx].Remarks,
                    DetailStatus = model.StockInLocationDetail[inx].DetailStatus
                };
                inx++;
                locDetail.Add(locInfo);
            });
            _SAFETYContext.StockInLocationDetail.AddRange(locDetail);
            resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            };

            //出入庫資料
            locDetail.ForEach(item =>
            {
                var tmp = model.StockInDetail.Where(x => x.OrderDetailId == item.OrderDetailId).FirstOrDefault();
                var inv = new Inventory
                {
                    InventoryKind = "I",
                    InventoryStatus = 1,
                    DcId = notificationOrder.DcId,
                    InvDate = model.StockInOrder.OrderDate,
                    LocationDetailId = item.LocationDetailId,
                    CustomerId = notificationOrder.CustomerId,
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

            //變更進貨通知單狀態為 3:入庫中
            if (model.StockInOrder.RelatedId != oldStockInOrder.RelatedId)
            {
                notificationOrder = _SAFETYContext.NotificationOrder.Where(x => x.OrderId == model.StockInOrder.RelatedId).FirstOrDefault();
                notificationOrder.NotificationStatus = 3;
                _SAFETYContext.NotificationOrder.Update(notificationOrder);
                //原來的那筆再改回2:待入庫
                var so = _SAFETYContext.NotificationOrder.Where(x => x.OrderId == oldStockInOrder.RelatedId).FirstOrDefault();
                so.NotificationStatus = 2;
                _SAFETYContext.NotificationOrder.Update(so);
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
                ? WriteJsonOk(_localizer["修改成功"], model.StockInOrder.OrderId)
                : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 新增入庫單 主檔附件
        /// </summary>
        /// <param name="orderid">入庫單id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadStockInNewFile(int orderid, List<StockInNewFile> model)
        {
            if (model == null || model.Count <= 0) return WriteJsonOk(_localizer["上傳附件成功"]);
            string sMsg = "";
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            _SAFETYContext.Database.BeginTransaction();
            model.ForEach(item =>
            {
                // 上傳檔案             
                var upResult = _fileService.UploadFile(_sFilePath, "StockIn\\" + orderid.ToString(), "", item.UP_FILE);
                if (upResult.Result.Success)
                {
                    var fileInfo = new UploadFiles
                    {
                        FormKind = 12,
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
        /// 讀取入庫單資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetStockInData(int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");
            //主檔
            var stockInOrder = (from t1 in _SAFETYContext.StockInOrder.Where(x => x.OrderId == orderid)
                                join t2 in _SAFETYContext.NotificationOrder on t1.RelatedId equals t2.OrderId into ps
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

            if (stockInOrder == null)
                return WriteJsonOk("");

            //明細
            var stockInDetail = (from t1 in _SAFETYContext.StockInDetail.Where(x => x.OrderId == orderid)
                                 join t2 in _SAFETYContext.StockInLocationDetail on t1.OrderDetailId equals t2.OrderDetailId into ps
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
                                     t1.MakeDate,
                                     t1.ExpirationDate,
                                     t1.ProductStatus,
                                     t2.Unit,
                                     t1.Quantity,
                                     t2.LocationId,
                                     oldLocationId = t2.LocationId,
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
            var uploadFiles = _SAFETYContext.UploadFiles.Where(x => x.FormId == orderid && x.FormKind == 12).OrderBy(x => x.UploadId).ToList();
            //是否可修改 入庫則不可修改
            var isView = stockInOrder.StockStatus <= 1 ? false : true;
            var result = new { stockInOrder, stockInDetail, uploadFiles, isView };
            return WriteJsonOk("", result);
        }


        #endregion

        //end class
    }
}
