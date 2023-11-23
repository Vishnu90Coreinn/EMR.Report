using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IInsuranceClassificationService
    {
        Task<InsuranceClassificationServiceObject> GetInsuranceClassificationByNameAsync(string insuranceClassification, CancellationToken token);
        Task<ICollection<InsuranceClassificationServiceObject>> GetInsuranceClassificationDistinctListFromNameListAsync(ICollection<InsuranceClassificationServiceObject> insuranceClassificationServiceObjectList, CancellationToken token);
    }
}
