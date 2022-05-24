using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class ReturnedOrder
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public int DcId { get; set; }
        public int RelatedId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerOrderNo { get; set; }
        public DateTime ReturnedDate { get; set; }
        public DateTime EstimatedReceiveDate { get; set; }
        public byte OperateType { get; set; }
        public byte ReturnType { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public byte ReturnStatus { get; set; }
        public int? AcceptId { get; set; }
        public DateTime? AcceptDate { get; set; }
        public string AcceptRemarks { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
