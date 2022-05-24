using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel.DBModels
{
    public partial class SysFunction
    {
        public int FunctionId { get; set; }
        public string FunctionName { get; set; }
        public string FnGroup { get; set; }
        public string FnGroupName { get; set; }
        public string FnClass { get; set; }
        public string FnArea { get; set; }
        public string FnController { get; set; }
        public string FnPageName { get; set; }
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }


    }
}
