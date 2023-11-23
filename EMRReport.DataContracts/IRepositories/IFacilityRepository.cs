using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.NotMappedEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IFacilityRepository
    {
        Task<FacilityEntity> GetFacilityByIdAsync(int FacilityID, CancellationToken token);
        Task<FacilityEntity> FindFacilityByCodeAsync(string FacilityCode, CancellationToken token);
        Task<FacilityNMEntity> GetFacilityByCodeAsync(string FacilityCode, CancellationToken token);
        Task<FacilityEntity> CreateFacilityAsync(FacilityEntity facilityEntity, CancellationToken token);
        Task<FacilityEntity> UpdateFacilityAsync(FacilityEntity facilityEntity, CancellationToken token);
        Task<IEnumerable<FacilityNMEntity>> GetFacilityListAsync(CancellationToken token);
        Task<IEnumerable<FacilityNMEntity>> GetFacilityListByNameAsync(string facilityName, CancellationToken token);
        Task<IEnumerable<FacilityNMEntity>> ReadExcelFacilityAsync(IFormFile Excelfile, CancellationToken token);
        Task<List<FacilityEntity>> BulkCreateFacilityAsync(List<FacilityEntity> facilityEntityList, CancellationToken token);
        Task<List<FacilityEntity>> BulkUpdateFacilityAsync(List<FacilityEntity> facilityEntityList, CancellationToken token);
        Task<IEnumerable<FacilityNMEntity>> GetFacilityDownloadListAsync(CancellationToken token);
        Task<IEnumerable<FacilityEntity>> GetFacilityDDLAsync(string facilityName, CancellationToken token);
        Task<FacilityEntity> GetFacilityByNameAsync(string facilityName, CancellationToken token);
    }
}
