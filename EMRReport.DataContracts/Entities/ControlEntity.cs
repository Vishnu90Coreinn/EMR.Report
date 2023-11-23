using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class ControlEntity
    {
        public int ControlId { get; set; }
        public string ControlName { get; set; }
        public int? Priority { get; set; }
        public bool Status { get; set; }
        public Guid ControlGuid { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserEntity userEntityCreated { get; set; }
        public virtual ICollection<GroupControlEntity> groupControlEntityList { get; set; }
    }
}
