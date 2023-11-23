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
    public sealed class ClaimEncounterService : IClaimEncounterService
    {
        private readonly IMapper _mapper;
        private readonly IClaimEncounterRepository _claimEncounterRepository;
        public ClaimEncounterService(IClaimEncounterRepository claimEncounterRepository, IMapper mapper)
        {
            _claimEncounterRepository = claimEncounterRepository;
            _mapper = mapper;
        }

        public async Task<List<ClaimEncounterServiceObject>> CreateBulkClaimEncounterAsync(List<ClaimEncounterServiceObject> claimEncounterServiceObjectList, CancellationToken token)
        {
            var claimEncounterEntityList = _mapper.Map<List<ClaimEncounterEntity>>(claimEncounterServiceObjectList);
            claimEncounterEntityList = await _claimEncounterRepository.CreateBulkClaimEncounterAsync(claimEncounterEntityList, token);
            return _mapper.Map<List<ClaimEncounterServiceObject>>(claimEncounterEntityList);
        }
        public async Task<List<ClaimEncounterServiceObject>> bulkCreateClaimEncounterFromDataTableAndValidateDOS(DataTable calimEncounterDataTable, ClaimBasketServiceObject ClaimBasket, bool IsFacilityDOS, DateTime? dateOfService, CancellationToken token)
        {
            List<ClaimEncounterServiceObject> claimEncounterServiceObjectList = await Task.Run(() => (from row in calimEncounterDataTable.AsEnumerable()
                                                                                                      select new ClaimEncounterServiceObject
                                                                                                      {
                                                                                                          EncounterType = Convert.ToInt32(row["Type"]),
                                                                                                          EncounterStartType = row.Table.Columns["StartType"] != null ? Convert.ToString(row["StartType"]) : "",
                                                                                                          EncounterEndType = row.Table.Columns["EndType"] != null ? Convert.ToString(row["EndType"]) : "",
                                                                                                          PatientID = Convert.ToString(row["PatientID"]),
                                                                                                          Start = Convert.ToString(row["Start"]),
                                                                                                          End = row.Table.Columns["End"] != null ? Convert.ToString(row["End"]) : "",
                                                                                                          XMLClaimTagID = Convert.ToInt32(row["Claim_Id"]),
                                                                                                          Gender = row.Table.Columns["Gender"] != null ? Convert.ToString(row["Gender"]) == "" ? null : Convert.ToString(row["Gender"]) : null,
                                                                                                          Age = row.Table.Columns["Age"] != null ? Convert.ToString(row["Age"]) == "" ? null : (int?)Convert.ToInt32(Regex.Replace(Convert.ToString(row["Age"]), "[^0-9]", "")) : null, //[^0-9.]
                                                                                                          Network = row.Table.Columns["Network"] != null ? Convert.ToString(row["Network"]) == "" ? null : Convert.ToString(row["Network"]) : null,
                                                                                                          PlanId = row.Table.Columns["PlanId"] != null ? Convert.ToString(row["PlanId"]) == "" ? null : Convert.ToString(row["PlanId"]) : null,
                                                                                                          ClaimBasketID = ClaimBasket.ClaimBasketID
                                                                                                      }).ToList());
            await Task.Run(() =>
            {
                for (int i = 0; i < claimEncounterServiceObjectList.Count; i++)
                {
                    var encounter = claimEncounterServiceObjectList.Skip(i).Take(1).FirstOrDefault();
                    encounter.StartDate = encounter.Start.ConvertStringToDateTime().Value;
                    encounter.EndDate = !string.IsNullOrEmpty(encounter.End) ? (DateTime?)encounter.End.ConvertStringToDateTime().Value : null;
                    encounter.IsDOS = IsFacilityDOS ? encounter.StartDate.CheckIsDOSValidation(dateOfService) : false;
                }
            });
            await CreateBulkClaimEncounterAsync(claimEncounterServiceObjectList, token);
            return claimEncounterServiceObjectList;
        }
    }
}
