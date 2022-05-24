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
    public class ProductApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        public ProductApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor)
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
        public IActionResult QueryProduct([FromBody] QueryProduct model)
        {          
            var res = (from t1 in _SAFETYContext.Product 
                       join t2 in _SAFETYContext.Customer on t1.CustomerId equals t2.CustomerId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.TempLayer on t1.TempLayerId equals t3.TempId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       join t4 in _SAFETYContext.ProductCategory on t1.CategoryId equals t4.CategoryId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       join t5 in _SAFETYContext.ProductType on t1.TypeId equals t5.TypeId into ps5
                       from t5 in ps5.DefaultIfEmpty()
                       join t6 in _SAFETYContext.Supplier on t1.SupplierId equals t6.SupplierId into ps6
                       from t6 in ps6.DefaultIfEmpty()
                       select new
                       {
                           t1.ProductId,
                           t1.ProductCode,
                           t1.CustomerId,
                           t1.ProductName,
                           t1.Barcode,
                           t1.TempLayerId,
                           t1.CategoryId,
                           t1.TypeId,
                           t1.SupplierId,
                           t1.Remarks,
                           t1.IsStop,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate,
                           t2.CustomerName,
                           t3.TempName,
                           t4.CategoryName,
                           t5.TypeName,
                           t6.SupplierName
                       }
                );

            if (!string.IsNullOrEmpty(model.ProductCode))
            {
                res = res.Where(x => x.ProductCode.Trim() == model.ProductCode.Trim());
            }
            if (!string.IsNullOrEmpty(model.ProductName))
            {
                res = res.Where(x => x.ProductName.Trim().Contains(model.ProductName.Trim()));
            }
            if (model.CustomerId != null && model.CustomerId.ToString() != "0")
            {
                res = res.Where(x => x.CustomerId == model.CustomerId);
            }
            if (model.TempLayerId != null && model.TempLayerId.ToString() != "0")
            {
                res = res.Where(x => x.TempLayerId == model.TempLayerId);
            }
            if (model.CategoryId != null && model.CategoryId.ToString() != "0")
            {
                res = res.Where(x => x.CategoryId == model.CategoryId);
            }
            if (model.TypeId != null && model.TypeId.ToString() != "0")
            {
                res = res.Where(x => x.TypeId == model.TypeId);
            }
            if (model.SupplierId != null && model.SupplierId.ToString() != "0")
            {
                res = res.Where(x => x.SupplierId == model.SupplierId);
            }
            if (!string.IsNullOrEmpty(model.IsStop))
            {
                res = res.Where(x => x.IsStop == model.IsStop);
            }

            res.OrderBy(x => x.ProductCode);
            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 新增/編輯
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> SaveProduct([FromBody] Product model)
        {         
            if (!ModelState.IsValid)
            {
                return ModelValidate();
            }

            var info = await _SAFETYContext.Product.Where(x => x.ProductCode.Trim() == model.ProductCode.Trim() && (model.ProductId == 0 || x.ProductId != model.ProductId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["商品代碼已存在"]);
            }
            info = await _SAFETYContext.Product.Where(x => x.ProductName.Trim() == model.ProductName.Trim() && (model.ProductId == 0 || x.ProductId != model.ProductId)).ToListAsync();
            if (info.Any() || info.Count > 0)
            {
                return WriteJsonErr(_localizer["商品名稱已存在"]);
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
                _SAFETYContext.Product.Add(model);
                status = 0;   //新增
            }
            else
            {
                model.ModifyId = _user.SysUser.UserId;
                model.ModifyDate = DateTime.Now;
                _SAFETYContext.Product.Update(model);
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
        public async Task<IActionResult> DeleteProduct([FromBody] Product model)
        {
            //檢查是否已被使用
            var isUsed = _SAFETYContext.ProductPackage.Where(x => x.ProductId == model.ProductId).Select(x => x.ProductId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已設定商品包裝資料，故不可刪除資料"]);

            isUsed = _SAFETYContext.ShippingDetail.Where(x => x.ProductId == model.ProductId).Select(x => x.ProductId).ToList();
            isUsed = isUsed.Union(_SAFETYContext.StockInDetail.Where(x => x.ProductId == model.ProductId).Select(x => x.ProductId)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.StockOutDetail.Where(x => x.ProductId == model.ProductId).Select(x => x.ProductId)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.Inventory.Where(x => x.ProductId == model.ProductId).Select(x => x.ProductId)).ToList();
            isUsed = isUsed.Union(_SAFETYContext.ProcessDetail.Where(x => x.ProductId == model.ProductId).Select(x => x.ProductId)).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["已有出入庫資料，故不可刪除資料"]);

            _SAFETYContext.Product.Remove(model);
            var res = await _SAFETYContext.SaveChangesAsync();
            return res > 0
               ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        //end class
    }
}
