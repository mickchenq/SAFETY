using SAFETYModel;
using SAFETYModel.DBModels;
using SAFETYService;
using SAFETY.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SAFETY.Resources;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAFETY.Areas.Stock.API
{
    [Area("Stock")]
    [Route("api/[area]/[controller]/[action]")]
    public class StockChangeApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private IHttpContextAccessor _IHttpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _sFilePath = "";
        /// <summary>
        /// 檔案服務
        /// </summary>
        private readonly FileService _fileService;

        public StockChangeApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor, IWebHostEnvironment webHostEnvironment, FileService fileService)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
            _IHttpContextAccessor = iHttpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            //上傳的固定父目錄
            _sFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot", "Upload");
        }

        /// <summary>
        /// 查詢列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QueryStockChange([FromBody] QueryStockChange model)
        {
            var res = (from t1 in _SAFETYContext.StockChangeOrder
                       join t3 in _SAFETYContext.DataCenter on t1.DcId equals t3.DcId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       join t4 in _SAFETYContext.Customer on t1.CustomerId equals t4.CustomerId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       join t5 in _SAFETYContext.Product on t1.ProductId equals t5.ProductId into ps5
                       from t5 in ps5.DefaultIfEmpty()
                       join t6 in _SAFETYContext.ProductPackage on t1.Unit equals t6.PackageId into ps6
                       from t6 in ps6.DefaultIfEmpty()
                       join t7 in _SAFETYContext.Room on t1.RoomId equals t7.RoomId into ps7
                       from t7 in ps7.DefaultIfEmpty()
                       join t8 in _SAFETYContext.Location on t1.LocationId equals t8.LocationId into ps8
                       from t8 in ps8.DefaultIfEmpty()
                       select new
                       {
                           t1.OrderId,
                           t1.OrderNo,
                           t1.DcId,
                           t1.CustomerId,
                           t1.ProductStatus,
                           t1.ProductId,
                           t1.ProductLotNo,
                           t1.Unit,
                           t1.ExpirationDate,
                           t1.RoomId,       
                           t1.LocationId,
                           t1.LocationQuantity,
                           t1.ChangeQuantity,
                           DiffQuantity=t1.ChangeQuantity-t1.LocationQuantity,
                           t1.OrderDate,
                           t1.StockAdjustId,
                           t1.ChangeStatus,
                           t3.DcName,
                           t4.CustomerName,
                           t5.ProductName,
                           t6.PackageName,
                           t7.RoomName,
                           t8.LocationCode,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate
                       }
                );

            if (!string.IsNullOrEmpty(model.OrderNo))
            {
                res = res.Where(x => x.OrderNo == model.OrderNo);
            }
            if (model.DcId != null && model.DcId.ToString() != "0")
            {
                res = res.Where(x => x.DcId == model.DcId);
            }
            if (model.CustomerId != null && model.CustomerId.ToString() != "0")
            {
                res = res.Where(x => x.CustomerId == model.CustomerId);
            }
            if (model.ProductId != null && model.ProductId.ToString() != "0")
            {
                res = res.Where(x => x.ProductId == model.ProductId);
            }
            if (!string.IsNullOrEmpty(model.ProductLotNo))
            {
                res = res.Where(x => x.ProductLotNo == model.ProductLotNo);
            }
            if (model.StockAdjustId != null && model.StockAdjustId.ToString() != "0")
            {
                res = res.Where(x => x.StockAdjustId == model.StockAdjustId);
            }
            if (model.ChangeStatus.ToString() != "" && model.ChangeStatus.ToString() != "0")
            {
                res = res.Where(x => x.ChangeStatus == model.ChangeStatus);
            }
            if (model.OrderDateStart.HasValue)
            {
                res = res.Where(x => x.OrderDate >= model.OrderDateStart.Value);
            }
            if (model.OrderDateEnd.HasValue)
            {
                res = res.Where(x => x.OrderDate <= model.OrderDateEnd.Value);
            }

            return WriteJsonOk("", res);
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteStockChange([FromBody] StockChangeOrder model)
        {
            //檢查是否可刪除
            var isUsed = _SAFETYContext.StockChangeOrder.Where(x => x.OrderId == model.OrderId && x.ChangeStatus == 3).Select(x => x.OrderId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["庫存調整已完成，故不可刪除資料"]);

            _SAFETYContext.Database.BeginTransaction();
            //主檔
            _SAFETYContext.StockChangeOrder.Remove(model);
          
            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
               ? WriteJsonOk(_localizer["刪除成功"])
               : WriteJsonErr(_localizer["刪除失敗"]);
        }

        /// <summary>
        /// 查詢符合條件的庫存資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult QuerySearchStock([FromBody] QueryStock model)
        {
            var res = (from t1 in _SAFETYContext.Inventory.Where(x => x.DcId == model.DcId && (x.InventoryStatus == 2 || x.InventoryKind != "I"))
                       join t2 in _SAFETYContext.DataCenter on t1.DcId equals t2.DcId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       join t3 in _SAFETYContext.Location on t1.LocationId equals t3.LocationId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       join t4 in _SAFETYContext.Layer on t3.LayerId equals t4.LayerId into ps4
                       from t4 in ps4.DefaultIfEmpty()
                       join t5 in _SAFETYContext.Shelf on t4.ShelfId equals t5.ShelfId into ps5
                       from t5 in ps5.DefaultIfEmpty()
                       join t6 in _SAFETYContext.Area on t5.AreaId equals t6.AreaId into ps6
                       from t6 in ps6.DefaultIfEmpty()
                       join t7 in _SAFETYContext.Room on t6.RoomId equals t7.RoomId into ps7
                       from t7 in ps7.DefaultIfEmpty()
                       join t8 in _SAFETYContext.Product on t1.ProductId equals t8.ProductId into ps8
                       from t8 in ps8.DefaultIfEmpty()
                       join t9 in _SAFETYContext.ProductPackage on t1.Unit equals t9.PackageId into ps9
                       from t9 in ps9.DefaultIfEmpty()
                       join t10 in _SAFETYContext.Customer on t1.CustomerId equals t10.CustomerId into ps10
                       from t10 in ps10.DefaultIfEmpty()
                       select new
                       {
                           info = t1,
                           t2.DcName,
                           t3.LocationCode,
                           t7.RoomId,
                           t7.HouseId,
                           t7.RoomName,
                           t8.ProductName,                           
                           t9.PackageName,
                           t10.CustomerName,
                           t1.CreateId,
                           t1.CreateDate,
                           t1.ModifyId,
                           t1.ModifyDate
                       }
                 );
           
            if (model.CustomerId.ToString() != "0")
            {
                res = res.Where(x => x.info.CustomerId == model.CustomerId);
            }
            if (model.ProductId.ToString() != "0")
            {
                res = res.Where(x => x.info.ProductId == model.ProductId);
            }
            if (model.HouseId.ToString() != "0")
            {
                res = res.Where(x => x.HouseId == model.HouseId);
            }
            if (model.RoomId.ToString() != "0")
            {
                res = res.Where(x => x.RoomId == model.RoomId);
            }
            if (!string.IsNullOrEmpty(model.LocationCode))
            {
                res = res.Where(x => x.LocationCode == model.LocationCode);
            }
            if (!string.IsNullOrEmpty(model.ProductLotNo))
            {
                res = res.Where(x => x.info.ProductLotNo == model.ProductLotNo);
            }
            if (model.ProductStatus.ToString() != "0")
            {
                res = res.Where(x => x.info.ProductStatus == model.ProductStatus);
            }
            if (model.ExpirationDateStart.HasValue)
            {
                res = res.Where(x => x.info.ExpirationDate >= model.ExpirationDateStart.Value);
            }
            if (model.ExpirationDateEnd.HasValue)
            {
                res = res.Where(x => x.info.ExpirationDate <= model.ExpirationDateEnd.Value);
            }

            var gpdata = res.ToList().GroupBy(x => new { x.info.ProductStatus, x.info.DcId, x.DcName, x.info.CustomerId, x.CustomerName, x.info.ProductId, x.ProductName, x.info.Unit, x.PackageName, x.info.ProductLotNo, x.info.ExpirationDate, x.RoomId, x.RoomName, x.info.LocationId, x.LocationCode })
               .Select(b => new
               {
                   LocationId = b.Key.LocationId,
                   LocationCode = b.Key.LocationCode,
                   ProductStatus = b.Key.ProductStatus,
                   DcId = b.Key.DcId,
                   DcName = b.Key.DcName,
                   CustomerId = b.Key.CustomerId,                
                   CustomerName = b.Key.CustomerName,
                   ProductId = b.Key.ProductId,
                   ProductName = b.Key.ProductName,
                   Unit = b.Key.Unit,
                   PackageName = b.Key.PackageName,
                   ProductLotNo = b.Key.ProductLotNo,
                   ExpirationDate = b.Key.ExpirationDate,
                   RoomId = b.Key.RoomId,
                   RoomName = b.Key.RoomName,
                   LocationQuantity = b.Select(bn => bn.info.LocationQuantity * (bn.info.InventoryKind == "O" ? -1 : 1)).Sum(),
                   ChangeQuantity = "",
                   DiffQuantity = ""
               }).Where(x => x.LocationQuantity > 0).Distinct().ToList();

            return WriteJsonOk("", gpdata);
        }

        /// <summary>
        /// 儲存庫存調整單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IActionResult> SaveStockChange([FromBody] StockChangeOrder model)
        {
            if (model.OrderId == 0 || model.OrderId.ToString() == "")
                return InsertStockChange(model);     //新增
            else
                return UpdateStockChange(model);     //修改
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertStockChange([FromBody] StockChangeOrder model)
        {
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.CreateId = _user.SysUser.UserId;
            model.CreateDate = DateTime.Now;
            model.ModifyId = _user.SysUser.UserId;
            model.ModifyDate = DateTime.Now;

            //有填 調整數量 則狀態改為 2.調整中
            if (model.ChangeStatus == 1)
            {                
                if (model.ChangeQuantity != null)
                    model.ChangeStatus = 2;
            }
            else if (model.ChangeStatus == 3)
            {
                model.OrderDate = DateTime.Today;           //調整完成則記錄調整日期
            }

            //判斷是否已有尚未調整完成的資料
            var adj = _SAFETYContext.StockChangeOrder.Where(x => x.ChangeStatus != 3 && x.ProductStatus == model.ProductStatus && x.DcId == model.DcId && x.ProductId == model.ProductId && x.ProductLotNo == model.ProductLotNo && x.Unit == model.Unit && x.ExpirationDate.Value == model.ExpirationDate.Value && x.LocationId == model.LocationId).Select(x => x.OrderId).ToList();
            adj = adj.Union(_SAFETYContext.LocationChangeOrder.Where(x => x.ChangeStatus != 3 && x.ProductStatus == model.ProductStatus && x.DcId == model.DcId && x.ProductId == model.ProductId && x.ProductLotNo == model.ProductLotNo && x.Unit == model.Unit && x.ExpirationDate.Value == model.ExpirationDate.Value && x.LocationId == model.LocationId).Select(x => x.OrderId)).ToList();
            if (adj.Any() || adj.Count > 0)
            {
                return WriteJsonErr(_localizer["新增失敗：商品資料已在調整中"]);
            }

            //系統產生單號 NOyyyyMMddnnnn,DA:固定, yyyy:西元年, MM月份(補零), dd(日), nnnn(流水號)
            var NoKey = "SC" + DateTime.Today.ToString("yyyyMMdd");
            //抓db裡同一天最大序號者
            var maxResult = _SAFETYContext.StockChangeOrder.Where(x => x.OrderNo.Substring(0, 10) == NoKey && x.OrderNo.Length > 10)
                .GroupBy(x => x.OrderNo.Substring(0, 10))
                .Select((b) => new
                {
                    maxNo = b.Max(bn => bn.OrderNo)
                }).ToList();
            if (maxResult != null && maxResult.Count() > 0)
            {
                int iValue = 0;
                if (maxResult[0].maxNo.Length >= 14)
                    int.TryParse(maxResult[0].maxNo.Substring(10, 4), out iValue);
                model.OrderNo = NoKey + (iValue + 1).ToString().PadLeft(4, '0');
            }
            else
                model.OrderNo = NoKey + "0001";

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockChangeOrder.Add(model);
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }

            //調整完成則要狀有差異數量寫到出入庫資料檔
            if (model.ChangeStatus == 3)
            {
                List<Inventory> invDetail = new List<Inventory>();
                if (model.ChangeQuantity.Value - model.LocationQuantity != 0)
                {
                    var invinfo = new Inventory
                    {
                        InventoryKind = "S",
                        InventoryStatus = 0,
                        DcId = model.DcId,
                        InvDate = model.OrderDate.Value,
                        LocationDetailId = model.OrderId,
                        CustomerId = model.CustomerId,
                        ProductId = model.ProductId,
                        LocationId = model.LocationId,
                        LocationQuantity = model.ChangeQuantity.Value - model.LocationQuantity,
                        Unit = model.Unit,
                        ProductLotNo = model.ProductLotNo,
                        ExpirationDate = model.ExpirationDate,
                        ProductStatus = model.ProductStatus,
                        CreateId = _user.SysUser.UserId,
                        CreateDate = DateTime.Now,
                        ModifyId = _user.SysUser.UserId,
                        ModifyDate = DateTime.Now,
                    };
                    _SAFETYContext.Inventory.Add(invinfo);
                    resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
                    /*if (resSaveFirst.Key)
                        _SAFETYContext.Database.CommitTransaction();
                    else
                        _SAFETYContext.Database.RollbackTransaction();*/
                }
            }
                     
            if (resSaveFirst.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return resSaveFirst.Key
                ? WriteJsonOk(_localizer["新增成功"], model.OrderId)
                : WriteJsonErr(_localizer["新增失敗"]);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateStockChange([FromBody] StockChangeOrder model)
        {
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.ModifyId = _user.SysUser.UserId;
            model.ModifyDate = DateTime.Now;

            //有填 調整數量 則狀態改為 2. 調整點中
            if (model.ChangeStatus == 1)
            {
                if (model.ChangeQuantity != null)
                    model.ChangeStatus = 2;
            }
            else if (model.ChangeStatus == 3)
            {
                model.OrderDate = DateTime.Today;           // 調整完成則記錄 調整日期
            }

            //判斷是否已有尚未 調整完成的資料
            var adj = _SAFETYContext.StockChangeOrder.Where(x => x.ChangeStatus != 3 && x.OrderId != model.OrderId && x.ProductStatus == model.ProductStatus && x.DcId == model.DcId && x.ProductId == model.ProductId && x.ProductLotNo == model.ProductLotNo && x.Unit == model.Unit && x.ExpirationDate.Value == model.ExpirationDate.Value && x.LocationId == model.LocationId).Select(x => x.OrderId).ToList();
            adj = adj.Union(_SAFETYContext.LocationChangeOrder.Where(x => x.ChangeStatus != 3 && x.ProductStatus == model.ProductStatus && x.DcId == model.DcId && x.ProductId == model.ProductId && x.ProductLotNo == model.ProductLotNo && x.Unit == model.Unit && x.ExpirationDate.Value == model.ExpirationDate.Value && x.LocationId == model.LocationId).Select(x => x.OrderId)).ToList();
            if (adj.Any() || adj.Count > 0)
            {
                return WriteJsonErr(_localizer["修改失敗：商品資料已在調整中"]);
            }           

            var stockChangeOrder = _SAFETYContext.StockChangeOrder.Where(x => x.OrderId == model.OrderId).FirstOrDefault();
            if (stockChangeOrder == null)
            {
                return WriteJsonErr(_localizer["修改失敗：單號不存在"]);
            }

            //把之前抓到的同一key值實體的EntityState設為獨立的
            _SAFETYContext.Entry(stockChangeOrder).State = EntityState.Detached;
            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockChangeOrder.Update(model);
            
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["修改失敗"]);
            }

            //調整完成則要將有差異數量寫到出入庫資料檔
            if (model.ChangeStatus == 3)
            {
                List<Inventory> invDetail = new List<Inventory>();
                if (model.ChangeQuantity.Value - model.LocationQuantity != 0)
                {
                    var invinfo = new Inventory
                    {
                        InventoryKind = "S",
                        InventoryStatus = 0,
                        DcId = model.DcId,
                        InvDate = model.OrderDate.Value,
                        LocationDetailId = model.OrderId,
                        CustomerId = model.CustomerId,
                        ProductId = model.ProductId,
                        LocationId = model.LocationId,
                        LocationQuantity = model.ChangeQuantity.Value - model.LocationQuantity,
                        Unit = model.Unit,
                        ProductLotNo = model.ProductLotNo,
                        ExpirationDate = model.ExpirationDate,
                        ProductStatus = model.ProductStatus,
                        CreateId = _user.SysUser.UserId,
                        CreateDate = DateTime.Now,
                        ModifyId = _user.SysUser.UserId,
                        ModifyDate = DateTime.Now,
                    };
                    _SAFETYContext.Inventory.Add(invinfo);
                    resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();                   
                }
            }

            if (resSaveFirst.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return resSaveFirst.Key
                ? WriteJsonOk(_localizer["修改成功"], model.OrderId)
                : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 讀取庫存調整單資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetStockChangeData(int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");

            //主檔
            var stockChangeOrder = (from t1 in _SAFETYContext.StockChangeOrder.Where(x => x.OrderId == orderid)
                                join t2 in _SAFETYContext.DataCenter on t1.DcId equals t2.DcId into ps2
                                from t2 in ps2.DefaultIfEmpty()
                                join t3 in _SAFETYContext.Location on t1.LocationId equals t3.LocationId into ps3
                                from t3 in ps3.DefaultIfEmpty()
                                join t4 in _SAFETYContext.Customer on t1.CustomerId equals t4.CustomerId into ps4
                                from t4 in ps4.DefaultIfEmpty()
                                join t7 in _SAFETYContext.Room on t1.RoomId equals t7.RoomId into ps7
                                from t7 in ps7.DefaultIfEmpty()
                                join t8 in _SAFETYContext.Product on t1.ProductId equals t8.ProductId into ps8
                                from t8 in ps8.DefaultIfEmpty()
                                join t9 in _SAFETYContext.ProductPackage on t1.Unit equals t9.PackageId into ps9
                                from t9 in ps9.DefaultIfEmpty()
                                select new
                                {   
                                    t1.OrderId,
                                    t1.OrderNo,
                                    t1.ProductStatus,
                                    t1.DcId,
                                    t1.CustomerId,
                                    t1.ProductId,
                                    t1.ProductLotNo,
                                    t1.Unit,
                                    t1.ExpirationDate,
                                    t1.RoomId,
                                    t1.LocationId,
                                    t1.LocationQuantity,
                                    t1.ChangeQuantity,
                                    DiffQuantity=t1.ChangeQuantity - t1.LocationQuantity,
                                    t1.OrderDate,
                                    t1.StockAdjustId,
                                    t1.ChangeReason,
                                    t1.ChangeStatus,
                                    t1.CreateId,
                                    t1.CreateDate,
                                    t1.ModifyId,
                                    t1.ModifyDate,
                                    t2.DcName,
                                    t3.LocationCode,  
                                    t4.CustomerName,
                                    t7.HouseId,
                                    t7.RoomName,
                                    t8.ProductName,
                                    t9.PackageName
                                }
            ).FirstOrDefault();

            //是否可修改盤點完成則不可修改
            var isView = stockChangeOrder.ChangeStatus != 3 ? false : true;
            var result = new { stockChangeOrder, isView };
            return WriteJsonOk("", result);
        }

        //end class
    }
}
