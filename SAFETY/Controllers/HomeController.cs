using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYModel.OldDBModels;
using SAFETY.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SAFETYContext _SAFETYContext;
        private IHttpContextAccessor _IHttpContextAccessor;
        public HomeController(ILogger<HomeController> logger, SAFETYContext SAFETYContext, IHttpContextAccessor iHttpContextAccessor)
        {
            _logger = logger;
            _SAFETYContext = SAFETYContext;
            _IHttpContextAccessor = iHttpContextAccessor;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeLanguage(string Culture)
        {
            if (Culture != null)
            {
                string value = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(Culture));
                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, value);                
            }
            return RedirectToAction("Index");
        }

        public IActionResult GetLoginInfo()
        {
            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData model = JsonConvert.DeserializeObject<UserData>(value);

            return WriteJsonOk("", model);
        }


    }
}
