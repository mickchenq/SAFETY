using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public int CustomerId { get; set; }
        public string ProductName { get; set; }
        public string Barcode { get; set; }
        public int? TempLayerId { get; set; }
        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }
        public int? SupplierId { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
