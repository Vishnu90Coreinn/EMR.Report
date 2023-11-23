using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.FacilityCategory
{
    public sealed class GetFacilityCategoryListResponseDto
    {
        public int FacilityCategoryID { get; set; }
        public string FacilityCategoryName { get; set; }
        public string Status { get; set; }
    }
}
