using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.NotMappedEntities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IPayerReceiverRepository
    {
        Task<IEnumerable<PayerReceiverNMEntity>> GetPayerReceiverListAsync(int facilityID, CancellationToken token);
        Task<IEnumerable<PayerReceiverNMEntity>> GetPayerReceiverListByNameAsync(int facilityID, string payerReceiverName, CancellationToken token);
        Task<IEnumerable<PayerReceiverEntity>> GetPayerReceiverDDLAsync(int facilityID, string payerReceiverName, CancellationToken token);
        Task<PayerReceiverEntity> GetPayerReceiverByIdAsync(int payerReceiverID, CancellationToken token);
        Task<PayerReceiverEntity> GetPayerReceiverByIdentificationAsync(int facilityID, string identification, CancellationToken token);
        Task<PayerReceiverEntity> CreatePayerReceiverAsync(PayerReceiverEntity payerReceiverEntity, CancellationToken token);
        Task<PayerReceiverEntity> UpdatePayerReceiverAsync(PayerReceiverEntity regulatoryEntity, CancellationToken token);
        Task<IEnumerable<PayerReceiverNMEntity>> ReadPayerReceiverFromExcelAsync(IFormFile Excelfile, CancellationToken token);
        Task<List<PayerReceiverEntity>> BulkCreatePyaerReceiverAsync(List<PayerReceiverEntity> facilityEntityList, CancellationToken token);
        Task<List<PayerReceiverEntity>> BulkUpdatePyaerReceiverAsync(List<PayerReceiverEntity> facilityEntityList, CancellationToken token);
        Task<IEnumerable<PayerReceiverNMEntity>> GetPayerReceiverDownloadListAsync(int facilityID, CancellationToken token);
    }
}
