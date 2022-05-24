using SAFETYModel;
using SAFETYModel.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SAFETY.Infrastructure
{
    public class CustomAuthAttribute : TypeFilterAttribute
    {

        public CustomAuthAttribute(FunctionEnum FunctionId) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { (int)FunctionId };
        }

    }

    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly int _FunctionId;

        public ClaimRequirementFilter(int FunctionId)
        {
            _FunctionId = FunctionId;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var hasClaim = context.HttpContext.Session..HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            try
            {
                var value = context.HttpContext.Session.GetString("_sysUser");

                UserData user = JsonConvert.DeserializeObject<UserData>(value);
                var hasClaim = user.UserRoleFunction.Any(x => x.FunctionId == _FunctionId);
                if (!hasClaim)
                {
                    context.Result = new RedirectResult("~/Home/Index");
                }
            }
            catch (Exception)
            {
                context.Result = new RedirectResult("~/Login/Index");
            }
            
        }
    }


    public enum FunctionEnum
    {
        溫別設定 = 1
        , 物流中心
        , 倉別類型資料維護
        , 倉別資料維護
        , 庫別類型資料維護
        , 庫別資料維護
        , 儲區資料
        , 貨架類型
        , 貨架資料
        , 層架資料
        , 儲位資料
        , 客戶資料維護
        , 供應商資料維護
        , 商品分類資料維護
        , 商品類型資料維護
        , 商品資料維護
        , 商品包裝資料維護
        , 進貨通知資料維護
        , 上架儲位指派
        , 出貨通知資料維護
        , 出庫作業
        , 盤點單
        , 庫存調整維護
        , 每日庫存量查詢
        , 儲位異動維護
        , 儲位商品狀況查詢
        , 儲位可視化
        , 退貨通知資料維護
        , 角色資料維護
        , 使用者資料維護

    }
    public static class CustomerUserExtesion
    {
        /// <summary>
        /// 判斷使用者是否擁有權限
        /// </summary>
        /// <param name="user">使用者</param>
        /// <param name="function">功能</param>
        /// <returns></returns>
        public static bool IsHavePermission(this UserData user, FunctionEnum function)
        {
            bool result = false;
            if (user.UserRoleFunction != null)
            {
                result = user.UserRoleFunction.Any(x => x.FunctionId == (int)function);
            }
            return result;
        }
    }


}
