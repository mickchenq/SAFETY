using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class ProcessDetail
    {
        public int DetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Unit { get; set; }
        public int Quantity { get; set; }
        public byte ProductStatus { get; set; }
    }
}
