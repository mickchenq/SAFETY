using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYService;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SAFETY.Areas.Return.API
{
    [Area("Return")]
    [Route("api/[area]/[controller]/[action]")]
    public class ReturnedOrderApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _sFilePath = "";
        private IHttpContextAccessor _IHttpContextAccessor;
        /// <summary>
        /// 檔案服務
        /// </summary>
        private readonly FileService _fileService;
        public ReturnedOrderApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IWebHostEnvironment webHostEnvironment, FileService fileService, IHttpContextAccessor iHttpContextAccessor)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            //上傳的固定父目錄
            _sFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Upload");
            _IHttpContextAccessor = iHttpContextAccessor;
        }

        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryReturn([FromBody] QueryReturn model)
        {
            var res = (from t1 in _SAFETYContext.ReturnedOrder
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
                           t1.RelatedId,
                           t1.ReturnedDate,
                           t1.EstimatedReceiveDate,
                           t1.OperateType,
                           t1.ContactName,
                           t1.ContactPhone,
                           t1.ContactAddress,
                           t1.ReturnStatus,
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
            if (model.ReturnStatus.ToString() != "" && model.ReturnStatus.ToString() != "0")
            {
                res = res.Where(x => x.ReturnStatus == model.ReturnStatus);
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
        public async Task<IActionResult> DeleteReturn([FromBody] ReturnedOrder model)
        {
            //檢查是否可刪除
            var isUsed = _SAFETYContext.ReturnedOrder.Where(x => x.OrderId == model.OrderId && x.ReturnStatus != 1).Select(x => x.OrderId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["退貨通知已驗收完成，故不可刪除資料"]);

            _SAFETYContext.Database.BeginTransaction();
            //主檔
            _SAFETYContext.ReturnedOrder.Remove(model);
            //明細
            var detail = _SAFETYContext.ReturnedDetail.Where(x => x.OrderId == model.OrderId);
            _SAFETYContext.ReturnedDetail.RemoveRange(detail);
            //附件
            var up = _SAFETYContext.UploadFiles.Where(x => x.FormId == model.OrderId && x.FormKind == 14).ToList();
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
        /// 抓取出貨完成的通知單資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>      
        public IActionResult GetShippingOrder([FromBody] QueryShipping model)
        {
            //找出已建立的退貨單
            var res_return = _SAFETYContext.ReturnedOrder.Select(x=>x.RelatedId).Distinct().ToList();
            //找出重疊的ShippingOrder
            var res_ship= _SAFETYContext.ShippingOrder.Where(x => res_return.Contains(x.OrderId)).ToList();
            //已出過退貨單就不可再次新增
            var result = _SAFETYContext.ShippingOrder.Where(x => (x.ShippingStatus == 3 || x.OrderId == model.OrderId) && x.CustomerId == model.CustomerId && x.DcId == model.DcId).ToList();
            var res = result.Except(res_ship);
            //var res = _SAFETYContext.ShippingOrder.FromSqlRaw(@"
            //With cte as
            //(
            //    Select RelatedId, ProductId, ProductLotNo, Unit, Sum(Quantity) as Quantity 
            //    from ReturnedDetail rd with (nolock) 
            //    inner join ReturnedOrder ro with (nolock) On ro.OrderId=rd.OrderId 
            //    group by RelatedId, ProductId, ProductLotNo, Unit
            //)
            //Select distinct so.*
            //from ShippingDetail sd with (nolock)
            //inner join ShippingOrder so On so.OrderId=sd.OrderId
            //left join Product p with (nolock) On p.ProductId=sd.ProductId
            //left join ProductPackage pp with (nolock) On pp.PackageId=sd.Unit
            //left join cte On cte.RelatedId=sd.OrderId and cte.ProductId=sd.ProductId and cte.ProductLotNo=sd.ProductLotNo and cte.Unit=sd.Unit 
            //Where sd.Quantity-Isnull(cte.Quantity, 0)>0 and (so.ShippingStatus=3 or so.OrderId=@OrderId) and so.CustomerId=@CustomerId and DcId=@DcId
            //", new Microsoft.Data.SqlClient.SqlParameter("OrderId", model.OrderId)
            //, new Microsoft.Data.SqlClient.SqlParameter("CustomerId", model.CustomerId)
            //, new Microsoft.Data.SqlClient.SqlParameter("DcId", model.DcId)
            //).ToList();
            return Ok(res);
        }

        /// <summary>
        /// 抓取出貨完成的通知單明細
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
                           t1.Unit,
                           t1.Quantity,
                           t1.ExpirationDate,
                           t2.ProductCode,
                           t2.ProductName,
                           t2.Barcode,
                           t3.PackageName
                       }).OrderBy(x => x.DetailId).ToList();

            //var res = _SAFETYContext.ShippingDetailVM.FromSqlRaw(@"
            //With cte as
            //(
            //    Select RelatedId, ProductId, ProductLotNo, Unit, Sum(Quantity) as Quantity 
            //    from ReturnedDetail rd with (nolock) 
            //    inner join ReturnedOrder ro with (nolock) On ro.OrderId=rd.OrderId 
            //    group by RelatedId, ProductId, ProductLotNo, Unit
            //)
            //Select sd.OrderId, sd.DetailId, sd.ProductId, sd.ProductLotNo, sd.Unit
            //, sd.Quantity-Isnull(cte.Quantity, 0) as Quantity, sd.ExpirationDate, p.ProductCode, p.ProductName, p.Barcode, pp.PackageName
            //from ShippingDetail sd with (nolock)
            //left join Product p with (nolock) On p.ProductId=sd.ProductId
            //left join ProductPackage pp with (nolock) On pp.PackageId=sd.Unit
            //left join cte On cte.RelatedId=sd.OrderId and cte.ProductId=sd.ProductId and cte.ProductLotNo=sd.ProductLotNo and cte.Unit=sd.Unit   
            //Where sd.OrderId=@OrderId
            //", new Microsoft.Data.SqlClient.SqlParameter("OrderId", orderid)).ToList();


            return WriteJsonOk("", res);
        }


        /// <summary>
        /// 儲存退貨通知單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IActionResult> SaveReturn([FromBody] FullReturn model)
        {
            if (model.ReturnedOrder.OrderId == 0 || model.ReturnedOrder.OrderId.ToString() == "")
                return InsertReturn(model);     //新增
            else
                return UpdateReturn(model);     //修改
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertReturn([FromBody] FullReturn model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            model.ReturnedOrder.CreateId = user.SysUser.UserId; //待串人員驗證
            model.ReturnedOrder.CreateDate = DateTime.Now;
            //系統產生單號 NOyyyyMMddnnnn,DA:固定, yyyy:西元年, MM月份(補零), dd(日), nnnn(流水號)
            var NoKey = "DR" + DateTime.Today.ToString("yyyyMMdd");
            //抓db裡同一天最大序號者
            var maxResult = _SAFETYContext.ReturnedOrder.Where(x => x.OrderNo.Substring(0, 10) == NoKey && x.OrderNo.Length > 10)
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
                model.ReturnedOrder.OrderNo = NoKey + (iValue + 1).ToString().PadLeft(4, '0');
            }
            else
                model.ReturnedOrder.OrderNo = NoKey + "0001";

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.ReturnedOrder.Add(model.ReturnedOrder);
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.ReturnedDetail.Select(x => { x.OrderId = model.ReturnedOrder.OrderId; return x; }).ToList();

            //明細檔
            _SAFETYContext.ReturnedDetail.AddRange(model.ReturnedDetail);

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else

                _SAFETYContext.Database.RollbackTransaction();
            return res.Key
                ? WriteJsonOk(_localizer["新增成功"], model.ReturnedOrder.OrderId)
                : WriteJsonErr(_localizer["新增失敗"]);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateReturn([FromBody] FullReturn model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            model.ReturnedOrder.ModifyId = user.SysUser.UserId; //待串人員驗證
            model.ReturnedOrder.ModifyDate = DateTime.Now;

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.ReturnedOrder.Update(model.ReturnedOrder);
            //(先刪除，後新增)
            var rtDetail = _SAFETYContext.ReturnedDetail.Where(x => x.OrderId == model.ReturnedOrder.OrderId);
            _SAFETYContext.ReturnedDetail.RemoveRange(rtDetail);

            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["修改失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.ReturnedDetail.Select(x => { x.OrderId = model.ReturnedOrder.OrderId; x.ReturnedDetailId = 0; return x; }).ToList();
            //明細檔
            _SAFETYContext.ReturnedDetail.AddRange(model.ReturnedDetail);

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
                ? WriteJsonOk(_localizer["修改成功"], model.ReturnedOrder.OrderId)
                : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 新增進貨通知 主檔附件
        /// </summary>
        /// <param name="orderid">進貨通知單id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadReturnNewFile(int orderid, List<ReturnNewFile> model)
        {
            if (model == null || model.Count <= 0) return WriteJsonOk(_localizer["上傳附件成功"]);

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            string sMsg = "";
            _SAFETYContext.Database.BeginTransaction();
            model.ForEach(item =>
            {
                // 上傳檔案             
                var upResult = _fileService.UploadFile(_sFilePath, "Purchase\\" + orderid.ToString(), "", item.UP_FILE);
                if (upResult.Result.Success)
                {
                    var fileInfo = new UploadFiles
                    {
                        FormKind = 14,
                        FormId = orderid,
                        FileName = upResult.Result.FileName,
                        FilePath = upResult.Result.FilePath,
                        CreateId = user.SysUser.UserId,
                        CreateDate = DateTime.Now
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
                return WriteJsonErr(_localizer["上傳附件失敗"] + "：" + sMsg.Trim());
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
        public IActionResult GetReturnData(int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");
            //主檔
            var redurnedOrder = _SAFETYContext.ReturnedOrder.FirstOrDefault(x => x.OrderId == orderid);
            if (redurnedOrder == null)
                return WriteJsonOk("");

            //明細
            var redurnedDetail = (from t1 in _SAFETYContext.ReturnedDetail.Where(x => x.OrderId == orderid)
                                  join t2 in _SAFETYContext.ProductPackage on t1.Unit equals t2.PackageId into ps
                                  from t2 in ps.DefaultIfEmpty()
                                  join t3 in _SAFETYContext.Product on t1.ProductId equals t3.ProductId into ps3
                                  from t3 in ps3.DefaultIfEmpty()
                                  select new
                                  {
                                      t1.ReturnedDetailId,
                                      t1.OrderId,
                                      t1.ProductId,
                                      t1.ProductLotNo,
                                      t3.ProductCode,
                                      t3.ProductName,
                                      t3.Barcode,
                                      t1.Unit,
                                      t1.Quantity,
                                      t1.ExpirationDate,
                                      t1.MakeDate,
                                      t1.ProductStatus,
                                      t2.PackageName
                                  }
            ).OrderBy(x => x.ReturnedDetailId).ToList();

            //附件
            var uploadFiles = _SAFETYContext.UploadFiles.Where(x => x.FormId == orderid && x.FormKind == 14).OrderBy(x => x.UploadId).ToList();
            //是否可修改 待入庫則不可修改
            var isView = redurnedOrder.ReturnStatus <= 2 ? false : true;
            var result = new { redurnedOrder, redurnedDetail, uploadFiles, isView };
            return WriteJsonOk("", result);
        }



    }
}
