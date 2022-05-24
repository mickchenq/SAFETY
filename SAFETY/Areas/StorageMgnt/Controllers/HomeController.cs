using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Infrastructure;

namespace SAFETY.Areas.StorageMgnt.Controllers
{
    [Area("StorageMgnt")]
    public class HomeController : Controller
    {
        [CustomAuth(FunctionEnum.儲位可視化)]
        public IActionResult StorageVisualization()
        {
            return View();
        }

        [CustomAuth(FunctionEnum.儲位異動維護)]
        public IActionResult LocationChangeList()
        {
            return View();
        }

        [CustomAuth(FunctionEnum.儲位異動維護)]
        public IActionResult LocationChange(int id)
        {
            LocationChangeOrder model = new LocationChangeOrder(); 
            model.OrderId = id;
            return View(model);
        }

        [CustomAuth(FunctionEnum.儲位商品狀況查詢)]
        public IActionResult LocationProductQuery()
        {
            return View();
        }

    }
}
