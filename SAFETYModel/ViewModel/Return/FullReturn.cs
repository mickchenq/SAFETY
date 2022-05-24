using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using SAFETYModel.DBModels;

namespace SAFETYModel
{
    /// <summary>
    ///  退貨通知單 主檔+明細+上傳檔案
    /// </summary>
    public class FullReturn
    {
        /// <summary>
        /// 退貨通知單主檔
        /// </summary>
        public ReturnedOrder ReturnedOrder { get; set; }
        /// <summary>
        /// 進貨通知單明細
        /// </summary>
        public List<ReturnedDetail> ReturnedDetail { get; set; }
        /// <summary>
        /// 已上傳資料
        /// </summary>
        public List<UploadFiles> UploadFiles { get; set; }
    }

    /// <summary>
    /// 主檔夾檔
    /// </summary>
    public class ReturnNewFile
    {
        // 新上傳附件
        public IFormFile UP_FILE { get; set; }
    }

}
