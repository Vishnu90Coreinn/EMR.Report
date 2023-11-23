using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IPayerReceiverService
    {
        Task<PayerReceiverServiceObject> GetPayerReceiverByIdAsync(int PayerReceiverID, CancellationToken token);
        Task<PayerReceiverServiceObject> GetPayerReceiverByIdentificationAsync(int FacilityID, string Identification, CancellationToken token);
        Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverDDLAsync(int facilityID, string payerReceiverName, CancellationToken token);
        Task<PayerReceiverServiceObject> CreateRegulatoryAsync(PayerReceiverServiceObject payerReceiverServiceObject, CancellationToken token);
        Task<PayerReceiverServiceObject> UpdateRegulatoryAsync(PayerReceiverServiceObject payerReceiverServiceObject, CancellationToken token);
        Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverListAsync(int facilityID, CancellationToken token);
        Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverListByNameAsync(int facilityID, string payerReceiverName, CancellationToken token);
        Task<string> BulkSavePayerReceiverAsync(IFormFile Excelfile, CancellationToken token);
        Task<ICollection<PayerReceiverServiceObject>> GetPayerReceiverDownloadListAsync(int facilityID, CancellationToken token);
    }
}
