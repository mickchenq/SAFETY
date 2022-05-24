using SAFETY.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public HomeController(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }
        
        public IActionResult Index()
        {
            var sss= _localizer["AccountExist"]; ;
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
    }
}
