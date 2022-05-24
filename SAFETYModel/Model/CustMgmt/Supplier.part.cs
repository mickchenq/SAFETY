using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(SupplierMetadate))]
    public partial class Supplier
    {
        private sealed class SupplierMetadate
        {
            [Required(ErrorMessage = "供應商代碼必填")]
            public string SupplierCode { get; set; }
            [Required(ErrorMessage = "供應商名稱必填")]
            public string SupplierName { get; set; }
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
