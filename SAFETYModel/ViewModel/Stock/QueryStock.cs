using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAFETYModel
{
    public class QueryStock
    {
        public byte ProductStatus { get; set; }
        public int DcId { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductLotNo { get; set; }
        public string LocationCode { get; set; }
        public DateTime? ExpirationDateStart { get; set; }
        public DateTime? ExpirationDateEnd { get; set; }

        //end class
    }
}
