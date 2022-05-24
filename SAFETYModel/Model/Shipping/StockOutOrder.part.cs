using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace SAFETYModel.DBModels
{
    public partial class StockOutOrder
    {
        [ModelMetadataType(typeof(StockOutOrderMetadata))]
        private sealed class StockOutOrderMetadata
        {
            [Required(ErrorMessage = "出貨通知單號必填")]
            public int RelatedId { get; set; }
            [Required(ErrorMessage = "出庫日期必填")]
            public DateTime OrderDate { get; set; }
            [Required(ErrorMessage = "出庫人員必填")]
            public int StockUserId { get; set; }
        }

        //end class
    }
}
