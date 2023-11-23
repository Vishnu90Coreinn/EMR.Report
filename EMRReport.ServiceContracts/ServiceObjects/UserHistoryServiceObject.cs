using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public class UserHistoryServiceObject
    {
        public long UserHistoryID { get; set; }
        public string LoginSessionID { get; set; }
        public string LoginIPAddress { get; set; }
        public string Browser { get; set; }
        public string Platform { get; set; }
        public bool IsLogOut { get; set; }
        public bool IsSessionOut { get; set; }
        public int LoginUserID { get; set; }
        public int? LoginRoleID { get; set; }
        public DateTime LogInTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public DateTime? SessionOutTime { get; set; }
        public int Day { get; set; }
    }
}
