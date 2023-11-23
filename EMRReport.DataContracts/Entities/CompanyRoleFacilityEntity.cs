using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class CompanyRoleFacilityEntity : BaseEntity
    {
        public int CompanyRoleFacilityId { get; set; }
        public int CompanyRoleId { get; set; }
        public int FacilityId { get; set; }
        public int CompanyId { get; set; }
        public Guid CompanyRoleFacilityTranGuid { get; set; }
        public virtual CompanyEntity companyEntity { get; set; }
        public virtual CompanyRoleEntity companyRoleEntity { get; set; }
        public virtual FacilityEntity facilityEntity { get; set; }
    }
}
