using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.Group
{
    public sealed class GetGroupDDLResponseDto
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }
    }
}
