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
    public class InventoryApiController : BaseController
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

        public InventoryApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer, IHttpContextAccessor iHttpContextAccessor, IWebHostEnvironment webHostEnvironment, FileService fileService)
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
        public IActionResult QueryInventory([FromBody] QueryInventory model)
        {
            var res = (from t1 in _SAFETYContext.StockAdjustment                     
                       join t3 in _SAFETYContext.DataCenter on t1.DcId equals t3.DcId into ps3
                       from t3 in ps3.DefaultIfEmpty()
                       select new
                       {
                           t1.OrderId,
                           t1.OrderNo,
                           t1.DcId,
                           t1.HouseId,
                           t1.RoomId,
                           t1.CustomerId,
                           t1.ProductId,
                           t1.OrderDate,
                           t1.StockAdjustId,
                           t1.AdjustStatus,                          
                           t3.DcName,
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
            if (model.StockAdjustId != null && model.StockAdjustId.ToString() != "0")
            {
                res = res.Where(x => x.StockAdjustId == model.StockAdjustId);
            }
            if (model.AdjustStatus.ToString() != "" && model.AdjustStatus.ToString() != "0")
            {
                res = res.Where(x => x.AdjustStatus == model.AdjustStatus);
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
        public async Task<IActionResult> DeleteInventory([FromBody] StockAdjustment model)
        {
            //檢查是否可刪除
            var isUsed = _SAFETYContext.StockAdjustment.Where(x => x.OrderId == model.OrderId && x.AdjustStatus == 3).Select(x => x.OrderId).ToList();
            if (isUsed.Any() || isUsed.Count > 0)
                return WriteJsonErr(_localizer["盤點已完成，故不可刪除資料"]);

            _SAFETYContext.Database.BeginTransaction();
            //主檔
            _SAFETYContext.StockAdjustment.Remove(model);
            //明細
            var detail = _SAFETYContext.StockAdjustmentDetail.Where(x => x.OrderId == model.OrderId);
            _SAFETYContext.StockAdjustmentDetail.RemoveRange(detail);
            
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
        public IActionResult QueryAdjustmentDetail([FromBody] QueryAdjDetail model)
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
                       select new
                       {
                           info=t1,                          
                           t2.DcName,
                           t3.LocationCode,
                           t7.RoomId,
                           t7.HouseId,
                           t7.RoomName,
                           t8.ProductName,
                           t9.PackageName,
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

            var gpdata = res.ToList().GroupBy(x => new { x.info.ProductStatus, x.info.DcId, x.DcName, x.info.CustomerId, x.info.ProductId, x.ProductName, x.info.Unit, x.PackageName, x.info.ProductLotNo, x.info.ExpirationDate, x.RoomId, x.RoomName, x.info.LocationId, x.LocationCode })
               .Select(b => new
               {
                   LocationId = b.Key.LocationId,
                   LocationCode = b.Key.LocationCode,
                   ProductStatus = b.Key.ProductStatus,
                   DcId = b.Key.DcId,
                   DcName = b.Key.DcName,
                   CustomerId = b.Key.CustomerId,
                   ProductId = b.Key.ProductId,
                   ProductName = b.Key.ProductName,
                   Unit = b.Key.Unit,
                   PackageName = b.Key.PackageName,
                   ProductLotNo = b.Key.ProductLotNo,
                   ExpirationDate = b.Key.ExpirationDate,
                   RoomId = b.Key.RoomId,
                   RoomName = b.Key.RoomName,
                   LocationQuantity = b.Select(bn => bn.info.LocationQuantity * (bn.info.InventoryKind == "O" ? -1 : 1)).Sum(),
                   AdjustQuantity = "",
                   DiffQuantity = ""
               }).Where(x => x.LocationQuantity > 0).Distinct().ToList();

            return WriteJsonOk("", gpdata);
        }

        /// <summary>
        /// 儲存盤點單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IActionResult> SaveInventory([FromBody] FullInventory model)
        {      
            if (model.StockAdjustment.OrderId == 0 || model.StockAdjustment.OrderId.ToString() == "")
                return InsertInventory(model);     //新增
            else
                return UpdateInventory(model);     //修改
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> InsertInventory([FromBody] FullInventory model)
        {
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.StockAdjustment.CreateId = _user.SysUser.UserId;
            model.StockAdjustment.CreateDate = DateTime.Now;
            model.StockAdjustment.ModifyId = _user.SysUser.UserId;
            model.StockAdjustment.ModifyDate = DateTime.Now;

            //任一筆明細有填 調整數量 則狀態改為 2.盤點中
            if (model.StockAdjustment.AdjustStatus == 1)
            {
               var tmp= model.StockAdjustmentDetail.Select(x => x.AdjustQuantity != null).ToList();
                if (tmp != null && tmp.Count > 0)
                    model.StockAdjustment.AdjustStatus = 2;
            }
           else if (model.StockAdjustment.AdjustStatus == 3)
            {
                model.StockAdjustment.OrderDate = DateTime.Today;           //盤點完成則記錄盤點日期
            }

            //判斷是否已有尚未盤點完成的資料
            var adj = (from t1 in _SAFETYContext.StockAdjustment.Where(x => x.AdjustStatus != 3)
                join t2 in _SAFETYContext.StockAdjustmentDetail on t1.OrderId equals t2.OrderId into ps2
                from t2 in ps2.DefaultIfEmpty()             
                select  new
                {
                    t2.ProductStatus,
                    t2.DcId,
                    t2.ProductId,
                    t2.ProductLotNo,
                    t2.Unit,
                    t2.ExpirationDate,
                    t2.RoomId,
                    t2.LocationId                 
                }).ToList();

            var bInv = false;
            model.StockAdjustmentDetail.ForEach(item =>
            {
                var lst = adj.Where(x => x.ProductStatus == item.ProductStatus && x.DcId == item.DcId && x.ProductId == item.ProductId && x.ProductLotNo == item.ProductLotNo && x.Unit == item.Unit && x.ExpirationDate.Value == item.ExpirationDate.Value && x.LocationId == item.LocationId).ToList();
                if (lst.Any() || lst.Count > 0)            
                    bInv = true;
            });

            if (bInv)
            {
                bInv = true;
                return WriteJsonErr(_localizer["新增失敗：有盤點明細資料已在盤點中"]);
            }

            //系統產生單號 NOyyyyMMddnnnn,DA:固定, yyyy:西元年, MM月份(補零), dd(日), nnnn(流水號)
            var NoKey = "SA" + DateTime.Today.ToString("yyyyMMdd");
            //抓db裡同一天最大序號者
            var maxResult = _SAFETYContext.StockAdjustment.Where(x => x.OrderNo.Substring(0, 10) == NoKey && x.OrderNo.Length > 10)
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
                model.StockAdjustment.OrderNo = NoKey + (iValue + 1).ToString().PadLeft(4, '0');
            }
            else
                model.StockAdjustment.OrderNo = NoKey + "0001";

            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockAdjustment.Add(model.StockAdjustment);
            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.StockAdjustmentDetail.Select(x => { x.OrderId = model.StockAdjustment.OrderId; return x; }).ToList();
            //明細檔
            _SAFETYContext.StockAdjustmentDetail.AddRange(model.StockAdjustmentDetail);
            var resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["新增失敗"]);
            }

            //盤點完成則要狀有差異數量寫到出入庫資料檔
            if (model.StockAdjustment.AdjustStatus == 3)
            {
                List<Inventory> invDetail = new List<Inventory>();
                model.StockAdjustmentDetail.ForEach(item =>
                {
                    if (item.AdjustQuantity.Value - item.LocationQuantity != 0)
                    { 
                        var invinfo = new Inventory
                        {
                            InventoryKind = "A",
                            InventoryStatus = 0,
                            DcId = item.DcId,
                            InvDate = model.StockAdjustment.OrderDate.Value,
                            LocationDetailId = item.OrderDetailId,
                            CustomerId = item.CustomerId,
                            ProductId = item.ProductId,
                            LocationId = item.LocationId,
                            LocationQuantity =item.AdjustQuantity.Value-item.LocationQuantity,
                            Unit = item.Unit,
                            ProductLotNo = item.ProductLotNo,
                            ExpirationDate = item.ExpirationDate,
                            ProductStatus = item.ProductStatus,
                            CreateId = _user.SysUser.UserId,
                            CreateDate = DateTime.Now,
                            ModifyId = _user.SysUser.UserId,
                            ModifyDate = DateTime.Now,
                        };
                        invDetail.Add(invinfo);
                    }
                });
                _SAFETYContext.Inventory.AddRange(invDetail);
            }

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["新增成功"], model.StockAdjustment.OrderId)
                : WriteJsonErr(_localizer["新增失敗"]);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateInventory([FromBody] FullInventory model)
        {
            var _sysUser = _IHttpContextAccessor.HttpContext.Session.GetString("_sysUser");
            UserData _user = JsonConvert.DeserializeObject<UserData>(_sysUser);
            model.StockAdjustment.ModifyId = _user.SysUser.UserId;
            model.StockAdjustment.ModifyDate = DateTime.Now;

            //任一筆明細有填 調整數量 則狀態改為 2.盤點中
            if (model.StockAdjustment.AdjustStatus == 1)
            {
                var tmp = model.StockAdjustmentDetail.Select(x => x.AdjustQuantity != null).ToList();
                if (tmp != null && tmp.Count > 0)
                    model.StockAdjustment.AdjustStatus = 2;
            }
            else if (model.StockAdjustment.AdjustStatus == 3)
            {
                model.StockAdjustment.OrderDate = DateTime.Today;           //盤點完成則記錄盤點日期
            }

            //判斷是否已有尚未盤點完成的資料
            var adj = (from t1 in _SAFETYContext.StockAdjustment.Where(x => x.AdjustStatus != 3 && x.OrderId !=model.StockAdjustment.OrderId)
                       join t2 in _SAFETYContext.StockAdjustmentDetail.Where(x=>x.OrderId != model.StockAdjustment.OrderId) on t1.OrderId equals t2.OrderId into ps2
                       from t2 in ps2.DefaultIfEmpty()
                       select new
                       {
                           t2.ProductStatus,
                           t2.DcId,
                           t2.ProductId,
                           t2.ProductLotNo,
                           t2.Unit,
                           t2.ExpirationDate,
                           t2.RoomId,
                           t2.LocationId
                       }).ToList();

            var bInv = false;
            model.StockAdjustmentDetail.ForEach(item =>
            {
                var lst = adj.Where(x => x.ProductStatus == item.ProductStatus && x.DcId == item.DcId && x.ProductId == item.ProductId && x.ProductLotNo == item.ProductLotNo && x.Unit == item.Unit && x.ExpirationDate.Value == item.ExpirationDate.Value && x.LocationId == item.LocationId).ToList();
                if (lst.Any() || lst.Count > 0)
                    bInv = true;
            });

            if (bInv)
            {
                bInv = true;
                return WriteJsonErr(_localizer["修改失敗：有盤點明細資料已在盤點中"]);
            }

            var stockAdjustment = _SAFETYContext.StockAdjustment.Where(x => x.OrderId == model.StockAdjustment.OrderId).FirstOrDefault();
            if (stockAdjustment == null)
            {
                return WriteJsonErr(_localizer["修改失敗：單號不存在"]);
            }

            //把之前抓到的同一key值實體的EntityState設為獨立的
            _SAFETYContext.Entry(stockAdjustment).State = EntityState.Detached;
            _SAFETYContext.Database.BeginTransaction();
            //  主檔
            _SAFETYContext.StockAdjustment.Update(model.StockAdjustment);
            //(先刪除，後新增)           
            var stockAdjustmentDetail = _SAFETYContext.StockAdjustmentDetail.Where(x => x.OrderId == model.StockAdjustment.OrderId);
            _SAFETYContext.StockAdjustmentDetail.RemoveRange(stockAdjustmentDetail);

            /*stockAdjustmentDetail.ToList().ForEach(item =>
            {
                var tmp = _SAFETYContext.Inventory.Where(x => x.InventoryKind=="A" && x.LocationDetailId == item.OrderDetailId).ToList();
                _SAFETYContext.Inventory.RemoveRange(tmp);                
            });*/

            var resSaveFirst = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveFirst.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["修改失敗"]);
            }
            // 更新主檔id
            //明細檔
            model.StockAdjustmentDetail.Select(x => { x.OrderId = model.StockAdjustment.OrderId; x.OrderDetailId = 0; return x; }).ToList();
            //明細檔
            _SAFETYContext.StockAdjustmentDetail.AddRange(model.StockAdjustmentDetail);
            var resSaveDetail = await _SAFETYContext.NewSaveChangesAsync();
            if (!resSaveDetail.Key)
            {
                _SAFETYContext.Database.RollbackTransaction();
                return WriteJsonErr(_localizer["修改失敗"]);
            }

            //盤點完成則要狀有差異數量寫到出入庫資料檔
            if (model.StockAdjustment.AdjustStatus == 3)
            {
                List<Inventory> invDetail = new List<Inventory>();
                model.StockAdjustmentDetail.ForEach(item =>
                {
                    if (item.AdjustQuantity.Value - item.LocationQuantity != 0)
                    { 
                        var invinfo = new Inventory
                        {
                            InventoryKind = "A",
                            InventoryStatus = 0,
                            DcId = item.DcId,
                            InvDate = model.StockAdjustment.OrderDate.Value,
                            LocationDetailId = item.OrderDetailId,
                            CustomerId = item.CustomerId,
                            ProductId = item.ProductId,
                            LocationId = item.LocationId,
                            LocationQuantity = item.AdjustQuantity.Value - item.LocationQuantity,
                            Unit = item.Unit,
                            ProductLotNo = item.ProductLotNo,
                            ExpirationDate = item.ExpirationDate,
                            ProductStatus = item.ProductStatus,
                            CreateId = _user.SysUser.UserId,
                            CreateDate = DateTime.Now,
                            ModifyId = _user.SysUser.UserId,
                            ModifyDate = DateTime.Now,
                        };
                        invDetail.Add(invinfo);
                    }
                });
                _SAFETYContext.Inventory.AddRange(invDetail);
            }

            var res = await _SAFETYContext.NewSaveChangesAsync();
            if (res.Key)
                _SAFETYContext.Database.CommitTransaction();
            else
                _SAFETYContext.Database.RollbackTransaction();

            return res.Key
                ? WriteJsonOk(_localizer["修改成功"], model.StockAdjustment.OrderId)
                : WriteJsonErr(_localizer["修改失敗"]);
        }

        /// <summary>
        /// 讀取盤點單資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetInventoryData(int? orderid)
        {
            if (orderid == null)
                return WriteJsonOk("");
            //主檔
            var stockAdjustment=_SAFETYContext.StockAdjustment.Where(x => x.OrderId == orderid).FirstOrDefault();

            if (stockAdjustment == null)
                return WriteJsonOk("");

            //明細
            /*var stockAdjustmentDetail = (from t1 in _SAFETYContext.StockAdjustmentDetail.Where(x => x.OrderId == orderid)
                                join t2 in _SAFETYContext.DataCenter on t1.DcId equals t2.DcId into ps2
                                from t2 in ps2.DefaultIfEmpty()
                                join t3 in _SAFETYContext.Location on t1.LocationId equals t3.LocationId into ps3
                                from t3 in ps3.DefaultIfEmpty()                             
                                join t7 in _SAFETYContext.Room on t1.RoomId equals t7.RoomId into ps7
                                from t7 in ps7.DefaultIfEmpty()
                                join t8 in _SAFETYContext.Product on t1.ProductId equals t8.ProductId into ps8
                                from t8 in ps8.DefaultIfEmpty()
                                join t9 in _SAFETYContext.ProductPackage on t1.Unit equals t9.PackageId into ps9
                                from t9 in ps9.DefaultIfEmpty()
                                select new
                                {                                    
                                    t1.OrderDetailId,
                                    t1.OrderId,
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
                                    t1.AdjustQuantity,
                                    DiffQuantity=t1.AdjustQuantity-t1.LocationQuantity,
                                    t2.DcName,
                                    t3.LocationCode,                                   
                                    t7.HouseId,
                                    t7.RoomName,
                                    t8.ProductName,
                                    t9.PackageName
                                }
            ).OrderBy(x => x.OrderDetailId).ToList();*/

            //是否可修改盤點完成則不可修改
            var isView = stockAdjustment.AdjustStatus != 3 ? false : true;
            var result = new { stockAdjustment, isView };
            return WriteJsonOk("", result);
        }

        /// <summary>
        /// 讀取盤點單明細資料
        /// </summary>
        /// <param name="orderid"></param>
        public IActionResult GetInventoryDetailData([FromBody] StockAdjustment data)
        {
            if (data.OrderId == 0)
                return WriteJsonOk("");
           
            //明細
            var stockAdjustmentDetail = (from t1 in _SAFETYContext.StockAdjustmentDetail.Where(x => x.OrderId == data.OrderId)
                        join t2 in _SAFETYContext.DataCenter on t1.DcId equals t2.DcId into ps2
                        from t2 in ps2.DefaultIfEmpty()
                        join t3 in _SAFETYContext.Location on t1.LocationId equals t3.LocationId into ps3
                        from t3 in ps3.DefaultIfEmpty()
                        join t7 in _SAFETYContext.Room on t1.RoomId equals t7.RoomId into ps7
                        from t7 in ps7.DefaultIfEmpty()
                        join t8 in _SAFETYContext.Product on t1.ProductId equals t8.ProductId into ps8
                        from t8 in ps8.DefaultIfEmpty()
                        join t9 in _SAFETYContext.ProductPackage on t1.Unit equals t9.PackageId into ps9
                        from t9 in ps9.DefaultIfEmpty()
                        select new
                        {
                            t1.OrderDetailId,
                            t1.OrderId,
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
                            t1.AdjustQuantity,
                            DiffQuantity = t1.AdjustQuantity - t1.LocationQuantity,
                            t2.DcName,
                            t3.LocationCode,
                            t7.HouseId,
                            t7.RoomName,
                            t8.ProductName,
                            t9.PackageName
                        }
            ).OrderBy(x => x.OrderDetailId).ToList();
                        
            return WriteJsonOk("", stockAdjustmentDetail);
        }

        //end class
    }
}
