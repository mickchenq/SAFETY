using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel.ViewModel.StorageMgnt
{
    public class LocationProductQuery
    {
        public int? DcId { get; set; }
        public int? HouseId { get; set; }
        public int? RoomId { get; set; }
        public int? AreaId { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? ShelfTypeId { get; set; }
        public string LocationCode { get; set; }
        public string ProductLotNo { get; set; }
        public DateTime? MakeDateS { get; set; }
        public DateTime? MakeDateE { get; set; }
        public DateTime? InvDateS { get; set; }
        public DateTime? InvDateE { get; set; }
        public DateTime? ExpirationDateS { get; set; }
        public DateTime? ExpirationDateE { get; set; }
    }
}
