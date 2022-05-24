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
    public class ProdTypeApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public ProdTypeApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryProductType([FromBody] QueryProductType model)
        {
            var res = _SAFETYContext.ProductType.AsQueryable();

            if (!string.IsNullOrEmpty(model.TypeCode))
            {
                res = res.Where(x => x.TypeCode.Trim() == model.TypeCode.Trim());
            }
            if (!string.IsNullOrEmpty(model.TypeName))
            {
                res = res.Where(x => x.TypeName.Trim().Contains(model.TypeName.Trim()));
            }

            if (!string.IsNullOrEmpty(model.IsStop))
            {
                res = res.Where(x => x.IsStop == model.IsStop);
            }

            res.OrderBy(x => x.TypeCode);
            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveProductType([FromBody] ProductType model)
        {          
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var info = await _SAFETYContext.ProductType.Where(x => x.TypeCode.Trim() == model.TypeCode.Trim() && (model.TypeId == 0 || x.TypeId != model.TypeId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["商品類型代碼已存在"]);
            }
            info = await _SAFETYContext.ProductType.Where(x => x.TypeName.Trim() == model.TypeName.Trim() && (model.TypeId == 0 || x.TypeId != model.TypeId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["商品類型名稱已存在"]);
            }

            int status = 0;
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            if (model.TypeId == 0)
            {
                model.CreateId = _user.SysUser.UserId;
                model.CreateDate = DateTime.Now;
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.ProductType.Add(model);
                status = 0;   //新增
            }
            else
            {
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.ProductType.Update(model);
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
        public async Task<IActionResult> DeleteProductType([FromBody] ProductType model)
        {
            //檢查是否已被使用
            var isUsed = _SAFETYContext.Product.Where(x => x.TypeId == model.TypeId).Select(x => x.TypeId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已設定商品資料，故不可刪除資料"]);

            _SAFETYContext.ProductType.Remove(model);
            var res = await _SAFETYContext.SaveChangesAsync();
            return res > 0
              ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        //end class
    }
}
