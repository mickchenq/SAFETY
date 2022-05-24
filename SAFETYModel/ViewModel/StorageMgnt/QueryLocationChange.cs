using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public  class QueryLocationChange
    {
        public string OrderNo { get; set; }
        public int? DcId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public string ProductLotNo { get; set; }
        public DateTime? OrderDateStart { get; set; }
        public DateTime? OrderDateEnd { get; set; }
        public int? StockAdjustId { get; set; }
        public byte ChangeStatus { get; set; }

        //end class
    }
}
