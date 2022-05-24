using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(ProductMetadate))]
    public partial class Product
    {
        private sealed class ProductMetadate
        {
            [Required(ErrorMessage = "商品代碼必填")]
            public string ProductCode { get; set; }
            [Required(ErrorMessage = "商品名稱必填")]
            public string ProductName { get; set; }
            [Required(ErrorMessage = "客戶必填")]
            public int CustomerId { get; set; }
            [Required(ErrorMessage = "條碼必填")]
            public string Barcode { get; set; }
            [Required(ErrorMessage = "温層必填")]
            public int? TempLayerId { get; set; }
            [Required(ErrorMessage = "商品分類必填")]
            public int? CategoryId { get; set; }
            [Required(ErrorMessage = "商品型態必填")]
            public int? TypeId { get; set; }
            [Required(ErrorMessage = "供應商必填")]
            public int? SupplierId { get; set; }
            [Required(ErrorMessage = "狀態必填")]
            public string IsStop { get; set; }
        }

        //end class
    }
}
