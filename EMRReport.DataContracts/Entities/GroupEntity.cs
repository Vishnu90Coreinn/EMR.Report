using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class GroupEntity : BaseEntity
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int? Priority { get; set; }
        public string GroupClass { get; set; }
        public int? ParentGroupId { get; set; }
        public Guid GroupGuid { get; set; }
        public virtual GroupEntity groupEntityParent { get; set; }
        public virtual ICollection<GroupControlEntity> groupControlEntityList { get; set; }
        public virtual ICollection<GroupEntity> goupEntityParentList { get; set; }
        public virtual ICollection<RoleGroupEntity> roleGroupEntityList { get; set; }
    }
}
