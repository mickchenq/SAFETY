using SAFETYModel.DBModels;
using SAFETYModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETY.Infrastructure;

namespace SAFETY.Areas.Return.Controllers
{
    [Area("Return")]
    public class HomeController : Controller
    {
        /// <summary>
        /// 退貨通知列表頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.退貨通知資料維護)]
        public IActionResult ReturnList()
        {
            return View();
        }

        /// <summary>
        /// 出庫單頁
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.退貨通知資料維護)]
        public IActionResult ReturnedOrder(int id)
        {
            FullReturn model = new FullReturn();
            model.ReturnedOrder = new ReturnedOrder();
            model.ReturnedOrder.OrderId = id;
            return View(model);
        }


    }
}
