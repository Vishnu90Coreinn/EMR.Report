using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class RoleGroupEntity : BaseEntity
    {
        public int RoleGroupId { get; set; }
        public int RoleId { get; set; }
        public int GroupId { get; set; }
        public Guid RoleGroupGuid { get; set; }
        public virtual GroupEntity groupEntity { get; set; }
        public virtual CompanyRoleEntity companyRoleEntity { get; set; }
    }
}
