using SAFETY.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.CustMgmt.Controllers
{
    [Area("CustMgmt")]
    public class HomeController : Controller
    {
        /// <summary>
        /// 客戶基本資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.客戶資料維護)]
        public IActionResult Customer()
        {
            return View();
        }

        /// <summary>
        /// 供應商基本資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.供應商資料維護)]
        public IActionResult Supplier()
        {
            return View();
        }

        /// <summary>
        /// 客戶商品分類資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.商品分類資料維護)]
        public IActionResult ProdCategory()
        {
            return View();
        }

        /// <summary>
        /// 客戶商品類型資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.商品類型資料維護)]
        public IActionResult ProdType()
        {
            return View();
        }

        /// <summary>
        /// 商品資料基本資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.商品資料維護)]
        public IActionResult Product()
        {
            return View();
        }

        /// <summary>
        /// 客戶商品包裝資料
        /// </summary>
        /// <returns></returns>
        [CustomAuth(FunctionEnum.商品包裝資料維護)]
        public IActionResult ProdPackage()
        {
            return View();
        }

        //end class
    }
}
