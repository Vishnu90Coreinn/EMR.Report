using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface ICompanyRoleRepository
    {
        Task<CompanyRoleEntity> GetRoleByNameAsync(string roleName, CancellationToken token);
        Task<IEnumerable<CompanyRoleEntity>> GetCompanyRoleDDLAsync(CancellationToken token);
    }
}