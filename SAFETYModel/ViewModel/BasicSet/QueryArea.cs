using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel.ViewModel.BasicSet
{
    public class QueryArea
    {
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int? RoomId { get; set; }
        public byte AreaType { get; set; }
        /// <summary>
        /// Y:停用 N:不停用
        /// </summary>
        public string IsStop { get; set; }
    }
}
