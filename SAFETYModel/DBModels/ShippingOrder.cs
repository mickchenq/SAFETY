using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class ShippingOrder
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public int DcId { get; set; }
        public int CustomerId { get; set; }
        public DateTime EstimatedShippingDate { get; set; }
        public DateTime EstimatedReceiveDate { get; set; }
        public byte OperationType { get; set; }
        public byte ArriveType { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactAddress { get; set; }
        public string ZipCode { get; set; }
        public byte ShippingStatus { get; set; }
        public string Remarks { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
