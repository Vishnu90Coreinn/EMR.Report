using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Control
{
    public sealed class GetControlDDLResponseDto
    {
        public int ControlID { get; set; }
        public string ControlName { get; set; }
    }
}
