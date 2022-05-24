using SAFETYModel.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFETYModel
{
    public class UserData
    {
        public SysUser SysUser { get;set;}
        public List<UserRoleFunction> UserRoleFunction { get;set; }
    }
}
