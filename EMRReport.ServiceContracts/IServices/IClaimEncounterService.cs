using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IClaimEncounterService
    {
        Task<List<ClaimEncounterServiceObject>> CreateBulkClaimEncounterAsync(List<ClaimEncounterServiceObject> claimEncounterServiceObjectList, CancellationToken token);
        Task<List<ClaimEncounterServiceObject>> bulkCreateClaimEncounterFromDataTableAndValidateDOS(DataTable calimEncounterDataTable, ClaimBasketServiceObject ClaimBasket, bool IsFacilityDOS, DateTime? dateOfService, CancellationToken token);
    }
}
