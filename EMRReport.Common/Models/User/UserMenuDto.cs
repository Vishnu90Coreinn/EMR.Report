using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.Common.Models.User
{
    public sealed class UserMenuDto
    {
        public string GroupName { get; set; }
        public string[] ControlList { get; set; }
    }
}
