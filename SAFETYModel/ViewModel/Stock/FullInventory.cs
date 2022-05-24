using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using SAFETYModel.DBModels;

namespace SAFETYModel
{
    /// <summary>
    ///  盤點單 主檔+明細
    /// </summary>
    public class FullInventory
    {
        /// <summary>
        /// 盤點單主檔
        /// </summary>
        public StockAdjustment StockAdjustment { get; set; }
        /// <summary>
        /// 盤點單明細
        /// </summary>
        public List<StockAdjustmentDetail> StockAdjustmentDetail { get; set; }

        //end class
    }
}
