using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel.DBModels
{
    public class LocationChangeOrder
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public byte ProductStatus { get; set; }
        public int DcId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductLotNo { get; set; }
        public int Unit { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int RoomId { get; set; }
        public int LocationId { get; set; }
        public int LocationQuantity { get; set; }
        public int? ChangeQuantity { get; set; }
        public DateTime? OrderDate { get; set; }
        public int StockAdjustId { get; set; }
        public int NewDcId { get; set; }
        public int NewHouseId { get; set; }
        public int NewRoomId { get; set; }
        public int NewAreaId { get; set; }
        public int NewShelfId { get; set; }
        public int NewLayerId { get; set; }
        public int NewLocationId { get; set; }
        public string ChangeReason { get; set; }
        public byte ChangeStatus { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }

    }
}
