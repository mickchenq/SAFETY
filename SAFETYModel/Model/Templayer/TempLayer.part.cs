using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAFETYModel.OldDBModels
{
    [ModelMetadataType(typeof(TempLayerMetadate))]
    public partial class TempLayer
    {
        private sealed class TempLayerMetadate
        {
            [Required(ErrorMessage ="代碼必填")]
            public string TempCode { get; set; }
            [Required(ErrorMessage ="名稱必填")]
            public string TempName { get; set; }
            [Required(ErrorMessage ="最高溫必填")]
            public decimal? MaxTemp { get; set; }
            [Required(ErrorMessage = "最低溫必填")]
            public decimal? MinTemp { get; set; }
            [Required(ErrorMessage = "狀態必填")]
            public string IsStop { get; set; }
        }
    }
}
