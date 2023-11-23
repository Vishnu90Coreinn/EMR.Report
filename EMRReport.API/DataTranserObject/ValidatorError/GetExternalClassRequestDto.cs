using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.ValidatorError
{
    public sealed class GetExternalClassRequestDto
    {
        public List<GetValidatorCPTRequestDto> ValidatorCPTList { get; set; }
        public List<GetValidatorICDRequestDto> ValidatorICDList { get; set; }
        public bool sequenceCPT { get; set; }
        public bool sequenceICD { get; set; }
        public string CPTS { get; set; }
        public string ICDS { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Classification { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
