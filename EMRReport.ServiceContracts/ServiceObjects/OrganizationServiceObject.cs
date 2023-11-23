using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class OrganizationServiceObject
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int? ClaimCount { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public bool IsUnlimited { get; set; }
        public bool Status { get; set; }
    }
}
