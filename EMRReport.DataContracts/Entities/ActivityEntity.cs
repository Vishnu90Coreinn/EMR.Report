using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public sealed class ActivityEntity : BaseEntity
    {
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public int ActivityNumber { get; set; }
        public Guid ActivityGuid { get; set; }
    }
}
