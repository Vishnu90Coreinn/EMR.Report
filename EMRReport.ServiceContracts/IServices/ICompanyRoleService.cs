using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface ICompanyRoleService
    {
        Task<CompanyRoleServiceObject> GetCompanyRoleByNameAsync(string facilityTypeName, CancellationToken token);
        Task<ICollection<CompanyRoleServiceObject>> GetCompanyRoleListFromNameListAsync(ICollection<CompanyRoleServiceObject> compnayRoleServiceObjectList, CancellationToken token);
        Task<ICollection<CompanyRoleServiceObject>> GetCompanyRoleDDLAsync(CancellationToken token);
    }
}
