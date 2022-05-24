using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class ReturnedDetail
    {
        public int ReturnedDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductLotNo { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }
        public byte ProductStatus { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime MakeDate { get; set; }
    }
}
