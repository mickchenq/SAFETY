using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(CustomerMetadate))]
    public partial class Customer
    {
        private sealed class CustomerMetadate
        {
            [Required(ErrorMessage = "客戶代碼必填")]
            public string CustomerCode { get; set; }
            [Required(ErrorMessage = "客戶名稱必填")]
            public string CustomerName { get; set; }
            [Required(ErrorMessage = "國家必填")]
            public string CountryCode { get; set; }
            [Required(ErrorMessage = "城市必填")]
            public string City { get; set; }
            [Required(ErrorMessage = "地址必填")]
            public string Addr { get; set; }
            [Required(ErrorMessage = "郵遞區號必填")]
            public string ZipCode { get; set; }
            [Required(ErrorMessage = "統一編號必填")]
            public string VatNo { get; set; }
            [Required(ErrorMessage = "電話必填")]
            public string Phone { get; set; }
            [Required(ErrorMessage = "狀態必填")]
            public string IsStop { get; set; }           
        }

        //end class
    }
}
