using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using SAFETYModel.DBModels;

namespace SAFETYModel
{
    /// <summary>
    ///  出貨通知單 主檔+明細+上傳檔案
    /// </summary>
    public class FullShipping
    {
        /// <summary>
        /// 出貨通知單主檔
        /// </summary>
        public ShippingOrder ShippingOrder { get; set; }
        /// <summary>
        /// 出貨通知單明細
        /// </summary>
        public List<ShippingDetail> ShippingDetail { get; set; }
        /// <summary>
        /// 已上傳資料
        /// </summary>
        public List<UploadFiles> UploadFiles { get; set; }
        //end class
    }

    /// <summary>
    /// 主檔夾檔
    /// </summary>
    public class ShippingNewFile
    {
        // 新上傳附件
        public IFormFile UP_FILE { get; set; }
    }
}
