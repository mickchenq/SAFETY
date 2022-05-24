using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.SysUsers.Home
{
    [Area("SysUsers")]
    public class HomeController : Controller
    {

        [CustomAuth(FunctionEnum.使用者資料維護)]
        public IActionResult AddUser()
        {
            return View();
        }

        [CustomAuth(FunctionEnum.角色資料維護)]
        public IActionResult AddUserRole()
        {
            return View();
        }

        [CustomAuth(FunctionEnum.角色資料維護)]
        public IActionResult SetUserRoleFunction(int id)
        {
            UserRole model = new UserRole();
            model.UserRoleId = id;
            return View(model);
        }




    }
}
