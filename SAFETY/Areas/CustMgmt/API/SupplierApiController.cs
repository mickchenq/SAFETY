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
    public class SupplierApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public SupplierApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QuerySupplier([FromBody] QuerySupplier model)
        {
            var res = _SAFETYContext.Supplier.AsQueryable();

            if (!string.IsNullOrEmpty(model.SupplierCode))
            {
                res = res.Where(x => x.SupplierCode.Trim() == model.SupplierCode.Trim());
            }
            if (!string.IsNullOrEmpty(model.SupplierName))
            {
                res = res.Where(x => x.SupplierName.Trim().Contains(model.SupplierName.Trim()));
            }

            if (!string.IsNullOrEmpty(model.IsStop))
            {
                res = res.Where(x => x.IsStop == model.IsStop);
            }

            res.OrderBy(x => x.SupplierCode);
            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveSupplier([FromBody] Supplier model)
        {           
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var info = await _SAFETYContext.Supplier.Where(x => x.SupplierCode.Trim() == model.SupplierCode.Trim() && (model.SupplierId == 0 || x.SupplierId != model.SupplierId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {              
                return WriteJsonErr(_localizer["供應商代碼已存在"]);
            }
            info = await _SAFETYContext.Supplier.Where(x => x.SupplierName.Trim() == model.SupplierName.Trim() && (model.SupplierId == 0 || x.SupplierId != model.SupplierId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {             
                return WriteJsonErr(_localizer["供應商名稱已存在"]);
            }

            int status = 0;
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            if (model.SupplierId == 0)
            {
                model.CreateId = _user.SysUser.UserId;
                model.CreateDate = DateTime.Now;
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Supplier.Add(model);
                status = 0;   //新增
            }
            else
            {
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Supplier.Update(model);
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
        public async Task<IActionResult> DeleteSupplier([FromBody] Supplier model)
        {
            //檢查是否已被使用
            var isUsed = _SAFETYContext.Product.Where(x => x.SupplierId == model.SupplierId).Select(x => x.SupplierId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已設定商品資料，故不可刪除資料"]);

            _SAFETYContext.Supplier.Remove(model);
            var res = await _SAFETYContext.SaveChangesAsync();
            return res > 0
               ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        //end class
    }
}
