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
    public class HouseTypeApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public HouseTypeApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryHouseType([FromBody] QueryHouseType querydata)
        {
            var res = _SAFETYContext.HouseType.Where(x => true);

            if (!string.IsNullOrEmpty(querydata.temp_code))
            {
                res = res.Where(x => x.HouseTypeCode.Trim().Contains(querydata.temp_code.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.temp_name))
            {
                res = res.Where(x => x.HouseTypeName.Trim().Contains(querydata.temp_name.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.is_stop))
            {
                res = res.Where(x => x.IsStop == querydata.is_stop);
            }

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateHouseType([FromBody] HouseType model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.HouseTypeId == 0)
            {
                var HouseTypeInfo = await _SAFETYContext.HouseType.FirstOrDefaultAsync(p => p.HouseTypeCode == model.HouseTypeCode);
                if (HouseTypeInfo != null)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }
                model.CreateId = user.SysUser.UserId; //待串人員驗證
                _SAFETYContext.HouseType.Add(model);
                status = 0;   //新增
            }
            else
            {
                var HouseTypeInfo = await _SAFETYContext.HouseType.Where(p => p.HouseTypeCode == model.HouseTypeCode && p.HouseTypeId != model.HouseTypeId).ToListAsync();
                if (HouseTypeInfo.Count > 0)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }

                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.HouseType.Update(model);
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
        public async Task<IActionResult> DeleteHouseType([FromBody] HouseType model)
        {
            try
            {
                var HouseTypeInfo = await _SAFETYContext.HouseType.FirstOrDefaultAsync(p => p.HouseTypeId == model.HouseTypeId);
                _SAFETYContext.HouseType.Remove(HouseTypeInfo);
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
