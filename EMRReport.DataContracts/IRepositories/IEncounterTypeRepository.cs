using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IEncounterTypeRepository
    {
        Task<EncounterTypeEntity> CreateEncounterTypeAsync(EncounterTypeEntity encounterTypeEntity, CancellationToken token);
        Task<EncounterTypeEntity> UpdateEncounterTypeAsync(EncounterTypeEntity encounterTypeEntity, CancellationToken token);
        Task<EncounterTypeEntity> FindEncounterTypeByIDAsync(int ID, CancellationToken token);
        Task<IEnumerable<EncounterTypeEntity>> GetEncounterTypeListAsync(CancellationToken token);
        Task<IEnumerable<EncounterTypeEntity>> GetEncounterTypeListByNameAsync(string EncounterTypeName, CancellationToken token);
        Task<EncounterTypeEntity> GetEncounterTypeByNameAsync(string EncounterTypeName, CancellationToken token);
    }
}

