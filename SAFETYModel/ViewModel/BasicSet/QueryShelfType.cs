using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel
{
    public class QueryShelfType
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string ShelfTypeCode { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string ShelfTypeName { get; set; }
        
        public string IsFlat { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }
    }
}
