using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using SAFETY.Resources;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SAFETY.Areas.BasicSet.API
{
    [Area("BasicSet")]
    [Route("api/[area]/[controller]/[action]")]
    public class ShelfTypeApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public ShelfTypeApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryShelfType([FromBody] QueryShelfType querydata)
        {
            var res = _SAFETYContext.ShelfType.Where(x => true);

            if (!string.IsNullOrEmpty(querydata.ShelfTypeCode))
            {
                res = res.Where(x => x.ShelfTypeCode.Trim().Contains(querydata.ShelfTypeCode.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.ShelfTypeName))
            {
                res = res.Where(x => x.ShelfTypeName.Trim().Contains(querydata.ShelfTypeName.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.IsFlat))
            {
                res = res.Where(x => x.IsFlat == querydata.IsFlat);
            }
            if (!string.IsNullOrEmpty(querydata.IsStop))
            {
                res = res.Where(x => x.IsStop == querydata.IsStop);
            }

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateShelfType([FromBody] ShelfType model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.ShelfTypeId == 0)
            {
                var ShelfTypeInfo = await _SAFETYContext.ShelfType.FirstOrDefaultAsync(p => p.ShelfTypeCode == model.ShelfTypeCode);
                if (ShelfTypeInfo != null)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.ShelfType.Add(model);
                status = 0;   //新增
            }
            else
            {
                var ShelfTypeInfo = await _SAFETYContext.ShelfType.Where(p => p.ShelfTypeCode == model.ShelfTypeCode && p.ShelfTypeId != model.ShelfTypeId).ToListAsync();
                if (ShelfTypeInfo.Count > 0)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }

                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.ShelfType.Update(model);
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
        public async Task<IActionResult> DeleteShelfType([FromBody] ShelfType model)
        {
            try
            {
                var ShelfTypeInfo = await _SAFETYContext.ShelfType.FirstOrDefaultAsync(p => p.ShelfTypeId == model.ShelfTypeId);
                _SAFETYContext.ShelfType.Remove(ShelfTypeInfo);
                var res = await _SAFETYContext.SaveChangesAsync();
                return res > 0
                   ? WriteJsonOk(_localizer[$"刪除成功"])
                   : WriteJsonErr(_localizer[$"刪除失敗"]);
            }
            catch (Exception err)
            {
                return WriteJsonErr(err.Message, null);
            }

        }


    }
}
