using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IBasketGroupService
    {
        Task<BasketGroupServiceObject> CreateBasketGroupAsync(BasketGroupServiceObject basketGroupServiceObject, CancellationToken token);
    }
}
