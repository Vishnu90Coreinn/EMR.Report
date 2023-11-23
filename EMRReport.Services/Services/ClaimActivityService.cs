using AutoMapper;
using EMRReport.Common.CodeEncryption;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class ClaimActivityService : IClaimActivityService
    {
        private readonly IMapper _mapper;
        private readonly IClaimActivityRepository _claimActivityRepository;
        public ClaimActivityService(IClaimActivityRepository claimActivityRepository, IMapper mapper)
        {
            _claimActivityRepository = claimActivityRepository;
            _mapper = mapper;
        }

        public async Task<List<ClaimActivityServiceObject>> CreateBulkClaimActivityAsync(List<ClaimActivityServiceObject> claimActivityServiceObjectList, CancellationToken token)
        {
            var claimActivityEntityList = _mapper.Map<List<ClaimActivityEntity>>(claimActivityServiceObjectList);
            claimActivityEntityList = await _claimActivityRepository.CreateBulkClaimActivityAsync(claimActivityEntityList, token);
            return _mapper.Map<List<ClaimActivityServiceObject>>(claimActivityEntityList);
        }
        public async Task<List<ClaimActivityServiceObject>> BulkCreateClaimActivityFromDataTable(DataTable claimActivityDataTable, ClaimBasketServiceObject ClaimBasket, CancellationToken token)
        {
            List<ClaimActivityServiceObject> claimActivityServiceObjectList = await Task.Run(() => (from row in claimActivityDataTable.AsEnumerable()
                                                                                                    select new ClaimActivityServiceObject
                                                                                                    {
                                                                                                        XMLClaimTagID = Convert.ToInt32(row["Claim_Id"]),
                                                                                                        ServiceCode = Convert.ToString(row["Code"]),
                                                                                                        Start = Convert.ToString(row["Start"]),
                                                                                                        ActivityType = Convert.ToInt32(row["Type"]),
                                                                                                        Quantity = Convert.ToDecimal(row["Quantity"]),
                                                                                                        ClaimBasketID = ClaimBasket.ClaimBasketID,
                                                                                                        ActivityID = Convert.ToString(row["ID"]),
                                                                                                        PriorAuthorizationID = row.Table.Columns["PriorAuthorizationID"] != null ? Convert.ToString(row["PriorAuthorizationID"]) == "" ? null : Convert.ToString(row["PriorAuthorizationID"]) : null,
                                                                                                        XMLActivityTagID = row.Table.Columns["Activity_Id"] != null ? Convert.ToString(row["Activity_Id"]) == "" ? null : (int?)Convert.ToInt32(row["Activity_Id"]) : null,
                                                                                                        ClinicianLicense = row.Table.Columns["OrderingClinician"] != null ? Convert.ToString(row["OrderingClinician"]) == "" ? "" : Convert.ToString(row["OrderingClinician"]) : "",
                                                                                                        ActivityClinicianLicense = row.Table.Columns["Clinician"] != null ? Convert.ToString(row["Clinician"]) == "" ? "" : Convert.ToString(row["Clinician"]) : "",
                                                                                                        Net = Convert.ToDecimal(row["Net"]),
                                                                                                        VAT = row.Table.Columns["VAT"] != null ? Convert.ToString(row["VAT"]) == "" ? 0 : Convert.ToDecimal(row["VAT"]) : 0,
                                                                                                    }).ToList());
            await Task.Run(() =>
            {
                for (int i = 0; i < claimActivityServiceObjectList.Count; i++)
                {
                    var activity = claimActivityServiceObjectList.Skip(i).Take(1).FirstOrDefault();
                    activity.NMServiceCode = activity.ServiceCode;
                    if (!string.IsNullOrEmpty(activity.ServiceCode))
                        activity.ServiceCode = activity.ServiceCode.EncryptCode();
                    if (!string.IsNullOrEmpty(activity.ClinicianLicense))
                        activity.OrderingClinician = activity.ClinicianLicense.EncryptCode();
                    if (!string.IsNullOrEmpty(activity.ActivityClinicianLicense))
                        activity.ActivityClinician = activity.ActivityClinicianLicense.EncryptCode();
                    activity.ActivityJoinId = i;
                }
            });
            await CreateBulkClaimActivityAsync(claimActivityServiceObjectList, token);
            return claimActivityServiceObjectList;
        }
    }
}
