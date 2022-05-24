using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAFETYModel.OldDBModels
{
    [ModelMetadataType(typeof(NoticeDtMetadata))]
    public partial class NotificationDetail
    {
        private sealed class NoticeDtMetadata
        {
            [Required(ErrorMessage ="通知單Id必填")]
            public int? OrderId { get; set; }
            [Required(ErrorMessage ="商品必填")]
            public int? ProductId { get; set; }
            [Required(ErrorMessage = "單位必填")]
            public byte? Unit { get; set; }
            [Required(ErrorMessage ="預計進貨單位數量必填")]
            public int? Quantity { get; set; }
            [Required(ErrorMessage ="有效日期必填")]
            public DateTime? ExpirationDate { get; set; }
        }
    }
}
