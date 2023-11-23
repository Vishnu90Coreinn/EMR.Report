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
    public sealed class ClaimActivityObservationService : IClaimActivityObservationService
    {
        private readonly IMapper _mapper;
        private readonly IClaimActivityObservationRepository _claimActivityObservationRepository;
        public ClaimActivityObservationService(IClaimActivityObservationRepository claimActivityObservationRepository, IMapper mapper)
        {
            _claimActivityObservationRepository = claimActivityObservationRepository;
            _mapper = mapper;
        }

        public async Task<List<ClaimActivityObservationServiceObject>> CreateBulkClaimActivityObservationAsync(List<ClaimActivityObservationServiceObject> claimActivityObservationServiceObjectList, CancellationToken token)
        {
            var claimActivityObservationEntityList = _mapper.Map<List<ClaimActivityObservationEntity>>(claimActivityObservationServiceObjectList);
            claimActivityObservationEntityList = await _claimActivityObservationRepository.CreateBulkClaimActivityObservationAsync(claimActivityObservationEntityList, token);
            return _mapper.Map<List<ClaimActivityObservationServiceObject>>(claimActivityObservationEntityList);
        }
        public async Task<List<ClaimActivityObservationServiceObject>> BulkCreateClaimActivityObservationFromDataTable(DataTable calimActivityObservationDataTable, ClaimBasketServiceObject ClaimBasket, CancellationToken token)
        {
            List<ClaimActivityObservationServiceObject> claimObservationServiceObjectsList = await Task.Run(() => (from row in calimActivityObservationDataTable.AsEnumerable()
                                                                                                                   select new ClaimActivityObservationServiceObject
                                                                                                                   {
                                                                                                                       XMLActivityTagID = Convert.ToInt32(row["Activity_Id"]),
                                                                                                                       Type = Convert.ToString(row["Type"]),
                                                                                                                       FileStatus = Convert.ToString(row["Type"]).ToLower() == "file" && row.Table.Columns["Value"] != null && Convert.ToString(row["Value"]).Length > 10 ? 2 : Convert.ToString(row["Type"]).ToLower() == "file" ? 1 : 0,// Status 2 means both  type = file and file (Value) exisits . 1 means  type = file exisits
                                                                                                                       Code = row.Table.Columns["Code"] != null ? Convert.ToString(row["Code"]) : "",
                                                                                                                       Value = Convert.ToString(row["Type"]).ToLower() != "file" && row.Table.Columns["Value"] != null ? Convert.ToString(row["Value"]).Length > 199 ? Convert.ToString(row["Value"]).Substring(0, 198) : Convert.ToString(row["Value"]) : "",
                                                                                                                       ValueType = row.Table.Columns["ValueType"] != null ? Convert.ToString(row["ValueType"]).Length > 99 ? Convert.ToString(row["ValueType"]).Substring(0, 98) : Convert.ToString(row["ValueType"]) : "",
                                                                                                                       ClaimBasketID = ClaimBasket.ClaimBasketID,
                                                                                                                   }).ToList());
            await Task.Run(() =>
            {
                for (int i = 0; i < claimObservationServiceObjectsList.Count; i++)
                {
                    var observtion = claimObservationServiceObjectsList.Skip(i).Take(1).FirstOrDefault();
                    if (!string.IsNullOrEmpty(observtion.Code) && !string.IsNullOrEmpty(observtion.Type))
                        observtion.Code = observtion.Type.Trim().ToLower() == "text" && observtion.Code.Length > 18 ? observtion.Code.Trim().EncryptCode() : observtion.Code.EncryptCode();
                    else if (!string.IsNullOrEmpty(observtion.Code))
                        observtion.Code = observtion.Code.EncryptCode();
                }
            });
            await CreateBulkClaimActivityObservationAsync(claimObservationServiceObjectsList, token);
            return claimObservationServiceObjectsList;
        }
    }
}
