using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class GroupControlEntity : BaseEntity
    {
        public int GroupControlId { get; set; }
        public int ControlId { get; set; }
        public int GroupId { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool Read { get; set; }
        public bool Delete { get; set; }
        public bool Menu { get; set; }
        public bool File { get; set; }
        public Guid GroupControlGuid { get; set; }
        public virtual ControlEntity controlEntity { get; set; }
        public virtual GroupEntity groupEntity { get; set; }
    }
}
