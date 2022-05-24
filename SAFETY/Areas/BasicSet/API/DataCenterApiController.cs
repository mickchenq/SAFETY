using SAFETYModel;
using SAFETYModel.DBModels;
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
    // [Authorize]
    public class DataCenterApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public DataCenterApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryDataCenter([FromBody] QueryDataCenter querydata)
        {
            var res = _SAFETYContext.DataCenter.Where(x => true);

            if (!string.IsNullOrEmpty(querydata.DcCode))
            {
                res = res.Where(x => x.DcCode.Trim().Contains(querydata.DcCode.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.DcName))
            {
                res = res.Where(x => x.DcName.Trim().Contains(querydata.DcName.Trim()));
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
        public async Task<IActionResult> UpdateDataCenter([FromBody] DataCenter model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.DcId == 0)
            {
                var DataCenterInfo = await _SAFETYContext.DataCenter.FirstOrDefaultAsync(p => p.DcCode == model.DcCode);
                if (DataCenterInfo != null)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.DataCenter.Add(model);
                status = 0;   //新增
            }
            else
            {
                var DataCenterInfo = await _SAFETYContext.DataCenter.Where(p => p.DcCode == model.DcCode && p.DcId != model.DcId).ToListAsync();
                if (DataCenterInfo.Count>0)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }

                model.ModifyId = user.SysUser.UserId; ; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.DataCenter.Update(model);
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
        public async Task<IActionResult> DeleteDataCenter([FromBody] DataCenter model)
        {
            try
            {
                var DataCenterInfo = await _SAFETYContext.DataCenter.FirstOrDefaultAsync(p => p.DcId == model.DcId);
                _SAFETYContext.DataCenter.Remove(DataCenterInfo);
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
