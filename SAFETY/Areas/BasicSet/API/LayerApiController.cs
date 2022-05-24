using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYModel.ViewModel.BasicSet;
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
    public class LayerApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public LayerApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
        }

        public async Task<IActionResult> QueryLayer([FromBody] QueryLayer query)
        {
            var layers = _SAFETYContext.Layer.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.LayerCode))
                layers = layers.Where(l => l.LayerCode.Contains(query.LayerCode));
            if (query.ShelfId.HasValue)
                layers = layers.Where(l => l.ShelfId.Equals(query.ShelfId.Value));
            if (query.CurrentLayer.HasValue)
                layers = layers.Where(l => l.CurrentLayer.Equals(query.CurrentLayer.Value));
            if (!string.IsNullOrWhiteSpace(query.IsStop))
                layers = layers.Where(l => l.IsStop.Contains(query.IsStop));

            var result = await layers.ToListAsync();
            return WriteJsonOk("", result);
        }

        public async Task<IActionResult> UpdateLayer([FromBody] Layer model)
        {
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var value = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData user = JsonConvert.DeserializeObject<UserData>(value);

            if (model.LayerId == 0)//新增
            {
                model.CreateId = user.SysUser.UserId;
                model.CreateDate = DateTime.Now;
                _SAFETYContext.Layer.Add(model);
                int ChangeCount = await _SAFETYContext.SaveChangesAsync();
                return ChangeCount > 0 ? WriteJsonOk(_localizer["新增成功"], null) : WriteJsonErr(_localizer["新增失敗"], null);
            }
            else//修改
            {
                Layer org = _SAFETYContext.Layer.First(l => l.LayerId == model.LayerId);
                org.ShelfId = model.ShelfId;
                org.LayerCode = model.LayerCode;
                org.CurrentLayer = model.CurrentLayer;
                org.Fields = model.Fields;
                org.Height = model.Height;
                org.Depth = model.Depth;
                org.Remarks = model.Remarks;
                org.IsStop = model.IsStop;
                org.ModifyId = user.SysUser.UserId;
                org.ModifyDate = DateTime.Now;
                _SAFETYContext.Layer.Update(org);
                int ChangeCount = await _SAFETYContext.SaveChangesAsync();
                return ChangeCount > 0 ? WriteJsonOk(_localizer["修改成功"], null) : WriteJsonErr(_localizer["修改失敗"], null);
            }
        }

        public async Task<IActionResult> DeleteLayer([FromBody] Layer model)
        {
            _SAFETYContext.Layer.Remove(model);
            int ChangeCount = await _SAFETYContext.SaveChangesAsync();
            return ChangeCount > 0 ? WriteJsonOk(_localizer["刪除成功"], null) : WriteJsonErr(_localizer["刪除失敗"], null);
        }
    }
}
