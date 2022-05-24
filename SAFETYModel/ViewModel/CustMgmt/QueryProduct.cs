using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryProduct
    {
        /// <summary>
        /// 代碼
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 客戶
        /// </summary>
        public int? CustomerId { get; set; }
        /// <summary>
        /// 温層
        /// </summary>
        public int? TempLayerId { get; set; }
        /// <summary>
        /// 產品分類
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// 產品型態
        /// </summary>
        public int? TypeId { get; set; }
        /// <summary>
        /// 供應商
        /// </summary>
        public int? SupplierId { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }

        //end class
    }
}
