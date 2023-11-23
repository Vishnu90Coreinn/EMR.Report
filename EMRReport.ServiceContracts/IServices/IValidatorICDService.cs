using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IValidatorICDService
    {
        Task<List<ValidatorICDServiceObject>> BulkCreateValidatorICDAsync(List<ValidatorICDServiceObject> validatorICDServiceObjectList, int ValidatorTransactionID, int Age, string Gender, CancellationToken token);
    }
}
