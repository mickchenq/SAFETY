using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Infrastructure;

namespace SAFETY.Areas.Stock.Controllers
{
    [Area("Stock")]
    public class HomeController : Controller
    {
        /// <summary>
        /// 盤點單列表頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.盤點單)]
        public IActionResult InventoryList()
        {
            return View();
        }

        /// <summary>
        /// 盤點單
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.盤點單)]
        public IActionResult Inventory(int id)
        {
            FullInventory model = new FullInventory();
            model.StockAdjustment = new StockAdjustment();
            model.StockAdjustment.OrderId = id;
            return View(model);
        }

        /// <summary>
        /// 庫存調整單列表頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.庫存調整維護)]
        public IActionResult StockChangeList()
        {
            return View();
        }

        /// <summary>
        /// 庫存調整單
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.庫存調整維護)]
        public IActionResult StockChange(int id)
        {
            StockChangeOrder model = new StockChangeOrder();
            model.OrderId = id;
            return View(model);
        }

        /// <summary>
        /// 每日庫存量查詢
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.每日庫存量查詢)]
        public IActionResult DailyStockQuery()
        {
            return View();
        }

        //end class
    }
}
