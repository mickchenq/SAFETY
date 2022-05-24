using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(ProductTypeMetadate))]
    public partial class ProductType
    {
        private sealed class ProductTypeMetadate
        {
            [Required(ErrorMessage = "商品類型代碼必填")]
            public string TypeCode { get; set; }
            [Required(ErrorMessage = "商品類型名稱必填")]
            public string TypeName { get; set; }
            [Required(ErrorMessage = "狀態必填")]
            public string IsStop { get; set; }
        }

        //endclass
    }
}
