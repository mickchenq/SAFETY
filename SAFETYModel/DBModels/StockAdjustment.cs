using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class StockAdjustment
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public int DcId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int StockAdjustId { get; set; }
        public byte AdjustStatus { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
