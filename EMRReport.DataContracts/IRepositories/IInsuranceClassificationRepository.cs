using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IInsuranceClassificationRepository
    {
        Task<InsuranceClassificationEntity> GetInsuranceClassificationByNameAsync(string insuranceClassification, CancellationToken token);
    }
}