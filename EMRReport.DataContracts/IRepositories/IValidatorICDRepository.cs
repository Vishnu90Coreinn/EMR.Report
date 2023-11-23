using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IValidatorICDRepository
    {
        Task<List<ValidatorICDEntity>> BulkCreateValidatorICDAsync(List<ValidatorICDEntity> validatorICDEntityList, CancellationToken token);
    }
}
