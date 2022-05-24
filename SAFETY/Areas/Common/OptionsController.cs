using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using SAFETYModel.DBModels;
using SAFETY.Controllers;

/// <summary>
/// 頁面共用元件
/// </summary>
namespace SAFETY.Areas.Common
{
    [Area("Common")]
    [Route("api/[area]/[controller]/[action]")]
    public class OptionsController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _sFilePath = "";
        public OptionsController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IWebHostEnvironment webHostEnvironment)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _webHostEnvironment = webHostEnvironment;
            //上傳的固定父目錄
            _sFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Upload");
        }

        /// <summary>
        /// 顯示上傳檔案
        /// </summary>
        /// <param name="selfPath">各功能上傳子目錄</param>
        /// <param name="fileName">檔案名稱</param>
        public IActionResult GetDownloadFile(string selfPath, string fileName)
        {
            try
            {
                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(Path.Combine(_sFilePath, selfPath, fileName));
                //Send the File to Download.
                return File(bytes, "application/octet-stream", fileName);
            }
            catch
            {
                return WriteJsonErr(_localizer["檔案讀取失敗"]);
            }
        }

        /// <summary>
        /// [下拉選單]倉別類型
        /// </summary>      
        /// <returns></returns>
        public async Task<IActionResult> GetHouseTypeData()
        {
            var res = await _SAFETYContext.HouseType.OrderBy(x => x.HouseTypeCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]物流中心
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetDataCenterData()
        {
            var res = await _SAFETYContext.DataCenter.OrderBy(x => x.DcCode).ToListAsync();          
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]倉別
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetHouseDataByDc(int? DcId)
        {
            var res = await _SAFETYContext.House.Where(x => x.DcId == DcId).OrderBy(x => x.HouseCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]庫別
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetRoomDataByHouse(int? DcId, int? HouseId)
        {
            var res = await _SAFETYContext.Room.Where(x => x.DcId == DcId && x.HouseId == HouseId).OrderBy(x => x.RoomCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]儲區
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetAreaDataByRoom(int? RoomId)
        {
            var res = await _SAFETYContext.Area.Where(x => x.RoomId == RoomId).OrderBy(x => x.AreaCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]貨架
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetShelfDataByArea(int? AreaId)
        {
            var res = await _SAFETYContext.Shelf.Where(x => x.AreaId == AreaId).OrderBy(x => x.ShelfCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]層架
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetLayerDataByShelf(int? ShelfId)
        {
            var res = await _SAFETYContext.Layer.Where(x => x.ShelfId == ShelfId).OrderBy(x => x.LayerCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]儲位
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetLocationDataByLayer(int? LayerId)
        {
            var res = await _SAFETYContext.Location.Where(x => x.LayerId == LayerId).OrderBy(x => x.LocationCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]客戶
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetCustomerData()
        {
            var res = await _SAFETYContext.Customer.OrderBy(x => x.CustomerCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]温層
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetTempLayerData()
        {
            var res = await _SAFETYContext.TempLayer.OrderBy(x => x.TempCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]供應商
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetSupplierData()
        {
            var res = await _SAFETYContext.Supplier.OrderBy(x => x.SupplierCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]產品分類
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetProductCategoryData()
        {
            var res = await _SAFETYContext.ProductCategory.OrderBy(x => x.CategoryCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]產品類型
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetProductTypeData()
        {
            var res = await _SAFETYContext.ProductType.OrderBy(x => x.TypeCode).ToListAsync();
            return Ok(res);
        }
                
        /// <summary>
        /// [下拉選單]客戶商品
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetProductDataByCust(int? CustId)
        {
            var res = await _SAFETYContext.Product.Where(x=>x.CustomerId== CustId).OrderBy(x => x.ProductCode).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]商品包裝
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetProductPackageData(int? ProductId)
        {
            var res = await _SAFETYContext.ProductPackage.Where(x => x.ProductId == ProductId).OrderBy(x => x.PackageId).ToListAsync();
            return Ok(res);
        }
    
        /// <summary>
        /// [下拉選單]使用者
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetUserData()
        {
            var res = await _SAFETYContext.SysUser.ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]儲區
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetAreaData()
        {
            var res = await _SAFETYContext.Area.ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]貨架類型
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetShelfTypeData()
        {
            var res = await _SAFETYContext.ShelfType.ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]貨架
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetShelfData(int ShelfId)
        {
            var res = await _SAFETYContext.Shelf.Where(x=>x.ShelfId != ShelfId).ToListAsync();
            return Ok(res);
        }

        /// <summary>
        /// [下拉選單]層架
        /// </summary>     
        /// <returns></returns>
        public async Task<IActionResult> GetLayerData()
        {
            var res = await _SAFETYContext.Layer.ToListAsync();
            return Ok(res);
        }

        //end class
    }
}
