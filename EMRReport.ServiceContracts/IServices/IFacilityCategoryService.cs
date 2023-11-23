using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IFacilityCategoryService
    {
        Task<ICollection<FacilityCategoryServiceObject>> GetFacilityCategoryDDLAsync(CancellationToken token);
        Task<FacilityCategoryServiceObject> CreateFacilityCategoryAsync(FacilityCategoryServiceObject facilityCategoryServiceObject, CancellationToken token);
        Task<FacilityCategoryServiceObject> UpdateFacilityCategoryAsync(FacilityCategoryServiceObject facilityCategoryServiceObject, CancellationToken token);
        Task<ICollection<FacilityCategoryServiceObject>> GetFacilityCategoryListAsync(CancellationToken token);
        Task<ICollection<FacilityCategoryServiceObject>> GetFacilityCategoryListByNameAsync(string facilityCategoryName, CancellationToken token);
        Task<FacilityCategoryServiceObject> FindFacilityCategoryByIdAsync(int facilityCategoryId, CancellationToken token);
    }
}
