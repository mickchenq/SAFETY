using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryProductPackage
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string PackageName { get; set; }
        /// <summary>
        /// 客戶
        /// </summary>
        public int? CustomerId { get; set; }
        /// <summary>
        /// 商品
        /// </summary>
        public int? ProductId { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }

        //end class
    }
}
