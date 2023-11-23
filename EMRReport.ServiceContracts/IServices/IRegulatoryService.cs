using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IRegulatoryService
    {
        Task<RegulatoryServiceObject> GetRegulatoryByNameAsync(string regulatoryName, CancellationToken token);
        Task<ICollection<RegulatoryServiceObject>> GetRegulatoryDistinctListFromNameListAsync(ICollection<RegulatoryServiceObject> regulatoryServiceObjectList, CancellationToken token);
        Task<ICollection<RegulatoryServiceObject>> GetRegulatoryDDLAsync(CancellationToken token);
        Task<RegulatoryServiceObject> CreateRegulatoryAsync(RegulatoryServiceObject regulatoryServiceObject, CancellationToken token);
        Task<RegulatoryServiceObject> UpdateRegulatoryAsync(RegulatoryServiceObject regulatoryServiceObject, CancellationToken token);
        Task<ICollection<RegulatoryServiceObject>> GetRegulatoryListAsync(CancellationToken token);
        Task<ICollection<RegulatoryServiceObject>> GetRegulatoryListByNameAsync(string RegulatoryName, CancellationToken token);
        Task<RegulatoryServiceObject> GetRegulatoryByIdAsync(int RegulatoryId, CancellationToken token);
    }
}
