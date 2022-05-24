using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Controllers
{
    /// <summary>
    /// 擴展的基底 Controller 繼承類別
    /// </summary>
    public class BaseController : Controller
    {
        #region Api回傳格式共用
        /// <summary>
        /// 成功回傳
        /// </summary>
        /// <param name="msg">訊息</param>
        /// <param name="data">資料
        /// </param>
        /// <returns></returns>
        protected IActionResult WriteJsonOk(string msg, object dt = null)
        {
            return Ok(new
            {
                isOk = true,
                msg,
                dt
            });
        }
        /// <summary>
        /// 失敗回傳
        /// </summary>
        /// <param name="msg">訊息</param>
        ///<param name="data">資料</param>
        /// <returns></returns>
        protected IActionResult WriteJsonErr(string msg, object dt = null)
        {
            return Ok(new
            {
                isOk = false,
                msg,
                dt
            });
        }

        #endregion

        /// <summary>
        /// Model 驗證回傳
        /// </summary>
        /// <returns></returns>
        protected IActionResult ModelValidate()
        {
            var errmsg = string.Join("<br>", ModelState.Values.SelectMany(v => v.Errors)
                                                             .Select(e => e.ErrorMessage));
            return WriteJsonErr(errmsg);
        }
    }
}
