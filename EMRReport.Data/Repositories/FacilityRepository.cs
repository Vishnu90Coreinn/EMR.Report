using EFCore.BulkExtensions;
using EMRReport.Common.Extensions;
using EMRReport.Data;
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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class FacilityRepository : IFacilityRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        private int companyID = 0;
        public FacilityRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
            companyID = 1;
        }
        public async Task<FacilityEntity> FindFacilityByCodeAsync(string FacilityCode, CancellationToken token)
        {
            FacilityCode = !string.IsNullOrEmpty(FacilityCode) ? FacilityCode.ToLower() : FacilityCode;
            return await _dbContext.Facility.FirstOrDefaultAsync(x => x.FacilityCode == FacilityCode, token);
        }
        public async Task<FacilityEntity> GetFacilityByIdAsync(int FacilityID, CancellationToken token)
        {
            return await _dbContext.Facility.FirstOrDefaultAsync(x => x.FacilityID == FacilityID, token);
        }
        public async Task<FacilityEntity> GetFacilityByNameAsync(string facilityName, CancellationToken token)
        {
            facilityName = !string.IsNullOrEmpty(facilityName) ? facilityName.ToLower() : facilityName;
            return await _dbContext.Facility.FirstOrDefaultAsync(x => x.FacilityName.ToLower().Equals(facilityName), token);
        }
        public async Task<FacilityNMEntity> GetFacilityByCodeAsync(string FacilityCode, CancellationToken token)
        {
            return await _dbContext.Facility.Where(x => x.FacilityCode.ToLower() == FacilityCode.ToLower()).Select(x => new FacilityNMEntity
            {
                ClaimCount = x.ClaimCount,
                FacilityCode = x.FacilityCode,
                FacilityID = x.FacilityID,
                FacilityName = x.FacilityName,
                FacilityType = x.facilityTypeEntity == null ? "" : x.facilityTypeEntity.FacilityTypeName,
                IsDOS = x.IsDOS,
                IsAbuDhabiDOS = x.IsAbuDhabiDOS,
                IsUnlimited = x.IsUnlimited,
                Organization = x.organizationEntity == null ? "" : x.organizationEntity.OrganizationName,
                Regulatory = x.regulatoryEntity == null ? "" : x.regulatoryEntity.RegulatoryName,
                SubscriptionEndDate = x.SubscriptionEndDate,
                SubscriptionStartDate = x.SubscriptionStartDate,
                Status = x.Status
            }).FirstOrDefaultAsync(token);
        }
        public async Task<FacilityEntity> CreateFacilityAsync(FacilityEntity facilityEntity, CancellationToken token)
        {
            if (facilityEntity.IsUnlimited)
                facilityEntity.ClaimCount = 0;
            if (facilityEntity.IsAbuDhabiDOS)
                facilityEntity.IsDOS = true;
            facilityEntity.CreatedBy = userID;
            facilityEntity.CreatedDate = DateTime.Now;
            facilityEntity.Status = true;
            facilityEntity.FacilityGuid = Guid.NewGuid();
            facilityEntity.CompanyID = companyID;
            facilityEntity.VisCoreLisenceTypeID = 2;
            await _dbContext.Facility.AddAsync(facilityEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return facilityEntity;
        }
        public async Task<FacilityEntity> UpdateFacilityAsync(FacilityEntity facilityEntity, CancellationToken token)
        {
            facilityEntity.ModifiedBy = userID;
            facilityEntity.ModifiedDate = DateTime.Now;
            _dbContext.Facility.Update(facilityEntity);
            await _dbContext.SaveChangesAsync(token);
            return facilityEntity;
        }
        public async Task<IEnumerable<FacilityNMEntity>> GetFacilityListAsync(CancellationToken token)
        {
            return await _dbContext.Facility.Select(x => new FacilityNMEntity
            {
                ClaimCount = x.ClaimCount,
                FacilityCode = x.FacilityCode,
                FacilityID = x.FacilityID,
                FacilityName = x.FacilityName,
                FacilityType = x.facilityTypeEntity == null ? "" : x.facilityTypeEntity.FacilityTypeName,
                IsDOS = x.IsDOS,
                IsAbuDhabiDOS = x.IsAbuDhabiDOS,
                IsUnlimited = x.IsUnlimited,
                Organization = x.organizationEntity == null ? "" : x.organizationEntity.OrganizationName,
                Regulatory = x.regulatoryEntity == null ? "" : x.regulatoryEntity.RegulatoryName,
                SubscriptionEndDate = x.SubscriptionEndDate,
                SubscriptionStartDate = x.SubscriptionStartDate,
                Status = x.Status
            }).ToListAsync(token);
        }
        public async Task<IEnumerable<FacilityNMEntity>> GetFacilityListByNameAsync(string facilityName, CancellationToken token)
        {
            facilityName = facilityName.ToLower();
            return await _dbContext.Facility.Where(x => x.FacilityName.ToLower().Contains(facilityName)).Select(x => new FacilityNMEntity
            {
                ClaimCount = x.ClaimCount,
                FacilityCode = x.FacilityCode,
                FacilityID = x.FacilityID,
                FacilityName = x.FacilityName,
                FacilityType = x.facilityTypeEntity == null ? "" : x.facilityTypeEntity.FacilityTypeName,
                IsDOS = x.IsDOS,
                IsAbuDhabiDOS = x.IsAbuDhabiDOS,
                IsUnlimited = x.IsUnlimited,
                Organization = x.organizationEntity == null ? "" : x.organizationEntity.OrganizationName,
                Regulatory = x.regulatoryEntity == null ? "" : x.regulatoryEntity.RegulatoryName,
                SubscriptionEndDate = x.SubscriptionEndDate,
                SubscriptionStartDate = x.SubscriptionStartDate,
                Status = x.Status
            }).ToListAsync(token);
        }
        public async Task<List<FacilityEntity>> BulkCreateFacilityAsync(List<FacilityEntity> facilityEntityList, CancellationToken token)
        {
            for (int i = 0; i < facilityEntityList.Count(); i++)
            {
                var item = facilityEntityList.Skip(i).Take(1).FirstOrDefault();
                if (item.IsUnlimited)
                    item.ClaimCount = 0;
                item.CreatedBy = userID;
                item.CreatedDate = DateTime.Now;
                item.Status = true;
                item.FacilityGuid = Guid.NewGuid();
                item.CompanyID = companyID;
                item.VisCoreLisenceTypeID = 2;
            }
            await _dbContext.BulkInsertAsync(facilityEntityList);
            return facilityEntityList;
        }
        public async Task<List<FacilityEntity>> BulkUpdateFacilityAsync(List<FacilityEntity> facilityEntityList, CancellationToken token)
        {
            List<FacilityEntity> dataList = new List<FacilityEntity>();
            var DataList = await _dbContext.Facility.ToListAsync(token);
            await Task.Run(() =>
            {
                for (int i = 0; i < facilityEntityList.Count(); i++)
                {
                    var item = facilityEntityList.Skip(i).Take(1).FirstOrDefault();
                    var data = DataList.FirstOrDefault(x => x.FacilityID == item.FacilityID);
                    if (data != null)
                    {
                        data.IsUnlimited = item.IsUnlimited;
                        if (item.IsUnlimited)
                            data.ClaimCount = 0;
                        data.FacilityName = item.FacilityName;
                        data.FacilityCode = item.FacilityCode;
                        if (item.IsAbuDhabiDOS)
                        {
                            data.IsDOS = true;
                            data.IsAbuDhabiDOS = true;
                        }
                        else
                            data.IsDOS = item.IsDOS;
                        data.RegulatoryID = item.RegulatoryID;
                        data.FacilityTypeID = item.FacilityTypeID;
                        data.OrganizationID = item.OrganizationID.HasValue ? item.OrganizationID : null;
                        data.SubscriptionStartDate = item.SubscriptionStartDate;
                        data.SubscriptionEndDate = item.SubscriptionEndDate;
                        data.ModifiedBy = userID;
                        data.ModifiedDate = DateTime.Now;
                        data.Status = item.Status;
                        dataList.Add(data);
                    }
                }
            });
            await _dbContext.BulkUpdateAsync(dataList);
            return facilityEntityList;
        }
        public async Task<IEnumerable<FacilityNMEntity>> ReadExcelFacilityAsync(IFormFile Excelfile, CancellationToken token)
        {
            ICollection<FacilityNMEntity> facilityEntityList = new List<FacilityNMEntity>();
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
                    facilityEntityList = (from DataRow dr in result.Tables[0].Rows
                                          select new FacilityNMEntity()
                                          {
                                              FacilityID = Convert.ToInt32(dr["FacilityID"]),
                                              FacilityName = dr["FacilityName"].ToString(),
                                              FacilityCode = dr["FacilityCode"].ToString(),
                                              FacilityType = dr["FacilityType"].ToString(),
                                              Regulatory = dr["Regulatory"].ToString(),
                                              IsDOS = dr["IsDOS"].ToString().ConvertStringToBool(),
                                              IsAbuDhabiDOS = dr["IsAbuDhabiDOS"].ToString().ConvertStringToBool(),
                                              Organization = dr["Organization"].ToString(),
                                              ClaimCount = dr["ClaimCount"].ToString().ConvertStringToInt(),
                                              SubscriptionStartDate = dr["SubscriptionStartDate"].ToString().ConvertStringToDateTime(),
                                              SubscriptionEndDate = dr["SubscriptionEndDate"].ToString().ConvertStringToDateTime(),
                                              IsUnlimited = dr["IsUnlimited"].ToString().ConvertStringToBool()
                                          }).ToList();
                });
            }
            return facilityEntityList;

        }
        public async Task<IEnumerable<FacilityNMEntity>> GetFacilityDownloadListAsync(CancellationToken token)
        {
            return await _dbContext.Facility.Select(x => new FacilityNMEntity
            {
                FacilityID = x.FacilityID,
                FacilityName = x.FacilityName,
                FacilityCode = x.FacilityCode,
                FacilityType = x.facilityTypeEntity.FacilityTypeName,
                Organization = x.organizationEntity.OrganizationName,
                Regulatory = x.regulatoryEntity.RegulatoryName,
                IsDOS = x.IsDOS,
                IsAbuDhabiDOS = x.IsAbuDhabiDOS,
                ClaimCount = x.ClaimCount,
                SubscriptionStartDate = x.SubscriptionStartDate,
                SubscriptionEndDate = x.SubscriptionEndDate,
                IsUnlimited = x.IsUnlimited
            }).ToListAsync(token);
        }
        public async Task<IEnumerable<FacilityEntity>> GetFacilityDDLAsync(string facilityName, CancellationToken token)
        {
            return await _dbContext.Facility.Select(x => new
            {
                x.FacilityID,
                x.FacilityCode,
                x.FacilityName,
                FacilitySearch = x.FacilityCode + " " + x.FacilityName
            }).Where(x => string.IsNullOrEmpty(facilityName) ? true : x.FacilitySearch.ToLower().Contains(facilityName.ToLower()))
            .Take(20).Select(x => new FacilityEntity
            {
                FacilityID = x.FacilityID,
                FacilityCode = x.FacilityCode,
                FacilityName = x.FacilityName,
            }).ToListAsync(token);

        }
    }
}
