using SAFETYModel.DBModels;
using SAFETYModel.ViewModel;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using SAFETY.Resources;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SAFETYModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SAFETY.Areas.BasicSet.API
{
    [Area("BasicSet")]
    [Route("api/[area]/[controller]/[action]")]
    public class RoomApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public RoomApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
        }

        public IActionResult QueryRoom([FromBody]QueryRoom query)
        {
            var rooms = _SAFETYContext.Room.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.RoomName))
                rooms = rooms.Where(r => r.RoomName.Contains(query.RoomName));
            if (!string.IsNullOrWhiteSpace(query.RoomCode))
                rooms = rooms.Where(r => r.RoomCode.Contains(query.RoomCode));
            if (query.DcId.HasValue)
                rooms = rooms.Where(r => r.DcId.Equals(query.DcId.Value));
            if (query.HouseId.HasValue)
                rooms = rooms.Where(r => r.HouseId.Equals(query.HouseId.Value));
            if (query.RoomTypeId.HasValue)
                rooms = rooms.Where(r => r.RoomTypeId.Equals(query.RoomTypeId.Value));
            if (!string.IsNullOrWhiteSpace(query.IsStop))
                rooms = rooms.Where(r => r.IsStop.Equals(query.IsStop));
            List<Room> result = rooms.ToList();
            return WriteJsonOk("",result);
        }

        public IActionResult InsertRoom([FromBody] Room model)
        {
            if(!ModelState.IsValid)
            {
                return ModelValidate();
            }
            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            model.CreateId = user.SysUser.UserId;
            model.CreateDate = DateTime.Now;
            _SAFETYContext.Room.Add(model);
            int ChangeCount = _SAFETYContext.SaveChanges();
            return ChangeCount > 0 ? WriteJsonOk(_localizer["新增成功"], null) : WriteJsonErr(_localizer["新增失敗"], null);
        }

        public IActionResult UpdateRoom([FromBody] Room model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            if (model.RoomId == 0)//新增
            {
                model.CreateId = user.SysUser.UserId;
                model.CreateDate = DateTime.Now;
                _SAFETYContext.Room.Add(model);
                int ChangeCount = _SAFETYContext.SaveChanges();
                return ChangeCount > 0 ? WriteJsonOk(_localizer["新增成功"], null) : WriteJsonErr(_localizer["新增失敗"], null);
            }
            else//修改
            {
                Room org = _SAFETYContext.Room.First(r => r.RoomId == model.RoomId);
                org.RoomCode = model.RoomCode;
                org.RoomName = model.RoomName;
                org.DcId = model.DcId;
                org.HouseId = model.HouseId;
                org.RoomTypeId = model.RoomTypeId;
                org.TempLayerId = model.TempLayerId;
                org.Remarks = model.Remarks;
                org.IsStop = model.IsStop;
                org.ModifyId = user.SysUser.UserId;
                org.ModifyDate = DateTime.Now;
                _SAFETYContext.Room.Update(org);
                int ChangeCount = _SAFETYContext.SaveChanges();
                return ChangeCount > 0 ? WriteJsonOk(_localizer["修改成功"], null) : WriteJsonErr(_localizer["修改失敗"], null);
            }
        }

        public IActionResult DeleteRoom([FromBody] Room model)
        {
            _SAFETYContext.Room.Remove(model);
            int ChangeCount = _SAFETYContext.SaveChanges();
            return ChangeCount > 0 ? WriteJsonOk(_localizer["刪除成功"], null) : WriteJsonErr(_localizer["刪除失敗"], null);
        }

    }
}
