using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(ShippingOrderMetadata))]
    public partial class ShippingOrder
    {
        private sealed class ShippingOrderMetadata
        {
            [Required(ErrorMessage = "物流中心必填")]
            public int DcId { get; set; }
            [Required(ErrorMessage = "客戶必填")]
            public int CustomerId { get; set; }
            [Required(ErrorMessage = "預計出貨日期必填")]
            public DateTime? EstimatedShippingDate { get; set; }
            [Required(ErrorMessage = "預計到貨日期必填")]
            public DateTime? EstimatedReceiveDate { get; set; }
            [Required(ErrorMessage = "作業方式必填")]
            public byte OperationType { get; set; }
            [Required(ErrorMessage = "出貨方式必填")]
            public byte ArriveType { get; set; }
            [Required(ErrorMessage = "聯絡人必填")]
            public string ContactName { get; set; }
            [Required(ErrorMessage = "聯絡人電話必填")]
            public string ContactPhone { get; set; }
            [Required(ErrorMessage = "聯絡人地址必填")]
            public string ContactAddress { get; set; }            
        }

        //end class
    }
}
