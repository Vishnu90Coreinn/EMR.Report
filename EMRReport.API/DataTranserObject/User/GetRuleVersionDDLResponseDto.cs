using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.User
{
    public sealed class GetRuleVersionDDLResponseDto
    {
        public int RuleVersion { get; set; }
        public string RuleVersionName { get; set; }
    }
}
