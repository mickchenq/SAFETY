using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel.ViewModel
{
    public class QueryRoom
    {
        public string RoomName { get; set; }
        public string RoomCode { get; set; }
        public int? DcId { get; set; }
        public int? HouseId { get; set; }
        public int? RoomTypeId { get; set; }
        public string IsStop { get; set; }
    }
}
