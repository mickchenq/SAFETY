using SAFETYModel.DBModels;
using SAFETYModel.OldDBModels;
using SAFETY.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SAFETYService;
using SAFETYModel;
using Microsoft.AspNetCore.Http;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;

namespace SAFETY.Controllers
{
    public class LoginController : BaseController
    {

        private readonly ILogger<LoginController> _logger;
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public LoginController(ILogger<LoginController> logger, SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
        {
            _logger = logger;
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] SysUser model)
        {
            UserData user = new UserData();
            user.SysUser = _SAFETYContext.SysUser.Where(x => x.LoginId == model.LoginId && x.LoginPwd == model.LoginPwd).FirstOrDefault();
            if (user.SysUser == null)
            {
                return WriteJsonErr(_localizer["登入失敗"]);
            }
            user.UserRoleFunction = _SAFETYContext.UserRoleFunction.Where(x => x.UserRoleId == user.SysUser.UserType).ToList();
            _IHttpContextAccessor.HttpContext.Session.SetString("_sysUser", JsonConvert.SerializeObject(user));
            if (user.SysUser != null)
                return WriteJsonOk(_localizer["登入成功"], user.SysUser.UserId);
            else
                return WriteJsonErr(_localizer["登入失敗"]);

        }

        public IActionResult LogOut()
        {
            _IHttpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }


    }
}
