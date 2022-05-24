using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel.ViewModel.Stock
{
    public class QueryStockIn
    {
        public string OrderNo { get; set; }
        public string RelatedOrderNo { get; set; }
        public string CustomerOrderNo { get; set; }
        public byte StockStatus { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDateStart { get; set; }
        public DateTime? OrderDateEnd { get; set; }
    }
}
