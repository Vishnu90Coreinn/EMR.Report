using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IFacilityService
    {
        Task<FacilityServiceObject> FindFacilityByCodeAsync(string facilityCode, CancellationToken token);
        Task<FacilityServiceObject> GetFacilityByIdAsync(int facilityID, CancellationToken token);
        Task<FacilityServiceObject> GetFacilityByCodeAsync(string facilityCode, CancellationToken token);
        Task<FacilityServiceObject> CreateFacilityAsync(FacilityServiceObject facilityServiceObject, CancellationToken token);
        Task<FacilityServiceObject> UpdateFacilityAsync(FacilityServiceObject facilityServiceObject, CancellationToken token);
        Task<ICollection<FacilityServiceObject>> GetFacilityListAsync(CancellationToken token);
        Task<ICollection<FacilityServiceObject>> GetFacilityListByNameAsync(string facilityName, CancellationToken token);
        Task<string> BulkSaveFacilityAsync(IFormFile Excelfile, CancellationToken token);
        Task<ICollection<FacilityServiceObject>> GetFacilityDownloadListAsync(CancellationToken token);
        Task<ICollection<FacilityServiceObject>> GetFacilityDDLAsync(string facilityName, CancellationToken token);
        Task<ICollection<FacilityServiceObject>> GetFacilityDistinctListFromCodeListAsync(ICollection<FacilityServiceObject> facilityServiceObjectList, CancellationToken token);
    }
}
