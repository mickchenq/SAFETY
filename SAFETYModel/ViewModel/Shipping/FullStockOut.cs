using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using SAFETYModel.DBModels;

namespace SAFETYModel
{
    /// <summary>
    ///  出庫單 主檔+明細+上傳檔案
    /// </summary>
    public class FullStockOut
    {
        /// <summary>
        /// 出庫單主檔
        /// </summary>
        public StockOutOrder StockOutOrder { get; set; }
        /// <summary>
        /// 出庫單明細
        /// </summary>
        public List<StockOutDetail> StockOutDetail { get; set; }
        /// <summary>
        /// 出庫單下架資料
        /// </summary>
        public List<StockOutLocationDetail> StockOutLocationDetail { get; set; }
        /// <summary>
        /// 已上傳資料
        /// </summary>
        public List<UploadFiles> UploadFiles { get; set; }

        //end class
    }

    /// <summary>
    /// 主檔夾檔
    /// </summary>
    public class StockOutNewFile
    {
        // 新上傳附件
        public IFormFile UP_FILE { get; set; }
    }
}
