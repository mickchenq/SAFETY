using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class Location
    {
        public int LocationId { get; set; }
        [Required(ErrorMessage = "層架為必填")]
        public int LayerId { get; set; }
        [Required(ErrorMessage = "儲位閘道為必填")]
        public int TagAddr { get; set; }
        [Required(ErrorMessage = "代碼為必填")]
        public string LocationCode { get; set; }
        public short SequenceNo { get; set; }
        public short PlateQuantity { get; set; }
        public decimal MedianX { get; set; }
        public decimal MedianY { get; set; }
        public decimal Width { get; set; }
        public string IsStackable { get; set; }
        public string IsMixable { get; set; }
        public decimal Square { get; set; }
        public decimal Weight { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
        [Required(ErrorMessage = "儲位閘道為必填")]
        public int TagGateWay { get; set; }
    }
}
