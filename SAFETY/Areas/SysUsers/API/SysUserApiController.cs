using SAFETYModel.DBModels;
using SAFETYModel;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SAFETY.Areas.SysUsers.API
{
    [Area("SysUsers")]
    [Route("api/[area]/[controller]/[action]")]
    public class SysUserApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public SysUserApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
        }

        #region 使用者帳號
        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult QuerySysUser([FromBody] QueryUser querydata)
        {
            var res = _SAFETYContext.SysUser.Where(x => true);


            if (!string.IsNullOrEmpty(querydata.LoginId))
            {
                res = res.Where(x => x.LoginId.Trim().Contains(querydata.LoginId.Trim()));
            }
            if (!string.IsNullOrEmpty(querydata.UserName))
            {
                res = res.Where(x => x.UserName.Trim().Contains(querydata.UserName.Trim()));
            }
            if (querydata.UserType != 0)
            {
                res = res.Where(x => x.UserType == querydata.UserType);
            }
            if (!string.IsNullOrEmpty(querydata.IsStop))
            {
                res = res.Where(x => x.IsStop == querydata.IsStop);
            }

            return WriteJsonOk("", res.ToList().OrderBy(x=>x.UserType));
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveSysUser([FromBody] SysUser model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.UserId == 0)
            {
                var SysUserInfo = await _SAFETYContext.SysUser.FirstOrDefaultAsync(p => p.LoginId == model.LoginId);
                if (SysUserInfo != null)
                {
                    return WriteJsonErr("使用者帳號已存在，請重新輸入!");
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.SysUser.Add(model);
                status = 0;   //新增
            }
            else
            {
                var SysUserInfo = await _SAFETYContext.SysUser.Where(p => p.UserId == model.UserId).FirstOrDefaultAsync();
                SysUserInfo.UserName = model.UserName;
                SysUserInfo.UserType = model.UserType;
                SysUserInfo.LoginPwd = model.LoginPwd;
                SysUserInfo.Remarks = model.Remarks;
                SysUserInfo.IsStop = model.IsStop;
                SysUserInfo.ModifyId = user.SysUser.UserId; //待串人員驗證
                SysUserInfo.ModifyDate = DateTime.Now;
                _SAFETYContext.SysUser.Update(SysUserInfo);
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
        public async Task<IActionResult> DeleteSysUser([FromBody] SysUser model)
        {
            try
            {
                var SysUserInfo = await _SAFETYContext.SysUser.FirstOrDefaultAsync(p => p.UserId == model.UserId);
                _SAFETYContext.SysUser.Remove(SysUserInfo);
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
        #endregion

        #region 使用者角色
        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult QueryUserRole([FromBody] QueryRole querydata)
        {
            var res = (from t1 in _SAFETYContext.UserRole
                       select new
                       { 
                        t1.UserRoleId,
                        t1.UserRoleName,
                        t1.Remarks,
                        t1.IsStop,
                        t1.CreateId,
                        t1.CreateDate,
                        t1.ModifyId,
                        t1.ModifyDate,
                        FnText= t1.UserRoleId.ToString()
                       });//_SAFETYContext.UserRole.Where(x => true);


            if (!string.IsNullOrEmpty(querydata.UserRoleName))
            {
                res = res.Where(x => x.UserRoleName.Trim().Contains(querydata.UserRoleName.Trim()));
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
        public async Task<IActionResult> SaveUserRole([FromBody] UserRole model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            int status = 0;
            if (model.UserRoleId == 0)
            {
                var SysUserInfo = await _SAFETYContext.UserRole.FirstOrDefaultAsync(p => p.UserRoleName == model.UserRoleName);
                if (SysUserInfo != null)
                {
                    return WriteJsonErr("使用者角色名稱已存在，請重新輸入!");
                }
                model.CreateId = user.SysUser.UserId;
                _SAFETYContext.UserRole.Add(model);
                status = 0;   //新增
            }
            else
            {
                var UserRoleInfo = await _SAFETYContext.UserRole.Where(p => p.UserRoleId == model.UserRoleId).FirstOrDefaultAsync();
                UserRoleInfo.UserRoleName = model.UserRoleName;
                UserRoleInfo.Remarks = model.Remarks;
                UserRoleInfo.IsStop = model.IsStop;
                UserRoleInfo.ModifyId = user.SysUser.UserId; //待串人員驗證
                UserRoleInfo.ModifyDate = DateTime.Now;
                _SAFETYContext.UserRole.Update(UserRoleInfo);
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
        public async Task<IActionResult> DeleteUserRole([FromBody] UserRole model)
        {
            try
            {
                var SysUserlist = await _SAFETYContext.SysUser.Where(p => p.UserType == model.UserRoleId).ToListAsync();
                if (SysUserlist.Count > 0)
                {
                    return WriteJsonErr(_localizer["該使用者角色已被使用，不可刪除"]);
                }

                var UserRoleInfo = await _SAFETYContext.UserRole.FirstOrDefaultAsync(p => p.UserRoleId == model.UserRoleId);
                _SAFETYContext.UserRole.Remove(UserRoleInfo);
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

        public async Task<IActionResult> GetRoleData(int userRoleid)
        {
            var res = await _SAFETYContext.UserRole.Where(x => x.UserRoleId == userRoleid).FirstOrDefaultAsync();
            return WriteJsonOk("", res);
        }

        public IActionResult GetRoleFunctionData(int? userRoleid)
        {
            var resSelect = _SAFETYContext.UserRoleFunction.Where(x => x.UserRoleId == userRoleid).Select(x=>x.FunctionId);

            var resfunt = (from t1 in _SAFETYContext.SysFunction
                           select new SysFunctionVM()
                           {
                               UserRoleId=0,
                               FunctionId=t1.FunctionId,
                               FunctionName= t1.FunctionName,
                               FnGroup=t1.FnGroup,
                               FnGroupName=t1.FnGroupName,
                               FnClass=t1.FnClass,
                               FnArea =t1.FnArea,
                               FnController=t1.FnController,
                               FnPageName=t1.FnPageName,
                               IsStop=t1.IsStop,
                               CreateId=t1.CreateId,
                               CreateDate=t1.CreateDate,
                               ModifyId=t1.ModifyId,
                               ModifyDate=t1.ModifyDate
                           }).ToList();

            var resfun = (from t1 in resfunt
                          group t1 by new { t1.FnGroup, t1.FnGroupName, t1.FnClass } into g
                          select new FullFunction()
                          {
                              FnGroup = g.Key.FnGroup,
                              FnGroupName = _localizer[g.Key.FnGroupName],
                              FnClass = g.Key.FnClass,
                              SysFunctionVM = g.ToList()
                          });

            foreach (var item in resfun)
            {
                item.FnGroupName = _localizer[item.FnGroupName];
                foreach (var sm in item.SysFunctionVM)
                {
                    sm.FunctionName= _localizer[sm.FunctionName];
                }
            }

            var result = new { resfun, resSelect };
            return WriteJsonOk("", result);
        }

        public IActionResult GetMenu(int? userRoleid)
        {
            var resSelect =( from t1 in _SAFETYContext.UserRoleFunction.Where(x => x.UserRoleId == userRoleid)
                             select new { 
                             t1.FunctionId
                             }
                );

            var resfunt = (from t1 in _SAFETYContext.SysFunction
                           join t2 in resSelect on t1.FunctionId equals t2.FunctionId into ps2
                           from t2 in ps2.DefaultIfEmpty()
                           select new SysFunctionVM()
                           {
                               UserRoleId = 0,
                               FunctionId = t2.FunctionId,
                               FunctionName = t1.FunctionName,
                               FnGroup = t1.FnGroup,
                               FnGroupName = t1.FnGroupName,
                               FnClass = t1.FnClass,
                               FnArea = t1.FnArea,
                               FnController = t1.FnController,
                               FnPageName = t1.FnPageName,
                               IsStop = t1.IsStop,
                               CreateId = t1.CreateId,
                               CreateDate = t1.CreateDate,
                               ModifyId = t1.ModifyId,
                               ModifyDate = t1.ModifyDate
                           }).Where(x=>x.FunctionId > 0).ToList();

            var resfun = (from t1 in resfunt
                          group t1 by new { t1.FnGroup, t1.FnGroupName, t1.FnClass } into g
                          select new FullFunction()
                          {
                              FnGroup = g.Key.FnGroup,
                              FnGroupName = _localizer[g.Key.FnGroupName],
                              FnClass = g.Key.FnClass,
                              SysFunctionVM = g.ToList()
                          });

            foreach (var item in resfun)
            {
                item.FnGroupName = _localizer[item.FnGroupName];
                foreach (var sm in item.SysFunctionVM)
                {
                    sm.FunctionName = _localizer[sm.FunctionName];
                }
            }

            var result = new { resfun, resSelect };
            return WriteJsonOk("", result);
        }

        public async Task<IActionResult> SaveUserRoleFunction([FromBody] FullRole fullmodel)
        {
            if (fullmodel.UserRoleId == 0 || fullmodel.lstFunSelect.Count ==0)
            {
                return WriteJsonErr(_localizer["儲存失敗"]);
            }
            UserRoleFunction model = new UserRoleFunction();
            List<UserRoleFunction> lst = new List<UserRoleFunction>();
            foreach (int i in fullmodel.lstFunSelect)
            {
                model = new UserRoleFunction();
                model.UserRoleId = fullmodel.UserRoleId;
                model.FunctionId = i;
                model.CreateId = 0;
                lst.Add(model);
            }

            _SAFETYContext.Database.BeginTransaction();
            //(先刪除，後新增)
            var UserRoleFunction = _SAFETYContext.UserRoleFunction.Where(x => x.UserRoleId == fullmodel.UserRoleId);
            _SAFETYContext.UserRoleFunction.RemoveRange(UserRoleFunction);

            _SAFETYContext.UserRoleFunction.AddRange(lst);

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["儲存成功"], fullmodel.UserRoleId)
                : WriteJsonErr(_localizer["儲存失敗"]);

        }


        #endregion



    }
}
