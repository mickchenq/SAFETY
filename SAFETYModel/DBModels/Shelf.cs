using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class Shelf
    {
        public int ShelfId { get; set; }
        [Required(ErrorMessage = "代碼為必填")]
        public string ShelfCode { get; set; }
        [Required(ErrorMessage = "儲區為必填")]
        public int AreaId { get; set; }
        [Required(ErrorMessage = "類型為必填")]
        public int ShelfTypeId { get; set; }
        public byte Racks { get; set; }
        public int PrevShelfId { get; set; }
        public int NextShelfId { get; set; }
        public decimal? DownAisleWidth { get; set; }
        public decimal? UpAisleWidth { get; set; }
        public decimal? LeftAisleWidth { get; set; }
        public decimal? RightAisleWidth { get; set; }
        public decimal? Width { get; set; }
        public decimal? Length { get; set; }
        public decimal? X1 { get; set; }
        public decimal? X2 { get; set; }
        public decimal? Y1 { get; set; }
        public decimal? Y2 { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
