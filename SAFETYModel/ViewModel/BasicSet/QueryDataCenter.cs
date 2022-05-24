using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryDataCenter
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string DcName { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string DcCode { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }
    }
}
