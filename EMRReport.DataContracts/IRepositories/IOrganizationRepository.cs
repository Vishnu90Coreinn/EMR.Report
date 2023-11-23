using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IOrganizationRepository
    {
        Task<OrganizationEntity> CreateOrganizationAsync(OrganizationEntity organizationEntity, CancellationToken token);
        Task<OrganizationEntity> UpdateOrganizationAsync(OrganizationEntity organizationEntity, CancellationToken token);
        Task<OrganizationEntity> GetOrganizationByIDAsync(int OrganizationID, CancellationToken token);
        Task<IEnumerable<OrganizationEntity>> GetOrganizationListAsync(CancellationToken token);
        Task<IEnumerable<OrganizationEntity>> GetOrganizationListByNameAsync(string organizationName, CancellationToken token);
        Task<OrganizationEntity> GetOrganizationByNameAsync(string organizationName, CancellationToken token);
    }
}