using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Organization
{
    public sealed class GetOrganizationResponseDto
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int? ClaimCount { get; set; }
        public string SubscriptionStartDate { get; set; }
        public string SubscriptionEndDate { get; set; }
        public string IsUnlimited { get; set; }
        public string Status { get; set; }

    }
}
