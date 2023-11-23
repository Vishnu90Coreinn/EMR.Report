using EFCore.BulkExtensions;
using EMRReport.Common.Extensions;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.DataContracts.NotMappedEntities;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class PayerReceiverRepository : IPayerReceiverRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public PayerReceiverRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<IEnumerable<PayerReceiverNMEntity>> GetPayerReceiverListAsync(int facilityID, CancellationToken token)
        {
            return await _dbContext.PayerReceiver.Where(x => x.FacilityID == facilityID).Take(100)
                .Select(x => new PayerReceiverNMEntity
                {
                    PayerReceiverID = x.PayerReceiverID,
                    PayerReceiverName = x.PayerReceiverName,
                    PayerReceiverShortName = x.PayerReceiverShortName,
                    Facility = x.facilityEntity.FacilityName,
                    InsuranceClassification = x.insuranceClassificationEntity.InsuranceClassification,
                    PayerReceiverIdentification = x.PayerReceiverIdentification,
                    Regulatory = x.regulatoryEntity.RegulatoryName,
                    Status = x.Status
                }).ToListAsync(token);
        }
        public async Task<IEnumerable<PayerReceiverNMEntity>> GetPayerReceiverListByNameAsync(int facilityID, string payerReceiverName, CancellationToken token)
        {
            payerReceiverName = !string.IsNullOrEmpty(payerReceiverName) ? payerReceiverName.ToLower() : "";
            return await _dbContext.PayerReceiver.Where(x => x.FacilityID == facilityID).Where(x => string.IsNullOrEmpty(payerReceiverName) ? true : x.PayerReceiverName.ToLower().Contains(payerReceiverName)).Take(50)
                .Select(x => new PayerReceiverNMEntity
                {
                    PayerReceiverID = x.PayerReceiverID,
                    PayerReceiverName = x.PayerReceiverName,
                    PayerReceiverShortName = x.PayerReceiverShortName,
                    Facility = x.facilityEntity.FacilityName,
                    InsuranceClassification = x.insuranceClassificationEntity.InsuranceClassification,
                    PayerReceiverIdentification = x.PayerReceiverIdentification,
                    Regulatory = x.regulatoryEntity.RegulatoryName,
                    Status = x.Status
                }).ToListAsync(token);
        }
        public async Task<IEnumerable<PayerReceiverEntity>> GetPayerReceiverDDLAsync(int facilityID, string payerReceiverName, CancellationToken token)
        {
            payerReceiverName = !string.IsNullOrEmpty(payerReceiverName) ? payerReceiverName.ToLower() : "";
            return await _dbContext.PayerReceiver.Where(x => x.FacilityID == facilityID).Where(x => string.IsNullOrEmpty(payerReceiverName) ? true : x.PayerReceiverName.ToLower().Contains(payerReceiverName)).Take(20)
                .Select(x => new PayerReceiverEntity { PayerReceiverID = x.PayerReceiverID, PayerReceiverName = x.PayerReceiverName }).ToListAsync(token);
        }
        public async Task<PayerReceiverEntity> GetPayerReceiverByIdAsync(int payerReceiverID, CancellationToken token)
        {
            return await _dbContext.PayerReceiver.FirstOrDefaultAsync(x => x.PayerReceiverID == payerReceiverID, token);
        }
        public async Task<PayerReceiverEntity> GetPayerReceiverByIdentificationAsync(int facilityID, string identification, CancellationToken token)
        {
            return await _dbContext.PayerReceiver.FirstOrDefaultAsync(x => x.FacilityID == facilityID && x.PayerReceiverIdentification.ToLower() == identification.ToLower(), token);
        }
        public async Task<PayerReceiverEntity> CreatePayerReceiverAsync(PayerReceiverEntity payerReceiverEntity, CancellationToken token)
        {
            payerReceiverEntity.Status = true;
            payerReceiverEntity.PayerReceiverGuid = Guid.NewGuid();
            payerReceiverEntity.CreatedBy = userID;
            payerReceiverEntity.CreatedDate = DateTime.Now;
            await _dbContext.PayerReceiver.AddAsync(payerReceiverEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return payerReceiverEntity;
        }
        public async Task<PayerReceiverEntity> UpdatePayerReceiverAsync(PayerReceiverEntity payerReceiverEntity, CancellationToken token)
        {
            payerReceiverEntity.ModifiedBy = userID;
            payerReceiverEntity.ModifiedDate = DateTime.Now;
            _dbContext.PayerReceiver.Update(payerReceiverEntity);
            await _dbContext.SaveChangesAsync(token);
            return payerReceiverEntity;
        }
        public async Task<IEnumerable<PayerReceiverNMEntity>> ReadPayerReceiverFromExcelAsync(IFormFile Excelfile, CancellationToken token)
        {
            ICollection<PayerReceiverNMEntity> payerReceiverNMEntityList = new List<PayerReceiverNMEntity>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                Excelfile.CopyTo(stream);
                stream.Position = 0;
                await Task.Run(() =>
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream);
                    DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    payerReceiverNMEntityList = (from DataRow dr in result.Tables[0].Rows
                                                 select new PayerReceiverNMEntity()
                                                 {
                                                     PayerReceiverID = Convert.ToInt32(dr["PayerReceiverID"]),
                                                     PayerReceiverName = dr["PayerReceiverName"].ToString(),
                                                     PayerReceiverShortName = dr["PayerReceiverShortName"].ToString(),
                                                     Facility = dr["Facility"].ToString(),
                                                     InsuranceClassification = dr["InsuranceClassification"].ToString(),
                                                     PayerReceiverIdentification = dr["PayerReceiverIdentification"].ToString(),
                                                     Status = dr["Status"].ToString().ConvertStringToBool()
                                                 }).ToList();
                });
            }
            return payerReceiverNMEntityList;
        }
        public async Task<List<PayerReceiverEntity>> BulkCreatePyaerReceiverAsync(List<PayerReceiverEntity> facilityEntityList, CancellationToken token)
        {
            for (int i = 0; i < facilityEntityList.Count(); i++)
            {
                var item = facilityEntityList.Skip(i).Take(1).FirstOrDefault();
                item.ReceiverID = item.PayerReceiverIdentification;
                item.PayerReceiverIdentificationValidate = item.PayerReceiverIdentification;
                item.CreatedBy = userID;
                item.CreatedDate = DateTime.Now;
                item.PayerReceiverGuid = Guid.NewGuid();
            }
            await _dbContext.BulkInsertAsync(facilityEntityList);
            return facilityEntityList;
        }
        public async Task<List<PayerReceiverEntity>> BulkUpdatePyaerReceiverAsync(List<PayerReceiverEntity> facilityEntityList, CancellationToken token)
        {
            for (int i = 0; i < facilityEntityList.Count(); i++)
            {
                var item = facilityEntityList.Skip(i).Take(1).FirstOrDefault();
                var data = await GetPayerReceiverByIdAsync(item.PayerReceiverID, token);
                data.PayerReceiverName = item.PayerReceiverName;
                data.PayerReceiverShortName = item.PayerReceiverShortName;
                data.PayerReceiverIdentification = item.PayerReceiverIdentification;
                data.ReceiverID = item.PayerReceiverIdentification;
                data.PayerReceiverIdentificationValidate = item.PayerReceiverIdentification;
                data.FacilityID = item.FacilityID;
                data.InsuranceClassificationID = item.InsuranceClassificationID;
                data.RegulatoryID = item.RegulatoryID;
                data.Status = item.Status;
                data.ModifiedBy = userID;
                data.ModifiedDate = DateTime.Now;
                data.Status = item.Status;
            }
            await _dbContext.BulkInsertAsync(facilityEntityList);
            return facilityEntityList;
        }
        public async Task<IEnumerable<PayerReceiverNMEntity>> GetPayerReceiverDownloadListAsync(int facilityID, CancellationToken token)
        {
            return await _dbContext.PayerReceiver.Where(x => x.FacilityID == facilityID)
                .Select(x => new PayerReceiverNMEntity
                {
                    PayerReceiverID = x.PayerReceiverID,
                    PayerReceiverName = x.PayerReceiverName,
                    PayerReceiverShortName = x.PayerReceiverShortName,
                    Facility = x.facilityEntity.FacilityCode,
                    InsuranceClassification = x.insuranceClassificationEntity.InsuranceClassification,
                    PayerReceiverIdentification = x.PayerReceiverIdentification,
                    Regulatory = x.regulatoryEntity.RegulatoryName,
                    Status = x.Status
                }).ToListAsync(token);
        }
    }
}