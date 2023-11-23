using EMRReport.DataContracts.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IBasketGroupRepository
    {
        Task<BasketGroupEntity> CreateBasketGroupAsync(BasketGroupEntity basketGroupEntiity, CancellationToken token);
    }
}
