using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using SAFETYModel.DBModels;

namespace SAFETYModel
{
    /// <summary>
    ///  上架儲位指派(入庫單） 主檔+明細+上傳檔案
    /// </summary>
    public class FullStockIn
    {
        /// <summary>
        /// 入庫單主檔
        /// </summary>
        public StockInOrder StockInOrder { get; set; }
        /// <summary>
        /// 入庫單明細
        /// </summary>
        public List<StockInDetail> StockInDetail { get; set; }
        /// <summary>
        /// 入庫單上架資料
        /// </summary>
        public List<StockInLocationDetail> StockInLocationDetail { get; set; }
        /// <summary>
        /// 已上傳資料
        /// </summary>
        public List<UploadFiles> UploadFiles { get; set; }
        //end class
    }

    /// <summary>
    /// 主檔夾檔
    /// </summary>
    public class StockInNewFile
    {
        // 新上傳附件
        public IFormFile UP_FILE { get; set; }
    }
}
