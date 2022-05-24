using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryDailyStock
    {
        public int? DcId { get; set; }
        public int? CustomerId { get; set; }        
        public int? ProductId { get; set; }
        public byte ProductStatus { get; set; }
        public string ProductLotNo { get; set; }
        public DateTime? ReportDate { get; set; }
        public byte StockType { get; set; }

        //end class
    }
}
