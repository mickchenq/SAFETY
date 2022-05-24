using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryStockOut
    {
        public int? CustomerId { get; set; }
        public int? DcId { get; set; }
        public string OrderNo { get; set; }
        public int? RelatedId { get; set; }
        public DateTime? OrderDateStart { get; set; }
        public DateTime? OrderDateEnd { get; set; }
        public int? StockUserId { get; set; }
        public byte StockStatus { get; set; }        
        public string ShippingOrderNo { get; set; }             //出庫通知單號
        //end class
    }
}
