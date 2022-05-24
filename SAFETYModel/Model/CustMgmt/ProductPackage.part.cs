using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SAFETYModel.DBModels
{
    [ModelMetadataType(typeof(ProductPackageMetadate))]
    public partial class ProductPackage
    {
        private sealed class ProductPackageMetadate
        {
            [Required(ErrorMessage = "包裝名稱必填")]
            public string PackageName { get; set; }
            [Required(ErrorMessage = "商品必填")]
            public int ProductId { get; set; }
            [Required(ErrorMessage = "包裝長度必填")]
            public decimal Length { get; set; }
            [Required(ErrorMessage = "包裝寬度必填")]
            public decimal Width { get; set; }
            [Required(ErrorMessage = "包裝高度必填")]
            public decimal Height { get; set; }
            [Required(ErrorMessage = "重量必填")]
            public decimal Weight { get; set; }
            [Required(ErrorMessage = "上層包裝必填")]
            public int ParentPackageId { get; set; }
            [Required(ErrorMessage = "上層包裝入數必填")]
            public int ParentPackageQuantity { get; set; }
            [Required(ErrorMessage = "是否為最小單位包裝必填")]
            public string IsMinSku { get; set; }
            [Required(ErrorMessage = "狀態必填")]
            public string IsStop { get; set; }
        }

        //end class
    }
}
