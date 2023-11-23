using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public class RegulatoryServiceObject
    {
        public int RegulatoryID { get; set; }
        public string RegulatoryName { get; set; }
        public bool Status { get; set; }
    }
}
