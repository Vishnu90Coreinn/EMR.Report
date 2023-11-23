using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IClaimActivityService
    {
        Task<List<ClaimActivityServiceObject>> CreateBulkClaimActivityAsync(List<ClaimActivityServiceObject> claimActivityServiceObjectList, CancellationToken token);
        Task<List<ClaimActivityServiceObject>> BulkCreateClaimActivityFromDataTable(DataTable claimActivityDataTable, ClaimBasketServiceObject ClaimBasket, CancellationToken token);
    }
}
