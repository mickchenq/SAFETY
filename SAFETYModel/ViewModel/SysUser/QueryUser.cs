using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class QueryUser
    {
        public string LoginId { get; set; }
        public string UserName { get; set; }
        public byte UserType { get; set; }
        public string IsStop { get; set; }
    }
}
