using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class ProcessOrder
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public int CustomerId { get; set; }
        public DateTime EstimatedProcessingDate { get; set; }
        public byte OperationType { get; set; }
        public byte ProcessingStatus { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
