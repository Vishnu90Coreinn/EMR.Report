using System;
using System.Collections.Generic;
using System.Text;

namespace EMRReport.Common.ProjectEnums
{
    public enum UserLoginTypeEnum
    {
        NotSet = 0,
        Admin = 1,
        FrontDesk = 2,
        Doctor = 3,
        Nurse = 4,
        Attender = 5,
        Common = 6,
        Coder = 7,
        ChiefNurse = 8,
        Biller = 9,
        RAAssigner = 10,
        Pharmacist = 11,
        ResubmissionUser = 12
    }
    public enum SignUpStatusEnum
    {
        NotSet = 0,
        New = 1,
        Approved = 2,
        Rejected = 3
    }

    public enum AuthorityTypeEnum
    {
        NotSet = 0,
        DOH = 1,
        DHA = 2,
        Both = 3
    }
    public enum RuleVersionEnum
    {
        NotSet = 0,
        ICD_2015 = 1,
        ICD_2018 = 2,
        AllActiveVersions = 3
    }
    public enum ApplicationTypeEnum
    {
        NotSet = 0,
        CodingValidator = 1,
        Scrubber = 2,
        CodingValidator_Scrubber = 3
    }

}
