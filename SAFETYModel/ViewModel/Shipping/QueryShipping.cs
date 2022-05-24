using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel
{
    public class QueryShipping
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public int? DcId { get; set; }
        public string OrderNo { get; set; }
        public byte ShippingStatus { get; set; }
        public DateTime? EstimatedShippingStart { get; set; }
        public DateTime? EstimatedShippingEnd { get; set; }

        //end class
    }
}
