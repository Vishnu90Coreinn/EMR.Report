using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IClaimActivityObservationService
    {
        Task<List<ClaimActivityObservationServiceObject>> CreateBulkClaimActivityObservationAsync(List<ClaimActivityObservationServiceObject> claimActivityObservationServiceObjectList, CancellationToken token);
        Task<List<ClaimActivityObservationServiceObject>> BulkCreateClaimActivityObservationFromDataTable(DataTable calimActivityObservationDataTable, ClaimBasketServiceObject ClaimBasket, CancellationToken token);
    }
}
