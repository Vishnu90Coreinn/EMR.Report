using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IOrganizationService
    {
        Task<OrganizationServiceObject> CreateOrganizationAsync(OrganizationServiceObject organizationServiceObject, CancellationToken token);
        Task<OrganizationServiceObject> UpdateOrganizationAsync(OrganizationServiceObject organizationServiceObject, CancellationToken token);
        Task<OrganizationServiceObject> GetOrganizationByIDAsync(int OrganizationID, CancellationToken token);
        Task<ICollection<OrganizationServiceObject>> GetOrganizationListAsync(CancellationToken token);
        Task<ICollection<OrganizationServiceObject>> GetOrganizationListByNameAsync(string organizationName, CancellationToken token);
        Task<OrganizationServiceObject> GetOrganizationByNameAsync(string organizationName, CancellationToken token);
        Task<ICollection<OrganizationServiceObject>> GetOrganizationDistinctListFromNameListAsync(ICollection<OrganizationServiceObject> organizationServiceObjectList, CancellationToken token);
    }
}
