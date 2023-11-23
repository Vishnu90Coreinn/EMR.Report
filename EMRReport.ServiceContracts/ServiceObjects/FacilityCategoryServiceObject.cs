using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public class FacilityCategoryServiceObject
    {
        public int FacilityCategoryID { get; set; }
        public string FacilityCategoryName { get; set; }
        public bool Status { get; set; }
    }
}
