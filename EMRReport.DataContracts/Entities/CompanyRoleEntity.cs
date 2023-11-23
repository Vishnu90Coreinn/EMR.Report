using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class CompanyRoleEntity : BaseEntity
    {
        public int CompanyRoleId { get; set; }
        public string CompanyRole { get; set; }
        public int CompanyId { get; set; }
        public Guid CompanyRoleGuid { get; set; }
        public virtual CompanyEntity companyEntity { get; set; }
        public virtual ICollection<CompanyRoleFacilityEntity> companyRoleFacilityEntityList { get; set; }
        public virtual ICollection<RoleGroupEntity> roleGroupEntityList { get; set; }
        public virtual ICollection<UserEntity> userEntityList { get; set; }
        public virtual ICollection<UserHistoryEntity> userHistoryEntityList { get; set; }
    }
}
