using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel
{
    public class QueryHouseType
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string temp_name { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string temp_code { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string is_stop { get; set; }
    }
}
