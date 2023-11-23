using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.ValidatorError
{
    public sealed class GetValidatorTransactionRequestDto
    {
        public List<GetValidatorCPTRequestDto> ValidatorCPTList { get; set; }
        public List<GetValidatorICDRequestDto> ValidatorICDList { get; set; }
        public bool sequenceCPT { get; set; }
        public bool sequenceICD { get; set; }
        public string CPTS { get; set; }
        public string ICDS { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}
