using SAFETYModel.DBModels;
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

namespace SAFETY.Areas.Stock.API
{
    [Area("Stock")]
    [Route("api/[area]/[controller]/[action]")]
    public class DashboardApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public DashboardApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
        }

        public IActionResult QueryStockInData()
        {
            DateTime today = DateTime.Today;
            DateTime Q1 = new DateTime(today.Year, 4, 1);
            DateTime Q2 = new DateTime(today.Year, 7, 1);
            DateTime Q3 = new DateTime(today.Year, 10, 1);
            DateTime Q4 = new DateTime(today.Year + 1, 1, 1);
            var Inv = _SAFETYContext.Inventory.Where(x=>x.InventoryKind == "I").Where(x => x.InvDate < Q4).ToList();
            int Q1_Amount = Inv.Where(x => x.InvDate < Q1).Sum(x => x.LocationQuantity);
            int Q2_Amount = Inv.Where(x => x.InvDate < Q2).Sum(x => x.LocationQuantity);
            int Q3_Amount = Inv.Where(x => x.InvDate < Q3).Sum(x => x.LocationQuantity);
            int Q4_Amount = Inv.Where(x => x.InvDate < Q4).Sum(x => x.LocationQuantity);
            int[] result = { Q1_Amount, Q2_Amount, Q3_Amount, Q4_Amount };
            return WriteJsonOk("", result);
        }
        public IActionResult QueryStockOutData()
        {
            DateTime today = DateTime.Today;
            DateTime Q1 = new DateTime(today.Year, 4, 1);
            DateTime Q2 = new DateTime(today.Year, 7, 1);
            DateTime Q3 = new DateTime(today.Year, 10, 1);
            DateTime Q4 = new DateTime(today.Year + 1, 1, 1);
            var Inv = _SAFETYContext.Inventory.Where(x => x.InventoryKind == "O").Where(x => x.InvDate < Q4).ToList();
            int Q1_Amount = Inv.Where(x => x.InvDate < Q1).Sum(x => x.LocationQuantity);
            int Q2_Amount = Inv.Where(x => x.InvDate < Q2).Sum(x => x.LocationQuantity);
            int Q3_Amount = Inv.Where(x => x.InvDate < Q3).Sum(x => x.LocationQuantity);
            int Q4_Amount = Inv.Where(x => x.InvDate < Q4).Sum(x => x.LocationQuantity);
            int[] result = { Q1_Amount, Q2_Amount, Q3_Amount, Q4_Amount };
            return WriteJsonOk("", result);
        }
        public IActionResult QueryInventoryData()
        {
            DateTime today = DateTime.Today;
            DateTime Q1 = new DateTime(today.Year, 4, 1);
            DateTime Q2 = new DateTime(today.Year, 7, 1);
            DateTime Q3 = new DateTime(today.Year, 10, 1);
            DateTime Q4 = new DateTime(today.Year + 1, 1, 1);
            var Inv = _SAFETYContext.Inventory.Where(x => x.InvDate < Q4).ToList();
            int Q1_Amount = Inv.Where(x => x.InvDate < Q1).Sum(x => (x.InventoryKind == "O" ? -1 : 1) * x.LocationQuantity);
            int Q2_Amount = Inv.Where(x => x.InvDate < Q2).Sum(x => (x.InventoryKind == "O" ? -1 : 1) * x.LocationQuantity);
            int Q3_Amount = Inv.Where(x => x.InvDate < Q3).Sum(x => (x.InventoryKind == "O" ? -1 : 1) * x.LocationQuantity);
            int Q4_Amount = Inv.Where(x => x.InvDate < Q4).Sum(x => (x.InventoryKind == "O" ? -1 : 1) * x.LocationQuantity);
            int[] result = { Q1_Amount, Q2_Amount, Q3_Amount, Q4_Amount };
            return WriteJsonOk("", result);
        }
        public IActionResult QueryStorageUsedRate()
        {
            // 根據 LocationId 去抓
            var lo = (
                          from t3 in _SAFETYContext.Location
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
            var gpdata = lo.GroupBy(x => new { x.LocationId })
                       .Select(b => new {
                           LocationId = b.Key.LocationId,
                           Quantity = b.Select(bn => bn.LocationQuantity * (bn.InventoryKind == "O" ? -1 : 1)).Sum()
                       });

            int AllLocationCount = gpdata.Count();
            int UsedLocationCount = gpdata.Where(x => x.Quantity > 0).Count();

            double UsedRate = (((double)UsedLocationCount / AllLocationCount) * 100);
            UsedRate = Math.Round(UsedRate, 1);
            double[] result = { UsedRate, 100 - UsedRate };
            return WriteJsonOk("", result);
        }
    }
}
