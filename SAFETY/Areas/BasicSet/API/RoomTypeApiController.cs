using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYModel.ViewModel;
using SAFETY.Controllers;
using SAFETY.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.BasicSet.API
{
    [Area("BasicSet")]
    [Route("api/[area]/[controller]/[action]")]
    public class RoomTypeApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public RoomTypeApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
        }

        public IActionResult QueryRoomType([FromBody] QueryRoomType query)
        {
            var roomtypes = _SAFETYContext.RoomType.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.RoomTypeName))
                roomtypes = roomtypes.Where(r => r.RoomTypeName.Contains(query.RoomTypeName));
            if (!string.IsNullOrWhiteSpace(query.RoomTypeCode))
                roomtypes = roomtypes.Where(r => r.RoomTypeCode.Contains(query.RoomTypeCode));
            if (!string.IsNullOrWhiteSpace(query.IsStop))
                roomtypes = roomtypes.Where(r => r.IsStop.Equals(query.IsStop));
            List<RoomType> result = roomtypes.ToList();
            return WriteJsonOk("", result);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateRoomType([FromBody] RoomType model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.RoomTypeId == 0)
            {
                var RoomTypeInfo = await _SAFETYContext.RoomType.FirstOrDefaultAsync(p => p.RoomTypeCode == model.RoomTypeCode);
                if (RoomTypeInfo != null)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.RoomType.Add(model);
                status = 0;   //新增
            }
            else
            {
                var RoomTypeInfo = await _SAFETYContext.RoomType.Where(p => p.RoomTypeCode == model.RoomTypeCode && p.RoomTypeId != model.RoomTypeId).ToListAsync();
                if (RoomTypeInfo.Count > 0)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }

                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.RoomType.Update(model);
                status = 1;   //修改
            }

            var res = await _SAFETYContext.SaveChangesAsync();
            if (status == 0)
                return res > 0  ? WriteJsonOk(_localizer["新增成功"]) : WriteJsonErr(_localizer["新增失敗"]);
            else
                return res > 0 ? WriteJsonOk(_localizer["修改成功"]) : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteRoomType([FromBody] RoomType model)
        {
            try
            {
                var RoomTypeInfo = await _SAFETYContext.RoomType.FirstOrDefaultAsync(p => p.RoomTypeId == model.RoomTypeId);
                _SAFETYContext.RoomType.Remove(RoomTypeInfo);
                var res = await _SAFETYContext.SaveChangesAsync();
                return res > 0
                   ? WriteJsonOk(_localizer["刪除成功"])
                   : WriteJsonErr(_localizer["刪除失敗"]);
            }
            catch (Exception err)
            {
                return WriteJsonErr(err.Message, null);
            }

        }

    }
}
