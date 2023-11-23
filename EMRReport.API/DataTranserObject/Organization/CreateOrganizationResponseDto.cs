using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Organization
{
    public sealed class CreateOrganizationResponseDto
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }

    }
}
