using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Facility
{
    public sealed class UpdateFacilityResponseDto
    {
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
    }
}
