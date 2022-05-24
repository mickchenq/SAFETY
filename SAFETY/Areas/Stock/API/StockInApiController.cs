using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYService;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETYModel.ViewModel.Stock;

namespace SAFETY.Areas.Purchase.API
{
    [Area("Stock")]
    [Route("api/[area]/[controller]/[action]")]
    public class StockInApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _sFilePath = "";
        /// <summary>
        /// 檔案服務
        /// </summary>
        private readonly FileService _fileService;
        public StockInApiController(SAFETYContext SAFETYContext, IWebHostEnvironment webHostEnvironment, FileService fileService)
        {
            _SAFETYContext = SAFETYContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            //上傳的固定父目錄
            _sFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Upload");
        }
        

        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryStockIn([FromBody] QueryStockIn model)
        {
            var res = (from t1 in _SAFETYContext.StockInOrder
                       join t2 in _SAFETYContext.NotificationOrder on t1.RelatedId equals t2.OrderId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.Customer on t2.CustomerId equals t3.CustomerId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       join t4 in _SAFETYContext.SysUser on t1.StockUserId equals t4.UserId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       select new
                       {
                           t1.OrderId,
                           t1.OrderNo,
                           t1.RelatedId,
                           t1.OrderDate,
                           t1.StockUserId,
                           t1.StockStatus,
                           StockStatusName = t1.StockStatus == 1 ? "待入庫" : t1.StockStatus == 2 ? "入庫中" : t1.StockStatus == 3 ? "已入庫" : "",
                           RelatedOrderNo = t2.OrderNo,
                           t2.CustomerOrderNo,
                           t3.CustomerId,
                           t3.CustomerName,
                           StockUserName = t4.UserName,
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
            if (!string.IsNullOrEmpty(model.RelatedOrderNo))
            {
                res = res.Where(x => x.RelatedOrderNo == model.RelatedOrderNo);
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
        /// 讀取入庫資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetStockInData([FromBody] int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");
            //主檔
            var stockOrder = _SAFETYContext.StockInOrder.FirstOrDefault(x => x.OrderId == orderid);
            if (stockOrder == null)
                return WriteJsonOk("");

            //明細
            var stockDetail = (from t1 in _SAFETYContext.StockInDetail.Where(x => x.OrderId == orderid)
                                      join t2 in _SAFETYContext.ProductPackage on t1.Unit equals t2.PackageId into ps
                                      from t2 in ps.DefaultIfEmpty()
                                      select new
                                      {
                                          t1.OrderDetailId,
                                          t1.OrderId,
                                          t1.ProductId,
                                          t1.ProductLotNo,
                                          t1.Unit,
                                          t1.Quantity,
                                          t1.MakeDate,
                                          t1.ExpirationDate,
                                          t1.Remarks,
                                          t2.PackageName
                                      }
            ).OrderBy(x => x.OrderDetailId).ToList();

            var stockLocationDetail = (from t1 in _SAFETYContext.StockInDetail.Where(x => x.OrderId == orderid)
                                       join t2 in _SAFETYContext.StockInLocationDetail on t1.OrderDetailId equals t2.OrderDetailId into ps
                                       from t2 in ps.DefaultIfEmpty()
                                       join t3 in _SAFETYContext.ProductPackage on t1.Unit equals t3.PackageId into ps2
                                       from t3 in ps2.DefaultIfEmpty()
                                       select new
                                       {
                                           t2.LocationDetailId,
                                           t2.LocationId,
                                           t2.LocationQuantity,
                                           t2.Remarks,
                                           t2.DetailStatus,
                                           DetailStatusName = t2.DetailStatus == 1 ? "待上架" : t2.DetailStatus == 2 ? "上架中" : t2.DetailStatus == 3 ? "已上架" : "",
                                           t1.OrderDetailId,
                                           t1.OrderId,
                                           t1.ProductId,
                                           t1.ProductLotNo,
                                           t1.Unit,
                                           t1.Quantity,
                                           t1.MakeDate,
                                           t1.ExpirationDate,
                                           t3.PackageName
                                       });

            //附件
            var uploadFiles = _SAFETYContext.UploadFiles.Where(x => x.FormId == orderid && x.FormKind == 10).OrderBy(x => x.UploadId).ToList();
            //是否可修改 2:入庫中 3:已入庫則不可修改
            var isView = stockOrder.StockStatus == 1 ? true : false;
            var result = new { stockOrder, stockDetail, uploadFiles, isView, stockLocationDetail };
            return WriteJsonOk("", result);
        }


    }
}
