﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QuerySupplier
    {
        /// <summary>
        /// 代碼
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }

        //end class
    }
}
