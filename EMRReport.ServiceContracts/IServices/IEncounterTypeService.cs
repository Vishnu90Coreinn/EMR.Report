using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IEncounterTypeService
    {
        Task<EncounterTypeServiceObject> CreateEncounterTypeAsync(EncounterTypeServiceObject encounterTypeServiceObject, CancellationToken token);
        Task<EncounterTypeServiceObject> UpdateEncounterTypeAsync(EncounterTypeServiceObject encounterTypeServiceObject, CancellationToken token);
        Task<EncounterTypeServiceObject> FindEncounterTypeByIDAsync(int ID, CancellationToken token);
        Task<ICollection<EncounterTypeServiceObject>> GetEncounterTypeListAsync(CancellationToken token);
        Task<ICollection<EncounterTypeServiceObject>> GetEncounterTypeListByNameAsync(string EncounterTypeName, CancellationToken token);
        Task<EncounterTypeServiceObject> GetEncounterTypeByNameAsync(string EncounterTypeName, CancellationToken token);
        Task<ICollection<EncounterTypeServiceObject>> GetEncounterTypeListFromNameListAsync(ICollection<EncounterTypeServiceObject> encounterTypeServiceObjectList, CancellationToken token);
    }
}
