using EMRReport.DataContracts.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IValidatorTransactionRepository
    {
        Task<ValidatorTransactionEntity> CreateValidatorTransactionAsync(ValidatorTransactionEntity validatorTransactionEntity, CancellationToken token);
    }
}
