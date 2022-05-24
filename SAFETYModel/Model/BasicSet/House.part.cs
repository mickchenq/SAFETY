using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(HouseMetadate))]
    public partial class House
    {
        private sealed class HouseMetadate
        {
            [Required(ErrorMessage = "倉別代碼必填")]
            public string HouseCode { get; set; }
            [Required(ErrorMessage = "倉別名稱必填")]
            public string HouseName { get; set; }
            [Required(ErrorMessage = "倉別類型必填")]
            public int? HouseTypeId { get; set; }
            [Required(ErrorMessage = "物流中心必填")]
            public int? DcId { get; set; }         
            [Required(ErrorMessage = "狀態必填")]
            public string IsStop { get; set; }
        }
        //end class
    }
}
