using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class Inventory
    {
        public int InventoryId { get; set; }
        public string InventoryKind { get; set; }
        public byte InventoryStatus { get; set; }
        public int DcId { get; set; }
        public DateTime InvDate { get; set; }
        public int LocationDetailId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int LocationId { get; set; }
        public int LocationQuantity { get; set; }
        public int Unit { get; set; }
        public string ProductLotNo { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public byte ProductStatus { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
