using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IStateRepository
    {
        Task<StateEntity> GetStateByNameAsync(string stateName, CancellationToken token);
    }
}