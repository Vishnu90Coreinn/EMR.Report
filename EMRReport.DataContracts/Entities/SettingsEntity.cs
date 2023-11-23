using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.DataContracts.Entities
{
    public partial class SettingsEntity : BaseEntity
    {
        public int ID { get; set; }
        public int ProjectConstantID { get; set; }
        public string ProjectConstantName { get; set; }
        public string ProjectConstantDBID { get; set; }
        public Guid SettingsTransGuid { get; set; }
    }
}
