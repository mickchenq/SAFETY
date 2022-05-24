using SAFETYModel.DBModels;
using SAFETYModel.ViewModel.StorageMgnt;
using SAFETY.Controllers;
using SAFETY.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace SAFETY.Areas.StorageMgnt.API
{
    [Area("StorageMgnt")]
    [Route("api/[area]/[controller]/[action]")]
    public class LocationProductQueryApiController : BaseController
    {
        private readonly SAFETYContext _SAFETYContext;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public LocationProductQueryApiController(SAFETYContext SAFETYContext, IStringLocalizer<SharedResources> localizer)
        {
            _SAFETYContext = SAFETYContext;
            _localizer = localizer;
        }

        public IActionResult QueryLocationProduct([FromBody] LocationProductQuery queryInfo)
        {
            var dc = _SAFETYContext.DataCenter.AsQueryable();
            var house = _SAFETYContext.House.AsQueryable();
            var room = _SAFETYContext.Room.AsQueryable();
            var area = _SAFETYContext.Area.AsQueryable();
            var customer = _SAFETYContext.Customer.AsQueryable();
            var product = _SAFETYContext.Product.AsQueryable();
            var shelf = _SAFETYContext.Shelf.AsQueryable();
            var layer = _SAFETYContext.Layer.AsQueryable();
            var location = _SAFETYContext.Location.AsQueryable();
            var inventory = _SAFETYContext.Inventory.AsQueryable();

            if (queryInfo.DcId.HasValue)
                dc = dc.Where(x => x.DcId == queryInfo.DcId);
            if (queryInfo.HouseId.HasValue)
                house = house.Where(x => x.HouseId == queryInfo.HouseId);
            if (queryInfo.RoomId.HasValue)
                room = room.Where(x => x.RoomId == queryInfo.RoomId);
            if (queryInfo.AreaId.HasValue)
                area = area.Where(x => x.AreaId == queryInfo.AreaId);
            if (queryInfo.CustomerId.HasValue)
                customer = customer.Where(x => x.CustomerId == queryInfo.CustomerId);
            if (queryInfo.ProductId.HasValue)
                product = product.Where(x => x.ProductId == queryInfo.ProductId);
            if (queryInfo.ShelfTypeId.HasValue)
                shelf = shelf.Where(x => x.ShelfTypeId == queryInfo.ShelfTypeId);
            if (!string.IsNullOrWhiteSpace(queryInfo.LocationCode))
                location = location.Where(x => x.LocationCode.Contains(queryInfo.LocationCode));
            if (!string.IsNullOrWhiteSpace(queryInfo.ProductLotNo))
                inventory = inventory.Where(x => x.ProductLotNo.Contains(queryInfo.ProductLotNo));
            if (queryInfo.InvDateS.HasValue)
                inventory = inventory.Where(x => x.InvDate >= queryInfo.InvDateS);
            if (queryInfo.InvDateE.HasValue)
                inventory = inventory.Where(x => x.InvDate <= queryInfo.InvDateE);
            if (queryInfo.ExpirationDateS.HasValue)
                inventory = inventory.Where(x => x.ExpirationDate >= queryInfo.ExpirationDateS);
            if (queryInfo.ExpirationDateE.HasValue)
                inventory = inventory.Where(x => x.ExpirationDate <= queryInfo.ExpirationDateE);

            var data = (
                from inv in inventory
                join lo in location on inv.LocationId equals lo.LocationId
                join la in layer on lo.LayerId equals la.LayerId
                join sh in shelf on la.ShelfId equals sh.ShelfId
                join ar in area on sh.AreaId equals ar.AreaId
                join ro in room on ar.RoomId equals ro.RoomId
                join ho in house on ro.HouseId equals ho.HouseId
                join d in dc on ho.DcId equals d.DcId
                join pu in product on inv.ProductId equals pu.ProductId
                join un in _SAFETYContext.ProductPackage on pu.ProductId equals un.ProductId
                select new
                {
                    ro.RoomName,
                    lo.LocationCode,
                    pu.ProductName,
                    inv.ProductStatus,
                    inv.ExpirationDate,
                    inv.InventoryKind,
                    inv.LocationQuantity,
                    inv.InvDate,
                    un.PackageName

                }).ToList();

            var group_data = data.GroupBy(x => new { x.RoomName, x.LocationCode, x.ProductName, x.ProductStatus, x.ExpirationDate })
                .Select(x => new
                {
                    x.Key.RoomName,
                    x.Key.LocationCode,
                    x.Key.ProductName,
                    x.Key.ProductStatus,
                    x.Key.ExpirationDate,
                    Quantity = x.Sum(y => (y.InventoryKind == "O" ? -1 : 1) * y.LocationQuantity)
                });

            return WriteJsonOk("", group_data);
        }
    }
}
