using SAFETYModel.DBModels;
using SAFETYModel.ViewModel.StorageMgnt;
using SAFETYModel.ViewModel.BasicSet;
using SAFETY.Controllers;
using SAFETY.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.StorageMgnt.API
{
    [Area("StorageMgnt")]
    [Route("api/[area]/[controller]/[action]")]
    public class StorageVisualizationApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public StorageVisualizationApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
        }

        public IActionResult QueryInventoryByLocation([FromBody]Location queryInfo)
        {
            // 根據 LocationId 去抓
            var result = (
                          from t3 in _SAFETYContext.Location.Where(x => x.LocationId == queryInfo.LocationId)
                          join t4 in _SAFETYContext.Inventory on t3.LocationId equals t4.LocationId into ps4
                          from t4 in ps4.DefaultIfEmpty()
                          select new
                          {
                              t3.LocationId,
                              t4.InventoryKind,
                              t4.DcId,
                              t4.CustomerId,
                              t4.ProductId,
                              t4.Unit,
                              t4.ProductLotNo,
                              t4.ExpirationDate,
                              t4.ProductStatus,
                              t4.LocationQuantity
                          }
             ).ToList();
            var gpdata = result.GroupBy(x => new { x.CustomerId, x.ProductId, x.Unit })
                       .Select(b => new
                       {
                           CustomerId = b.Key.CustomerId,
                           ProductId = b.Key.ProductId,
                           Unit = b.Key.Unit,
                           Quantity = b.Select(bn => bn.LocationQuantity * (bn.InventoryKind == "O" ? -1 : 1)).Sum()
                       }).Distinct().Join(_SAFETYContext.Product, g => g.ProductId, p => p.ProductId, (g, p) => new {
                           g.CustomerId,
                           g.ProductId,
                           g.Unit,
                           g.Quantity,
                           p.ProductName,
                           p.ProductCode
                       }).ToList();

            return WriteJsonOk("", gpdata);
        }
    }
}
