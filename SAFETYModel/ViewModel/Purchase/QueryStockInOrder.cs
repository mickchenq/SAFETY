using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryStockInOrder
    {
        public int? CustomerId { get; set; }
        public int? DcId { get; set; }
        public string OrderNo { get; set; }
        public int? RelatedId { get; set; }
        public DateTime? OrderDateStart { get; set; }
        public DateTime? OrderDateEnd { get; set; }
        public int? StockUserId { get; set; }
        public byte StockStatus { get; set; }
        public string NotificationOrderNo { get; set; }             //進貨通知單號

        //end class
    }
}
