using System;
using System.Collections.Generic;
using System.Text;

namespace SAFETYModel
{
    public class QueryHouse
    {        
        /// <summary>
        /// 代碼
        /// </summary>
        public string HouseCode { get; set; }
        /// <summary>
        /// 名稱
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// 倉別類型id
        /// </summary>
        public int? HouseTypeId { get; set; }
        /// <summary>
        /// 物流中心id
        /// </summary>
        public int? DcId { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }

        //end class
    }
}
