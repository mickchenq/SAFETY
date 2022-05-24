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
    public class ShelfApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public ShelfApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryShelf([FromBody] QueryShelf querydata)
        {
            var res = (from t1 in _SAFETYContext.Shelf
                       join t2 in _SAFETYContext.Area on t1.AreaId equals t2.AreaId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.ShelfType on t1.ShelfTypeId equals t3.ShelfTypeId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       join t4 in _SAFETYContext.Shelf on t1.PrevShelfId equals t4.ShelfId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       join t5 in _SAFETYContext.Shelf on t1.NextShelfId equals t5.ShelfId into ps5
                       from t5 in ps5.DefaultIfEmpty()
                       select new
                       { 
                            t1.ShelfId,
                            t1.ShelfCode,
                            t1.AreaId,
                            t2.AreaName,
                            t1.ShelfTypeId,
                            t3.ShelfTypeName,
                            t1.Racks,
                            t1.PrevShelfId,
                            PrevShelfCode = t4.ShelfCode,
                            t1.NextShelfId,
                            NextShelfCode = t5.ShelfCode,
                            t1.DownAisleWidth,
                            t1.UpAisleWidth,
                            t1.LeftAisleWidth,
                            t1.RightAisleWidth,
                            t1.Width,
                            t1.Length,
                            t1.X1,
                            t1.X2,
                            t1.Y1,
                            t1.Y2,
                            t1.Remarks,
                            t1.IsStop,
                            t1.CreateDate,
                            t1.CreateId
                       }
                );

            if (!string.IsNullOrEmpty(querydata.ShelfCode))
            {
                res = res.Where(x => x.ShelfCode.Trim().Contains(querydata.ShelfCode.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.AreaId))
            {
                res = res.Where(x => x.AreaId.ToString().Trim().Contains(querydata.AreaId.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.ShelfTypeId))
            {
                res = res.Where(x => x.ShelfTypeId.ToString() == querydata.ShelfTypeId.ToString());
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
        public async Task<IActionResult> UpdateShelf([FromBody] Shelf model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.ShelfId == 0)
            {
                var ShelfInfo = await _SAFETYContext.Shelf.FirstOrDefaultAsync(p => p.ShelfCode == model.ShelfCode);
                if (ShelfInfo != null)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.Shelf.Add(model);
                status = 0;   //新增
            }
            else
            {
                var ShelfInfo = await _SAFETYContext.Shelf.Where(p => p.ShelfCode == model.ShelfCode && p.ShelfId != model.ShelfId).ToListAsync();
                if (ShelfInfo.Count > 0)
                {
                    return WriteJsonErr(_localizer["代碼已存在，請重新輸入!"]);
                }

                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Shelf.Update(model);
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
        public async Task<IActionResult> DeleteShelf([FromBody] Shelf model)
        {
            try
            {
                var ShelfInfo = await _SAFETYContext.Shelf.FirstOrDefaultAsync(p => p.ShelfId == model.ShelfId);
                _SAFETYContext.Shelf.Remove(ShelfInfo);
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
