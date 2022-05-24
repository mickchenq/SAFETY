using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETY.Controllers;

namespace SAFETY.Areas.CustMgmt.API
{
    [Area("CustMgmt")]
    [Route("api/[area]/[controller]/[action]")]
    public class ProdPackageApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public ProdPackageApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryProductPackage([FromBody] QueryProductPackage model)
        {
            var res = (from t1 in _SAFETYContext.ProductPackage
                       join t2 in _SAFETYContext.Product on t1.ProductId equals t2.ProductId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.Customer on t2.CustomerId equals t3.CustomerId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       join t4 in _SAFETYContext.ProductPackage on t1.ParentPackageId equals t4.PackageId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       select new
                       {
                           t1.PackageId,
                           t1.PackageName,
                           t1.ProductId,
                           t1.Length,
                           t1.Width,
                           t1.Height,
                           t1.Weight,
                           t1.ParentPackageId,
                           t1.ParentPackageQuantity,
                           t1.IsMinSku,
                           t1.Remarks,
                           t1.IsStop,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate,
                           t2.ProductName,
                           t3.CustomerId,
                           t3.CustomerName,
                           ParentPackageName = t4.PackageName
                       }
                );

            if (!string.IsNullOrEmpty(model.PackageName))
            {
                res = res.Where(x => x.PackageName.Trim().Contains(model.PackageName.Trim()));
            }
            if (model.CustomerId != null && model.CustomerId.ToString() != "0")
            {
                res = res.Where(x => x.CustomerId == model.CustomerId);
            }
            if (model.ProductId != null && model.ProductId.ToString() != "0")
            {
                res = res.Where(x => x.ProductId == model.ProductId);
            }
            if (!string.IsNullOrEmpty(model.IsStop))
            {
                res = res.Where(x => x.IsStop == model.IsStop);
            }
           
            res.OrderBy(x => x.ProductId).ThenBy(x=>x.PackageId);
            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveProductPackage([FromBody] ProductPackage model)
        {           
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var info = await _SAFETYContext.ProductPackage.Where(x => x.PackageName.Trim() == model.PackageName.Trim() && x.ProductId == model.ProductId && (model.PackageId == 0 || x.PackageId != model.PackageId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["包裝名稱已存在"]);
            }

            int status = 0;
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            if (model.ProductId == 0)
            {
                model.CreateId = _user.SysUser.UserId;
                model.CreateDate = DateTime.Now;
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.ProductPackage.Add(model);
                status = 0;   //新增
            }
            else
            {
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.ProductPackage.Update(model);
                status = 1;   //修改
            }
            var res = await _SAFETYContext.SaveChangesAsync();

            if (status == 0)
                return res > 0 ? WriteJsonOk(_localizer["新增成功"]) : WriteJsonErr(_localizer["新增失敗"]);
            else
                return res > 0 ? WriteJsonOk(_localizer["修改成功"]) : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 抓取客戶下的商品及該商品的包裝資料
        /// </summary>
        public async Task<IActionResult> GetProductAndPackageData(int CustomerId, int ProductId)
        {
            var product = await _SAFETYContext.Product.Where(x => x.CustomerId == CustomerId).ToListAsync();
            var package = await _SAFETYContext.ProductPackage.Where(x => x.ProductId == ProductId).ToListAsync();
            var result = new { product, package };
            return WriteJsonOk("", result);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteProductPackage([FromBody] ProductPackage model)
        {
            //檢查是否已被使用
            var isUsed = _SAFETYContext.ProductPackage.Where(x => x.ParentPackageId == model.PackageId).Select(x => x.PackageId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已有下層包裝，故不可刪除資料"]);

            isUsed = _SAFETYContext.NotificationDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit).ToList();
            isUsed = isUsed.Union(_SAFETYContext.ShippingDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.Inventory.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.StockInDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.StockInLocationDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.StockOutDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.StockOutLocationDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.ProcessDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.ReturnedDetail.Where(x => x.Unit == model.PackageId).Select(x => x.Unit)).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已有出入庫資料，故不可刪除資料"]);

            _SAFETYContext.ProductPackage.Remove(model);
            var res = await _SAFETYContext.SaveChangesAsync();
            return res > 0
                ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        //end class
    }
}
