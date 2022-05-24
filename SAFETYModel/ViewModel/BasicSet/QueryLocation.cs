using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel.ViewModel.BasicSet
{
    public class QueryLocation
    {
        /// <summary>
        /// 名稱
        /// </summary>
        public string LocationCode { get; set; }
        /// <summary>
        /// 代碼
        /// </summary>
        public int LayerId { get; set; }
        
        public string IsStackable { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }
    }
}
