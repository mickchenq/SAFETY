using SAFETYModel.ViewModel.BasicSet;
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
using SAFETYModel;

namespace SAFETY.Areas.BasicSet.API
{
    [Area("BasicSet")]
    [Route("api/[area]/[controller]/[action]")]
    public class LocationApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public LocationApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryLocation([FromBody] QueryLocation querydata)
        {
            var res = (from t1 in _SAFETYContext.Location
                       join t2 in _SAFETYContext.Layer on t1.LayerId equals t2.LayerId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       select new
                       { 
                            t1.LocationId,
                            t1.LocationCode,
                            t1.LayerId,
                            t2.LayerCode,
                            t1.TagGateWay,
                            t1.TagAddr,
                            t1.SequenceNo,
                            t1.PlateQuantity,
                            t1.MedianX,
                            t1.MedianY,
                            t1.Width,
                            t1.IsStackable,
                            t1.IsMixable,
                            t1.Square,
                            t1.Weight,
                            t1.Remarks,
                            t1.IsStop,
                            t1.CreateDate,
                            t1.CreateId
                       }
                );

            if (!string.IsNullOrEmpty(querydata.LocationCode))
            {
                res = res.Where(x => x.LocationCode.Trim().Contains(querydata.LocationCode.Trim()));
            }
            if (querydata.LayerId > 0)
            {
                res = res.Where(x => x.LayerId.ToString().Trim().Contains(querydata.LayerId.ToString().Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.IsStackable))
            {
                res = res.Where(x => x.IsStackable.ToString() == querydata.IsStackable.ToString());
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
        public async Task<IActionResult> UpdateLocation([FromBody] Location model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.LocationId == 0)
            {
                var LocationInfo = await _SAFETYContext.Location.FirstOrDefaultAsync(p => p.LocationCode == model.LocationCode);
                if (LocationInfo != null)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.Location.Add(model);
                status = 0;   //新增
            }
            else
            {
                var LocationInfo = await _SAFETYContext.Location.Where(p => p.LocationCode == model.LocationCode && p.LocationId != model.LocationId).ToListAsync();
                if (LocationInfo.Count > 0)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }

                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Location.Update(model);
                status = 1;   //修改
            }
            var res = 0;
            try
            {
                 res = await _SAFETYContext.SaveChangesAsync();
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

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
        public async Task<IActionResult> DeleteLocation([FromBody] Location model)
        {
            try
            {
                var LocationInfo = await _SAFETYContext.Location.FirstOrDefaultAsync(p => p.LocationId == model.LocationId);
                _SAFETYContext.Location.Remove(LocationInfo);
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
