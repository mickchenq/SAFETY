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
using SAFETY.Resources;
using Microsoft.Extensions.Localization;

namespace SAFETY.Areas.Stock.API
{
    [Area("Stock")]
    [Route("api/[area]/[controller]/[action]")]
    public class DailyStockApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _sFilePath = "";
        /// <summary>
        /// 檔案服務
        /// </summary>
        private readonly FileService _fileService;

        public DailyStockApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IWebHostEnvironment webHostEnvironment, FileService fileService)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
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
        public IActionResult QueryDailyStock([FromBody] QueryDailyStock model)
        {
            //不包含未上架者
            var res = (from t1 in _SAFETYContext.Inventory.Where(x => x.DcId == model.DcId && (x.InventoryStatus == 2 || x.InventoryKind != "I"))
                       join t5 in _SAFETYContext.Product on t1.ProductId equals t5.ProductId into ps5
                       from t5 in ps5.DefaultIfEmpty()
                       join t6 in _SAFETYContext.ProductPackage on t1.Unit equals t6.PackageId into ps6
                       from t6 in ps6.DefaultIfEmpty()
                       select new
                       {               
                           t1.InventoryKind,
                           t1.InvDate,
                           t1.DcId,
                           t1.CustomerId,
                           t1.ProductStatus,
                           t1.ProductId,
                           t1.ProductLotNo,
                           t1.Unit,
                           t1.ExpirationDate,                          
                           t1.LocationQuantity,                          
                           t5.ProductName,
                           t6.PackageName                          
                       }
                );
                        
            if (model.DcId != null && model.DcId.ToString() != "0")
            {
                res = res.Where(x => x.DcId == model.DcId);
            }
            if (model.CustomerId != null && model.CustomerId.ToString() != "0")
            {
                res = res.Where(x => x.CustomerId == model.CustomerId);
            }
            if (model.ProductId != null && model.ProductId.ToString() != "0")
            {
                res = res.Where(x => x.ProductId == model.ProductId);
            }
            if (!string.IsNullOrEmpty(model.ProductLotNo))
            {
                res = res.Where(x => x.ProductLotNo == model.ProductLotNo);
            }           
            if (model.ProductStatus.ToString() != "" && model.ProductStatus.ToString() != "0")
            {
                res = res.Where(x => x.ProductStatus == model.ProductStatus);
            }
            if (model.ReportDate.HasValue)
            {
                res = res.Where(x => x.InvDate <= model.ReportDate.Value);
            }
            if (model.StockType.ToString() != "" && model.StockType.ToString() != "0")
            {
                if (model.StockType.ToString() == "2")          //過期品
                    res = res.Where(x => x.ExpirationDate < model.ReportDate.Value);
                else if (model.StockType.ToString() == "3")          //即期品 (有效期限在一個月內)
                    res = res.Where(x => x.ExpirationDate >= model.ReportDate.Value && x.ExpirationDate <= model.ReportDate.Value.AddDays(30));
            }

            var gpdata = res.ToList().GroupBy(x => new { x.ProductStatus, x.ProductId, x.ProductName, x.Unit, x.PackageName, x.ProductLotNo, x.ExpirationDate })
               .Select(b => new
               {                  
                   ProductStatus = b.Key.ProductStatus,                   
                   ProductId = b.Key.ProductId,
                   ProductName = b.Key.ProductName,
                   Unit = b.Key.Unit,
                   PackageName = b.Key.PackageName,
                   ProductLotNo = b.Key.ProductLotNo,
                   ExpirationDate = b.Key.ExpirationDate,                   
                   Quantity = b.Select(bn => bn.LocationQuantity * (bn.InventoryKind == "O" ? -1 : 1)).Sum()
               }).Where(x => x.Quantity > 0).Distinct().ToList();

            return WriteJsonOk("", gpdata);
        }



        //end class
    }
}
