using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class StockInLocationDetail
    {
        public int LocationDetailId { get; set; }
        public int OrderDetailId { get; set; }
        public int LocationId { get; set; }
        public int Unit { get; set; }
        public int LocationQuantity { get; set; }
        public string Remarks { get; set; }
        public byte DetailStatus { get; set; }
    }
}
