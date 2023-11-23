using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IValidatorErrorService
    {
        Task<List<ValidatorErrorServiceObject>> GetDetialValidateAsync(List<ValidatorCPTServiceObject> validatorCPTServiceObjectList, List<ValidatorICDServiceObject> validatorICDServiceObjectList, string DateOfBirth, string Gender, bool sequenceCPT, bool sequenceICD, string CPTS, string ICDS, string Classification, CancellationToken token, string userName = "");
        Task<List<ValidatorErrorServiceObject>> GetValidatorAPPErrorAsync(ValidatorErrorServiceObject validatorErrorServiceObject, string Classification, CancellationToken token);
        Task<List<ValidatorErrorServiceObject>> GetValidatorErrorAsync(ValidatorErrorServiceObject validatorErrorServiceObject, CancellationToken token);
        Task<List<ValidatorErrorServiceObject>> GetServiceValidarErrorDescriptAsync(List<ValidatorErrorServiceObject> validatorErrorServiceObjectList, CancellationToken token);
    }
}
