using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(NoticeOrderMetadata))]
    public partial class NotificationOrder
    {
        private sealed class NoticeOrderMetadata
        {
            [Required(ErrorMessage ="客戶必填")]
            public int? CustomerId { get; set; }
            [Required(ErrorMessage = "物流中心必填")]
            public int? DcId { get; set; }
            //[Required(ErrorMessage = "系統進貨單號必填")]
            public string OrderNo { get; set; }
            [Required(ErrorMessage ="客戶進貨單號必填")]
            public string CustomerOrderNo { get; set; }
            [Required(ErrorMessage = "預計到貨日期必填")]
            public DateTime? EstimatedReceiveDate { get; set; }
            [Required(ErrorMessage ="進貨接單日期必填")]
            public DateTime? ActualReceiveDate { get; set; }
            [Required(ErrorMessage ="到貨方式必填")]
            public byte? ReceiveType { get; set; }
            [Required(ErrorMessage ="付款方式必填")]
            public byte? OperateType { get; set; }
            [Required(ErrorMessage ="緊急進貨必填")]
            public byte? IsUrgentOrder { get; set; }
            [Required(ErrorMessage ="聯絡人必填")]
            public string ContactName { get; set; }
            [Required(ErrorMessage = "聯絡人電話必填")]
            public string ContactPhone { get; set; }
            [Required(ErrorMessage = "聯絡人地址必填")] 
            public string ContactAddress { get; set; }
            [Required(ErrorMessage = "驗收人員必填")]
            public int? AcceptId { get; set; }

            [Required(ErrorMessage = "驗收日期必填")]
            public DateTime? AcceptDate { get; set; }           
        }
    }
}
