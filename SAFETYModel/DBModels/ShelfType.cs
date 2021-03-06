using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class ShelfType
    {
        public int ShelfTypeId { get; set; }
        [Required(ErrorMessage = "代碼為必填")]
        public string ShelfTypeCode { get; set; }
        [Required(ErrorMessage = "名稱為必填")]
        public string ShelfTypeName { get; set; }
        public string IsFlat { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
