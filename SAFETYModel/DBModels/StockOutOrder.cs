using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class StockOutOrder
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public int RelatedId { get; set; }
        public DateTime OrderDate { get; set; }
        public int StockUserId { get; set; }
        public byte StockStatus { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
