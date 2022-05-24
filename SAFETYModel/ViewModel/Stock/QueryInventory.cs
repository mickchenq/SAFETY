using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryInventory
    {
        public int? DcId { get; set; }
        public string OrderNo { get; set; }
        public DateTime? OrderDateStart { get; set; }
        public DateTime? OrderDateEnd { get; set; }
        public int? StockAdjustId { get; set; }
        public byte AdjustStatus { get; set; }

        //end class
    }
}
