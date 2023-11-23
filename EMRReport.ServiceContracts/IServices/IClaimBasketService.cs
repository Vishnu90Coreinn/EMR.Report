using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IClaimBasketService
    {
        Task<ClaimBasketServiceObject> GetClaimBasketByIdAsync(int ClaimBasketID, CancellationToken token);
        Task<ClaimBasketServiceObject> CreateClaimBasketAsync(ClaimBasketServiceObject claimBasketServiceObject, CancellationToken token);
        Task<List<ClaimBasketServiceObject>> CreateBulkClaimBasketAsync(List<ClaimBasketServiceObject> claimBasketServiceObjectList, CancellationToken token);
        Task<Tuple<string, bool, bool, ClaimBasketServiceObject>> CreateClaimBasketFromXMLFilesAsync(IFormFile XMLfile, int basketGroupID, bool IsScrubberDemo, CancellationToken token);
    }
}
