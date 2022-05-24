using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel
{
    public class QueryShelf
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string ShelfCode { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public string AreaId { get; set; }
        
        public string ShelfTypeId { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }
    }
}
