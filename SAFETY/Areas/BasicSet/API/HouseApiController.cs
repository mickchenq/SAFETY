using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFETY.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SAFETY.Areas.BasicSet.API
{
    [Area("BasicSet")]
    [Route("api/[area]/[controller]/[action]")]
    public class HouseApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public HouseApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public async Task<IActionResult> QueryHouse([FromBody] QueryHouse model)
        {            
            var res = (from t1 in _SAFETYContext.House
                       join t2 in _SAFETYContext.HouseType on t1.HouseTypeId equals t2.HouseTypeId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.DataCenter on t1.DcId equals t3.DcId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       select new
                       {
                           t1.HouseId,
                           t1.HouseCode,
                           t1.HouseName,
                           t1.HouseTypeId,
                           t1.DcId,
                           t1.Remarks,
                           t1.IsStop,
                           t2.HouseTypeName,
                           t3.DcName,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate                          
                       }
                );
         
            if (!string.IsNullOrEmpty(model.HouseCode))
            {
                res = res.Where(x => x.HouseCode.Trim() == model.HouseCode.Trim());
            }
            if (!string.IsNullOrEmpty(model.HouseName))
            {
                res = res.Where(x => x.HouseName.Trim().Contains(model.HouseName.Trim()));
            }
            if (model.HouseTypeId != null && model.HouseTypeId.ToString() != "0")
            {
                int iValue = 0;
                int.TryParse(model.HouseTypeId.ToString(), out iValue);
                res = res.Where(x => x.HouseTypeId == iValue);
            }
            if (model.DcId != null && model.DcId.ToString() != "0")
            {
                int iValue=0;
                int.TryParse(model.DcId.ToString(), out iValue);
                res = res.Where(x => x.DcId == iValue);
            }
            if (!string.IsNullOrEmpty(model.IsStop))
            {
                res = res.Where(x => x.IsStop == model.IsStop);
            }

            res.OrderBy(x => x.HouseCode);
            var result = await res.ToListAsync();
            return WriteJsonOk("", result);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveHouse([FromBody] House model)
        {          
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            if (model.HouseTypeId.ToString().Trim() == "0")
            {
                return WriteJsonErr(_localizer["倉別類型必填"]);
            }
            if (model.DcId.ToString().Trim() == "0")
            {
                return WriteJsonErr(_localizer["物流中心必填"]);
            }
            
            var info = await _SAFETYContext.House.Where(x => x.HouseCode.Trim() == model.HouseCode.Trim() && (model.HouseId==0 || x.HouseId != model.HouseId)).ToListAsync();
            if (info.Any() || info.Count>0)
            {
                return WriteJsonErr(_localizer["倉別代碼已存在"]);
            }
            info = await _SAFETYContext.House.Where(x => x.HouseName.Trim() == model.HouseName.Trim() && (model.HouseId == 0 || x.HouseId != model.HouseId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["倉別名稱已存在"]);
            }

            int status = 0;
            if (model.HouseId == 0)
            {
                model.CreateId = user.SysUser.UserId; //待串人員驗證
                model.CreateDate = DateTime.Now;
                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.House.Add(model);
                status = 0;   //新增
            }
            else
            {
                model.ModifyId = user.SysUser.UserId; //待串人員驗證
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.House.Update(model);
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
        public async Task<IActionResult> DeleteHouse([FromBody] House model)
        {
            //檢查是否已被使用
            var isUsed = await _SAFETYContext.Room.Where(x => x.HouseId == model.HouseId).Select(x => x.HouseId).ToListAsync();          
            if (isUsed.Any() || isUsed.Count>0)
                return WriteJsonErr(_localizer["已設定庫別資料，故不可刪除資料"]);

            _SAFETYContext.House.Remove(model);
            var res =await _SAFETYContext.SaveChangesAsync();
            return res > 0
               ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        //end class
    }
}
