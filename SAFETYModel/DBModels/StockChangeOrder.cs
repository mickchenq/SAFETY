using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class StockChangeOrder
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
        public string ChangeReason { get; set; }
        public byte ChangeStatus { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
