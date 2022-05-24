using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Controllers;

namespace SAFETY.Areas.CustMgmt.API
{
    [Area("CustMgmt")]
    [Route("api/[area]/[controller]/[action]")]

    public class CustomerApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public CustomerApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
        }

        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult QueryCustomer([FromBody] QueryCustomer model)
        {
            var res = _SAFETYContext.Customer.AsQueryable();

            if (!string.IsNullOrEmpty(model.CustomerCode))
            {
                res = res.Where(x => x.CustomerCode.Trim() == model.CustomerCode.Trim());
            }
            if (!string.IsNullOrEmpty(model.CustomerName))
            {
                res = res.Where(x => x.CustomerName.Trim().Contains(model.CustomerName.Trim()));
            }

            if (!string.IsNullOrEmpty(model.IsStop))
            {
                res = res.Where(x => x.IsStop == model.IsStop);
            }

            res.OrderBy(x => x.CustomerCode);
            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveCustomer([FromBody] Customer model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var info = await _SAFETYContext.Customer.Where(x => x.CustomerCode.Trim() == model.CustomerCode.Trim() && (model.CustomerId == 0 || x.CustomerId != model.CustomerId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["客戶代碼已存在"]);
            }
            info = await _SAFETYContext.Customer.Where(x => x.CustomerName.Trim() == model.CustomerName.Trim() && (model.CustomerId == 0 || x.CustomerId != model.CustomerId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["客戶名稱已存在"]);
            }

            int status = 0;
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            if (model.CustomerId == 0)
            {
                model.CreateId = _user.SysUser.UserId;
                model.CreateDate = DateTime.Now;
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Customer.Add(model);
                status = 0;   //新增
            }
            else
            {
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Customer.Update(model);
                status = 1;   //修改
            }
            var res = await _SAFETYContext.SaveChangesAsync();

            if (status == 0)
                return res > 0 ? WriteJsonOk(_localizer["新增成功"]) : WriteJsonErr(_localizer["新增失敗"]);
            else
                return res > 0 ? WriteJsonOk(_localizer["修改成功"]) : WriteJsonErr(_localizer["修改失敗"]);          
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteCustomer([FromBody] Customer model)
        {
            //檢查是否已被使用
            var isUsed = _SAFETYContext.Product.Where(x => x.CustomerId == model.CustomerId).Select(x => x.CustomerId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已設定商品資料，故不可刪除資料"]);

            isUsed =  _SAFETYContext.ShippingOrder.Where(x => x.CustomerId == model.CustomerId).Select(x => x.CustomerId).ToList();
            isUsed = isUsed.Union(_SAFETYContext.ProcessOrder.Where(x => x.CustomerId == model.CustomerId).Select(x => x.CustomerId)).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已出貨通知或加工通知，故不可刪除資料"]);

            _SAFETYContext.Customer.Remove(model);
            var res = await _SAFETYContext.SaveChangesAsync();
            return res > 0
               ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        //end class
    }
}
