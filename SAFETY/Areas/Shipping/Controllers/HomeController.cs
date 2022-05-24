using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Infrastructure;

namespace SAFETY.Areas.Shipping.Controllers
{
    [Area("Shipping")]
    public class HomeController : Controller
    {
        /// <summary>
        /// 出貨通知列表頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.出貨通知資料維護)]
        public IActionResult ShippingNoticeList()
        {
            return View();
        }

        /// <summary>
        /// 出貨通知
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.出貨通知資料維護)]
        public IActionResult ShippingNotice(int id)
        {
            FullShipping model = new FullShipping();
            model.ShippingOrder = new ShippingOrder();
            model.ShippingOrder.OrderId = id;
            return View(model);
        }

        /// <summary>
        /// 出庫單列表頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.出庫作業)]
        public IActionResult StockOutList()
        {
            return View();
        }

        /// <summary>
        /// 出庫單頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.出庫作業)]
        public IActionResult StockOut(int id)
        {
            FullStockOut model = new FullStockOut();
            model.StockOutOrder = new StockOutOrder();
            model.StockOutOrder.OrderId = id;
            return View(model);
        }

        //end class
    }
}
