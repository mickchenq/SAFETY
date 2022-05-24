using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel
{
    public class QueryReturn
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? DcId { get; set; }
        public string OrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public byte ReturnStatus { get; set; }
        public DateTime? ReceiveDateStart { get; set; }
        public DateTime? ReceiveDateEnd { get; set; }
    }
}
