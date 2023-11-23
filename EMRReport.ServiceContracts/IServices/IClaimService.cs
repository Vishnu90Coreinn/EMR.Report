using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IClaimService
    {
        Task<List<ClaimServiceObject>> CreateBulkClaimAsync(List<ClaimServiceObject> claimServiceObjectList, CancellationToken token);
        Task<List<ClaimServiceObject>> GetClaimFromDataTableAsync(DataTable claimDataTable, ClaimBasketServiceObject ClaimBasket, string cardPattern, CancellationToken token);
        Task<List<ClaimServiceObject>> BulkCreateClaimWithClaimDetails(List<ClaimServiceObject> claimServiceObjectList, List<ClaimDiagnosisServiceObject> claimDiagnosisServiceObjectList, List<ClaimActivityServiceObject> claimActivityServiceObjectList, CancellationToken token);
    }
}
