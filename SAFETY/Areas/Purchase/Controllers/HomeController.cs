using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Infrastructure;

namespace SAFETY.Areas.Purchase.Controllers
{
    [Area("Purchase")]
    public class HomeController : Controller
    {

        /// <summary>
        /// 進貨通知列表頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.進貨通知資料維護)]
        public IActionResult PurchaseNoticeList()
        {
            return View();
        }

        /// <summary>
        /// 進貨通知
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.進貨通知資料維護)]
        public IActionResult PurchaseNotice(int id)
        {
            FullNotice model = new FullNotice();
            model.NotificationOrder = new NotificationOrder();
            model.NotificationOrder.OrderId = id;
            return View(model);
        }

        /// <summary>
        /// 上架儲位指派(入庫)列表頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.上架儲位指派)]
        public IActionResult StockInList()
        {
            return View();
        }

        /// <summary>
        ///  上架儲位指派(入庫)
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.上架儲位指派)]
        public IActionResult StockIn(int id)
        {
            FullStockIn model = new FullStockIn();
            model.StockInOrder = new StockInOrder();
            model.StockInOrder.OrderId = id;
            return View(model);
        }

    }
}
