using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Regulatory
{
    public sealed class FindRegulatoryResponceDto
    {
        public int RegulatoryID { get; set; }
        public string RegulatoryName { get; set; }
        public bool Status { get; set; }
    }
}
