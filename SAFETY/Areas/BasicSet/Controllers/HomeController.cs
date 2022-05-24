using SAFETY.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.BasicSet.Controllers
{
    [Area("BasicSet")]
    public class HomeController : Controller
    {
        /// <summary>
        /// 溫別設定
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.溫別設定)]
        public IActionResult TempLayer()
        {
            return View();
        }

        /// <summary>
        /// 倉別類型資料維護
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.倉別類型資料維護)]
        public IActionResult HouseType()
        {
            return View();
        }


        /// <summary>
        /// 倉別資料維護
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.倉別資料維護)]
        public IActionResult House()
        {
            return View();
        }

        /// <summary>
        /// 庫別類型設定
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.庫別類型資料維護)]
        public IActionResult RoomType()
        {
            return View();
        }


        /// <summary>
        /// 庫別設定
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.庫別資料維護)]
        public IActionResult Room()
        {
            return View();
        }

        /// <summary>
        /// 貨架類型資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.貨架類型)]
        public IActionResult ShelfType()
        {
            return View();
        }

        /// <summary>
        /// 貨架資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.貨架資料)]
        public IActionResult Shelf()
        {
            return View();
        }

        /// <summary>
        /// 物流中心
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.物流中心)]
        public IActionResult DataCenter()
        {
            return View();
        }

        /// <summary>
        /// 儲區
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.儲區資料)]
        public IActionResult Area()
        {
            return View();
        }

        /// <summary>
        /// 層架
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.層架資料)]
        public IActionResult Layer()
        {
            return View();
        }

        /// <summary>
        /// 儲位
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.儲位資料)]
        public IActionResult Location()
        {
            return View();
        }

    }
}
