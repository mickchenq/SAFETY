using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SAFETYModel.DBModels
{
    public partial class SysUser
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "使用者名稱必填")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "使用者帳號必填")]
        public string LoginId { get; set; }       
        public byte UserType { get; set; }
        [Required(ErrorMessage = "密碼必填")]
        public string LoginPwd { get; set; }      
        public string Remarks { get; set; }
        public string IsStop { get; set; }
        public int CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ModifyId { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
