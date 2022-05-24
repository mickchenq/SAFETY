using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class Area
    {
        public int AreaId { get; set; }
        [Required(ErrorMessage = "儲區代碼為必填")]
        public string AreaCode { get; set; }
        [Required(ErrorMessage = "儲區名稱為必填")]
        public string AreaName { get; set; }
        public int RoomId { get; set; }
        public byte AreaType { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
