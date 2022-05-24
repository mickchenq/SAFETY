using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class Room
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public int DcId { get; set; }
        public int HouseId { get; set; }
        public int RoomTypeId { get; set; }
        public int TempLayerId { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
