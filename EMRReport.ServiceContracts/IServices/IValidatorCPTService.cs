using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IValidatorCPTService
    {
        Task<List<ValidatorCPTServiceObject>> BulkCreateValidatorCPTAsync(List<ValidatorCPTServiceObject> basketGroupServiceObjectList, int ValidatorTransactionID, int Age, string Gender, CancellationToken token);
    }
}
