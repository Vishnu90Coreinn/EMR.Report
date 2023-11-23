using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IValidatorCPTRepository
    {
        Task<List<ValidatorCPTEntity>> BulkCreateValidatorCPTAsync(List<ValidatorCPTEntity> validatorCPTEntityList, CancellationToken token);
    }
}
