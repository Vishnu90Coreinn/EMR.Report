using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.FacilityType
{
    public sealed class GetFacilityTypeDDLResponseDto
    {
        public int FacilityTypeID { get; set; }
        public string FacilityTypeName { get; set; }
    }
}
