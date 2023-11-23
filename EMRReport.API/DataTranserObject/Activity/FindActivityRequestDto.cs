using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Activity
{
    public sealed class FindActivityRequestDto
    {
        public int ActivityID { get; set; }
        public int ActivityNumber { get; set; }
        public string ActivityName { get; set; }
    }
}
