using System;
namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class ActivityServiceObject
    {
        public int ActivityID { get; set; }
        public int ActivityNumber { get; set; }
        public string ActivityName { get; set; }
        public bool Status { get; set; }
    }
}