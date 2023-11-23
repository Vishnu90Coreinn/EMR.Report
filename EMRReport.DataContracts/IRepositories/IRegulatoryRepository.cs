using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IRegulatoryRepository
    {
        Task<RegulatoryEntity> GetRegulatoryByNameAsync(string regulatoryName, CancellationToken token);
        Task<IEnumerable<RegulatoryEntity>> GetRegulatoryDDLAsync(CancellationToken token);
        Task<RegulatoryEntity> CreateRegulatoryAsync(RegulatoryEntity regulatoryEntity, CancellationToken token);
        Task<RegulatoryEntity> UpdateRegulatoryAsync(RegulatoryEntity regulatoryEntity, CancellationToken token);
        Task<RegulatoryEntity> GetRegulatoryByIdAsync(int regulatoryId, CancellationToken token);
        Task<IEnumerable<RegulatoryEntity>> GetRegulatoryListAsync(CancellationToken token);
        Task<IEnumerable<RegulatoryEntity>> GetRegulatoryListByNameAsync(string RegulatoryName, CancellationToken token);
    }
}