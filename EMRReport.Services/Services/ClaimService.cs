using AutoMapper;
using EMRReport.Common.Extensions;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class ClaimService : IClaimService
    {
        private readonly IMapper _mapper;
        private readonly IClaimRepository _claimRepository;
        public ClaimService(IClaimRepository claimRepository, IMapper mapper)
        {
            _claimRepository = claimRepository;
            _mapper = mapper;
        }

        public async Task<List<ClaimServiceObject>> CreateBulkClaimAsync(List<ClaimServiceObject> claimServiceObjectList, CancellationToken token)
        {
            var claimEntityList = _mapper.Map<List<ClaimEntity>>(claimServiceObjectList);
            claimEntityList = await _claimRepository.CreateBulkClaimAsync(claimEntityList, token);
            return _mapper.Map<List<ClaimServiceObject>>(claimEntityList);
        }
        public async Task<List<ClaimServiceObject>> GetClaimFromDataTableAsync(DataTable claimDataTable, ClaimBasketServiceObject ClaimBasket, string cardPattern, CancellationToken token)
        {
            return await Task.Run(() => (from row in claimDataTable.AsEnumerable()
                                         select new ClaimServiceObject
                                         {
                                             ClaimID = Convert.ToString(row["ID"]),
                                             PayerID = Convert.ToString(row["PayerID"]),
                                             MemberID = row.Table.Columns["MemberID"] != null ? Convert.ToString(row["MemberID"]) : "",
                                             EmiratesIDNumber = row.Table.Columns["EmiratesIDNumber"] != null ? Convert.ToString(row["EmiratesIDNumber"]) : "",
                                             XMLClaimTagID = Convert.ToInt32(row["Claim_Id"]),
                                             ClaimGross = row.Table.Columns["Gross"] != null ? Convert.ToDecimal(row["Gross"]) : 0,
                                             ClaimNet = Convert.ToDecimal(row["Net"]),
                                             PatientShare = row.Table.Columns["PatientShare"] != null ? Convert.ToDecimal(row["PatientShare"]) : 0,
                                             ClaimBasketID = ClaimBasket.ClaimBasketID,
                                             MemberFormatID = row.Table.Columns["MemberID"] != null && !string.IsNullOrEmpty(Convert.ToString(row["MemberID"])) ? Regex.Match(Convert.ToString(row["MemberID"]), cardPattern).Groups[2].Value : "",
                                         }).ToList());
        }
        public async Task<List<ClaimServiceObject>> BulkCreateClaimWithClaimDetails(List<ClaimServiceObject> claimServiceObjectList, List<ClaimDiagnosisServiceObject> claimDiagnosisServiceObjectList, List<ClaimActivityServiceObject> claimActivityServiceObjectList, CancellationToken token)
        {
            int XMLClaimTagID = -1;
            StringBuilder sbCodes = new StringBuilder(10);
            await Task.Run(() =>
            {
                for (int i = 0; i < claimServiceObjectList.Count; i++)
                {
                    var claim = claimServiceObjectList.Skip(i).Take(1).FirstOrDefault();
                    XMLClaimTagID = claim.XMLClaimTagID;
                    claim.PrimaryICD = string.Join("; ", claimDiagnosisServiceObjectList.Where(x => x.XMLClaimTagID == XMLClaimTagID && x.IsPrimary).Select(x => x.NMDiagnosisCode));
                    claim.SecondaryICDS = string.Join("; ", claimDiagnosisServiceObjectList.Where(x => x.XMLClaimTagID == XMLClaimTagID && !x.IsPrimary && !x.ReasonForVisit).Select(x => x.NMDiagnosisCode)).LimitCommaSeperatedCodesTo4000Chars();
                    claim.ReasonForVisitICDS = string.Join("; ", claimDiagnosisServiceObjectList.Where(x => x.XMLClaimTagID == XMLClaimTagID && x.ReasonForVisit).Select(x => x.NMDiagnosisCode)).LimitCommaSeperatedCodesTo1000Chars();
                    claim.ServiceCodes = string.Join("; ", claimActivityServiceObjectList.Where(x => x.XMLClaimTagID == XMLClaimTagID).Select(x => x.NMServiceCode)).LimitCommaSeperatedCodesTo4000Chars();
                }
            });
            return await CreateBulkClaimAsync(claimServiceObjectList, token);
        }
    }
}
