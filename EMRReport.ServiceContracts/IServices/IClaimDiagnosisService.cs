using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IClaimDiagnosisService
    {
        Task<List<ClaimDiagnosisServiceObject>> CreateBulkClaimDiagnosisAsync(List<ClaimDiagnosisServiceObject> claimDiagnosisServiceObjectList, CancellationToken token);
        Task<List<ClaimDiagnosisServiceObject>> BulkCreateClaimDaignosisFromDataTable(DataTable claimdiagnosisDatatable, ClaimBasketServiceObject ClaimBasket, CancellationToken token);
    }
}
