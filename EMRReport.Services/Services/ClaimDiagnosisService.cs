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
    public sealed class ClaimDiagnosisService : IClaimDiagnosisService
    {
        private readonly IMapper _mapper;
        private readonly IClaimDiagnosisRepository _claimDiagnosisRepository;
        public ClaimDiagnosisService(IClaimDiagnosisRepository claimDiagnosisRepository, IMapper mapper)
        {
            _claimDiagnosisRepository = claimDiagnosisRepository;
            _mapper = mapper;
        }
        public async Task<List<ClaimDiagnosisServiceObject>> CreateBulkClaimDiagnosisAsync(List<ClaimDiagnosisServiceObject> claimDiagnosisServiceObjectList, CancellationToken token)
        {
            var claimDiagnosisEntityList = _mapper.Map<List<ClaimDiagnosisEntity>>(claimDiagnosisServiceObjectList);
            claimDiagnosisEntityList = await _claimDiagnosisRepository.CreateBulkClaimDiagnosisAsync(claimDiagnosisEntityList, token);
            return _mapper.Map<List<ClaimDiagnosisServiceObject>>(claimDiagnosisEntityList);
        }
        public async Task<List<ClaimDiagnosisServiceObject>> BulkCreateClaimDaignosisFromDataTable(DataTable claimdiagnosisDatatable, ClaimBasketServiceObject ClaimBasket, CancellationToken token)
        {

            var claimDiagnosisServiceObjectList = await Task.Run(() => (from row in claimdiagnosisDatatable.AsEnumerable()
                                                                        select new ClaimDiagnosisServiceObject
                                                                        {
                                                                            DiagnosisCode = Convert.ToString(row["Code"]),
                                                                            XMLClaimTagID = Convert.ToInt32(row["Claim_Id"]),
                                                                            Type = Convert.ToString(row["Type"]),
                                                                            IsPrimary = Convert.ToString(row["Type"]) == "Principal" ? true : false,
                                                                            ReasonForVisit = Convert.ToString(row["Type"]) == "ReasonForVisit" ? true : false,
                                                                            ClaimBasketID = ClaimBasket.ClaimBasketID
                                                                        }).ToList());
            await Task.Run(() =>
            {
                for (int i = 0; i < claimDiagnosisServiceObjectList.Count; i++)
                {
                    var diagnosis = claimDiagnosisServiceObjectList.Skip(i).Take(1).FirstOrDefault();
                    diagnosis.NMDiagnosisCode = diagnosis.DiagnosisCode;
                    diagnosis.DiagnosisCode = diagnosis.DiagnosisCode.EncryptCode();
                }
            });
            await CreateBulkClaimDiagnosisAsync(claimDiagnosisServiceObjectList, token);
            return claimDiagnosisServiceObjectList;
        }
    }
}
