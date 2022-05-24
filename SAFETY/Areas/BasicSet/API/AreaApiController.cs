using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYModel.ViewModel.BasicSet;
//using SAFETYModel.OldDBModels;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SAFETY.Areas.BasicSet.API
{
    [Area("BasicSet")]
    [Route("api/[area]/[controller]/[action]")]
    // [Authorize]
    public class AreaApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public AreaApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryArea([FromBody] QueryArea querydata)
        {
            var res = (from t1 in _SAFETYContext.Area
                       join t4 in _SAFETYContext.Room on t1.RoomId equals t4.RoomId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       join t3 in _SAFETYContext.House on t4.HouseId equals t3.HouseId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       join t2 in _SAFETYContext.DataCenter on t3.DcId equals t2.DcId into ps2
                       from t2 in ps2.DefaultIfEmpty()                      
                       select new
                       {
                           t1.AreaId,
                           t1.AreaCode,
                           t1.AreaName,
                           t4.HouseId,
                           t4.DcId,
                           t1.RoomId,
                           t1.Remarks,
                           t1.IsStop,
                           t3.HouseName,
                           t2.DcName,
                           t4.RoomName,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate
                       }
                      );

                //_SAFETYContext.Area.Where(x => true);

            if (!string.IsNullOrEmpty(querydata.AreaCode))
            {
                res = res.Where(x => x.AreaCode.Trim().Contains(querydata.AreaCode.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.AreaName))
            {
                res = res.Where(x => x.AreaName.Trim().Contains(querydata.AreaName.Trim()));
            }
            if (querydata.RoomId != null && querydata.RoomId.ToString() != "0")
            {
                int iValue = 0;
                int.TryParse(querydata.RoomId.ToString(), out iValue);
                res = res.Where(x => x.RoomId == iValue);
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
        public async Task<IActionResult> SaveArea([FromBody] Area model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.AreaId == 0)
            {
                var AreaInfo = await _SAFETYContext.Area.FirstOrDefaultAsync(p => p.AreaCode == model.AreaCode);
                if (AreaInfo != null)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.Area.Add(model);
                status = 0;   //新增
            }
            else
            {
                var AreaInfo = await _SAFETYContext.Area.Where(p => p.AreaCode == model.AreaCode && p.AreaId != model.AreaId).ToListAsync();
                if (AreaInfo.Count>0)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }

                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Area.Update(model);
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
        public async Task<IActionResult> DeleteArea([FromBody] Area model)
        {
            try
            {
                var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
                UserData user = JsonConvert.DeserializeObject<UserData>(value);

                var AreaInfo = await _SAFETYContext.Area.FirstOrDefaultAsync(p => p.AreaId == model.AreaId);
                _SAFETYContext.Area.Remove(AreaInfo);
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
