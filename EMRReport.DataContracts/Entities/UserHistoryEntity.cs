using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EMRReport.DataContracts.Entities
{
    public partial class UserHistoryEntity
    {
        public long UserHistoryID { get; set; }
        public string LoginSessionID { get; set; }
        public string LoginIPAddress { get; set; }
        public string Browser { get; set; }
        public string Platform { get; set; }
        public bool IsLogOut { get; set; }
        public bool IsSessionOut { get; set; }
        public int LoginUserID { get; set; }
        public int? LoginRoleID { get; set; }
        public DateTime LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public DateTime? SessionOutTime { get; set; }
        public int Day { get; set; }
        public virtual UserEntity userEntity { get; set; }
        public virtual CompanyRoleEntity companyRoleEntity { get; set; }

    }
}