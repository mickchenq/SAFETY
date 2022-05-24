using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class Layer
    {
        public int LayerId { get; set; }
        [Required(ErrorMessage = "層架代碼為必填")]
        public string LayerCode { get; set; }
        public int ShelfId { get; set; }
        public byte CurrentLayer { get; set; }
        public short Fields { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
