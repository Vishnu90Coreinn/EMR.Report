using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IFacilityCategoryRepository
    {
        Task<IEnumerable<FacilityCategoryEntity>> GetFacilityCategoryDDLAsync(CancellationToken token);
        Task<FacilityCategoryEntity> CreateFacilityCategoryAsync(FacilityCategoryEntity facilityCategoryEntity, CancellationToken token);
        Task<FacilityCategoryEntity> UpdateFacilityCategoryAsync(FacilityCategoryEntity facilityCategoryEntity, CancellationToken token);
        Task<FacilityCategoryEntity> GetFacilityCategoryByIdAsync(int facilityCategoryId, CancellationToken token);
        Task<IEnumerable<FacilityCategoryEntity>> GetFacilityCategoryListAsync(CancellationToken token);
        Task<IEnumerable<FacilityCategoryEntity>> GetFacilityCategoryListByNameAsync(string facilityCategoryName, CancellationToken token);
    }
}