using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IValidatorTransactionService
    {
        Task<ValidatorTransactionServiceObject> CreateValidatorTransactionAsync(ValidatorTransactionServiceObject validatorTransactionServiceObject, CancellationToken token);
    }
}
