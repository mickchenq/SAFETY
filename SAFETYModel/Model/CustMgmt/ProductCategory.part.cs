using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(ProductCategoryMetadate))]
    public partial class ProductCategory
    {
        private sealed class ProductCategoryMetadate
        {
            [Required(ErrorMessage = "商品分類代碼必填")]
            public string CategoryCode { get; set; }
            [Required(ErrorMessage = "商品分類名稱必填")]
            public string CategoryName { get; set; }            
            [Required(ErrorMessage = "狀態必填")]
            public string IsStop { get; set; }
        }

        //end class
    }
}
