using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.FacilityCategory
{
    public sealed class FindFacilityCategoryResponseDto
    {
        public int FacilityCategoryID { get; set; }
        public string FacilityCategoryName { get; set; }
        public bool Status { get; set; }
    }
}
