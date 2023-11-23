using EFCore.BulkExtensions;
using EMRReport.Data;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class ScrubberErrorRepository : IScrubberErrorRepository
    {

        private readonly ScrubberDbContext _dbContext;
        private int userID;
        private int roleID;
        private int clinicID;
        private string connectionString;
        public ScrubberErrorRepository(ScrubberDbContext dbContext, IConfiguration configuration)
        {
            userID = 1;
            roleID = 1;
            clinicID = 1;
            _dbContext = dbContext;
            connectionString = configuration.GetValue<string>("ConnectionStrings:ScrubberDBConnectionString");
        }
        public async Task<List<ScrubberErrorEntity>> CreateBulkScrubberErrorAsync(List<ScrubberErrorEntity> scrubberErrorEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(scrubberErrorEntityList);
            return scrubberErrorEntityList;
        }
        private async Task<DataTable> ExecuteQueryFromStoreProcedure(string SpName, int scrubClaimBasketID, int facilityID, int? payerReceiverID = null, int? clinicID = null)
        {
            SqlCommand cmd = new SqlCommand(SpName);
            DataTable data = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;//300
                    cmd.Parameters.AddWithValue("@ScrubClaimBasketID", scrubClaimBasketID);
                    cmd.Parameters.AddWithValue("@FacilityID", facilityID);
                    if (payerReceiverID.HasValue)
                        cmd.Parameters.AddWithValue("@PayerReceiverID", payerReceiverID.Value);
                    if (clinicID.HasValue)
                        cmd.Parameters.AddWithValue("@ClinicID", clinicID.Value);
                    sda.SelectCommand = cmd;
                    await Task.Run(() => sda.Fill(data));
                }
            }
            return data;
        }
        public async Task<List<ScrubberErrorEntity>> GetScrubberReportByBasketGroupIdAync(int basketGroupID, bool IsDetail, CancellationToken token)
        {
            return await _dbContext.ScrubberError.Where(x => IsDetail ? true : x.IsHit).Join(_dbContext.ClaimBasket.Where(x => x.BasketGroupID == basketGroupID),
                e => e.ClaimBasketID, b => b.ClaimBasketID, (e, b) => new { e, b }).Select(x => x.e).ToListAsync();
        }
        public async Task<List<ScrubberErrorEntity>> GetScrubberErrorsByBasketGroupIdAync(int basketGroupID, int paage, CancellationToken token)
        {
            paage = paage - 1;
            return await _dbContext.ScrubberError.Join(_dbContext.ClaimBasket.Where(x => x.BasketGroupID == basketGroupID),
                e => e.ClaimBasketID, b => b.ClaimBasketID, (e, b) => new { e, b }).Select(x => x.e).Skip(paage * 50).Take(50).ToListAsync();
        }
        public async Task<Tuple<int, List<ScrubberErrorEntity>>> GetScrubberErrorsByBasketGroupIdAndTotalAync(int basketGroupID, int paage, CancellationToken token)
        {
            paage = paage - 1;
            int Total = await _dbContext.ScrubberError.Join(_dbContext.ClaimBasket.Where(x => x.BasketGroupID == basketGroupID),
                e => e.ClaimBasketID, b => b.ClaimBasketID, (e, b) => new { e, b }).Select(x => x.e).CountAsync();

            var scrubberErrorEntityList = await _dbContext.ScrubberError.Join(_dbContext.ClaimBasket.Where(x => x.BasketGroupID == basketGroupID),
                e => e.ClaimBasketID, b => b.ClaimBasketID, (e, b) => new { e, b }).Select(x => x.e).Skip(paage * 50).Take(50).ToListAsync();
            return Tuple.Create(Total, scrubberErrorEntityList);
        }
        public async Task<List<ScrubberErrorEntity>> GetScrubberActivityErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token)
        {
            DataTable activityDataTable = await ExecuteQueryFromStoreProcedure("SPScrubberActivityRules", claimBasket.ClaimBasketID, claimBasket.FacilityID.Value, claimBasket.PayerReceiverID, clinicID);
            var scrubberErrorEntityList = await Task.Run(() => (from DataRow dr in activityDataTable.Rows
                                                                select new ScrubberErrorEntity()
                                                                {
                                                                    ClaimID = dr["ClaimID"].ToString(),
                                                                    ErrorCode1 = dr["RuleActivity"].ToString(),
                                                                    ErrorCode2 = dr["TransActivity"].ToString(),
                                                                    Message = dr["Message"].ToString(),
                                                                    CodingTips = dr["CodingTips"].ToString(),
                                                                    MemberID = dr["MemberID"].ToString(),
                                                                    EmiratesID = dr["EmiratesIDNumber"].ToString(),
                                                                    MRN = dr["PatientID"].ToString(),
                                                                    ClaimBasketID = claimBasket.ClaimBasketID,
                                                                    ErrorDate = claimBasket.CreatedDate,
                                                                    CreatedBy = claimBasket.CreatedBy.Value,
                                                                    Start = dr["EncounterStart"].ToString(),
                                                                    ActivityStart = dr["ActivityStart"].ToString(),
                                                                    ActivityID = dr["ActivityID"].ToString(),
                                                                    SenderID = claimBasket.SenderID,
                                                                    ReceiverID = claimBasket.ReceiverID,
                                                                    TransactionDate = claimBasket.TransactionDate,
                                                                    RecordCount = claimBasket.RecordCount,
                                                                    DispositionFlag = claimBasket.DispositionFlag,
                                                                    FileName = claimBasket.FileName,
                                                                    ErrorHit = dr["ErrorHit"].ToString(),
                                                                    RoleID = roleID,
                                                                    IsValidated = true,
                                                                    IsHit = true,
                                                                    XMLClaimTagID = Convert.ToInt32(dr["Claim_Id"]),
                                                                    ScrubberErrorCategory = Convert.ToInt32(dr["ScrubberErrorCategory"]),
                                                                    ScrubberPrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                                                    EncounterType = Convert.ToInt32(dr["EncounterType"]),
                                                                    EncounterStartType = dr["EncounterStartType"].ToString(),
                                                                    EncounterEndType = dr["EncounterEndType"].ToString(),
                                                                    EncounterEnd = dr["EncounterEnd"].ToString(),
                                                                    ClaimGross = Convert.ToDecimal(dr["ClaimGross"]),
                                                                    PatientShare = Convert.ToDecimal(dr["PatientShare"]),
                                                                    Gender = Convert.ToString(dr["Gender"]),
                                                                    Age = Convert.ToString(dr["Age"]),
                                                                    ClaimNet = Convert.ToDecimal(dr["ClaimNet"]),
                                                                    ServiceCode = dr["ServiceCode"].ToString(),
                                                                    Quantity = Convert.ToDecimal(dr["Quantity"]),
                                                                    Net = Convert.ToDecimal(dr["Net"]),
                                                                    VAT = Convert.ToDecimal(dr["VAT"]),
                                                                    ActivityType = Convert.ToInt32(dr["ActivityType"]),
                                                                    PriorAuthorizationID = dr["PriorAuthorizationID"].ToString(),
                                                                    OrderingClinician = dr["OrderingClinician"].ToString(),
                                                                    ActivityClinician = dr["ActivityClinician"].ToString(),
                                                                    CPTS = dr["CPTS"].ToString(),
                                                                    PrimaryICD = dr["PrimaryICD"].ToString(),
                                                                    SecondaryICDS = dr["SecondaryICDS"].ToString(),
                                                                    ReasonForVisitICDS = dr["ReasonForVisitICDS"].ToString()
                                                                }).ToList());
            return scrubberErrorEntityList;
        }

        public async Task<List<ScrubberErrorEntity>> GetScrubberICDErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token)
        {
            DataTable iCDDataTable = await ExecuteQueryFromStoreProcedure("SPScrubberICDRules", claimBasket.ClaimBasketID, claimBasket.FacilityID.Value, claimBasket.PayerReceiverID, clinicID);
            var scrubberErrorEntityList = await Task.Run(() => (from DataRow dr in iCDDataTable.Rows
                                                                select new ScrubberErrorEntity()
                                                                {
                                                                    ClaimID = dr["ClaimID"].ToString(),
                                                                    ErrorCode1 = dr["RuleICD"].ToString(),
                                                                    ErrorCode2 = dr["TransICD"].ToString(),
                                                                    Message = dr["Message"].ToString(),
                                                                    CodingTips = dr["CodingTips"].ToString(),
                                                                    MemberID = dr["MemberID"].ToString(),
                                                                    EmiratesID = dr["EmiratesIDNumber"].ToString(),
                                                                    MRN = dr["PatientID"].ToString(),
                                                                    ClaimBasketID = claimBasket.ClaimBasketID,
                                                                    ErrorDate = claimBasket.CreatedDate,
                                                                    CreatedBy = claimBasket.CreatedBy.Value,
                                                                    Start = dr["EncounterStart"].ToString(),
                                                                    SenderID = claimBasket.SenderID,
                                                                    ReceiverID = claimBasket.ReceiverID,
                                                                    TransactionDate = claimBasket.TransactionDate,
                                                                    RecordCount = claimBasket.RecordCount,
                                                                    DispositionFlag = claimBasket.DispositionFlag,
                                                                    FileName = claimBasket.FileName,
                                                                    ErrorHit = dr["ErrorHit"].ToString(),
                                                                    RoleID = roleID,
                                                                    IsValidated = true,
                                                                    IsHit = true,
                                                                    XMLClaimTagID = Convert.ToInt32(dr["Claim_Id"]),
                                                                    ScrubberErrorCategory = Convert.ToInt32(dr["ScrubberErrorCategory"]),
                                                                    ScrubberPrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                                                    EncounterType = Convert.ToInt32(dr["EncounterType"]),
                                                                    EncounterStartType = dr["EncounterStartType"].ToString(),
                                                                    EncounterEndType = dr["EncounterEndType"].ToString(),
                                                                    EncounterEnd = dr["EncounterEnd"].ToString(),
                                                                    ClaimGross = Convert.ToDecimal(dr["ClaimGross"]),
                                                                    PatientShare = Convert.ToDecimal(dr["PatientShare"]),
                                                                    Gender = Convert.ToString(dr["Gender"]),
                                                                    Age = Convert.ToString(dr["Age"]),
                                                                    ClaimNet = Convert.ToDecimal(dr["ClaimNet"]),
                                                                    ActivityID = dr["ActivityID"].ToString(),
                                                                    ServiceCode = dr["ServiceCode"].ToString(),
                                                                    ActivityStart = dr["ActivityStart"].ToString(),
                                                                    Quantity = Convert.ToDecimal(dr["Quantity"]),
                                                                    Net = Convert.ToDecimal(dr["Net"]),
                                                                    VAT = Convert.ToDecimal(dr["VAT"]),
                                                                    ActivityType = Convert.ToInt32(dr["ActivityType"]),
                                                                    PriorAuthorizationID = dr["PriorAuthorizationID"].ToString(),
                                                                    OrderingClinician = dr["OrderingClinician"].ToString(),
                                                                    ActivityClinician = dr["ActivityClinician"].ToString(),
                                                                    CPTS = dr["CPTS"].ToString(),
                                                                    PrimaryICD = dr["PrimaryICD"].ToString(),
                                                                    SecondaryICDS = dr["SecondaryICDS"].ToString(),
                                                                    ReasonForVisitICDS = dr["ReasonForVisitICDS"].ToString()
                                                                }).ToList());
            return scrubberErrorEntityList;
        }
        public async Task<List<ScrubberErrorEntity>> GetScrubberICDCPTErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token)
        {

            DataTable iCDCPTDataTable = await ExecuteQueryFromStoreProcedure("SPScrubberICDCPTRules", claimBasket.ClaimBasketID, claimBasket.FacilityID.Value, claimBasket.PayerReceiverID, clinicID);
            var scrubberErrorEntityList = await Task.Run(() => (from DataRow dr in iCDCPTDataTable.Rows
                                                                select new ScrubberErrorEntity()
                                                                {
                                                                    ClaimID = dr["ClaimID"].ToString(),
                                                                    ErrorCode1 = dr["RuleCPT"].ToString(),
                                                                    ErrorCode2 = dr["TransICD"].ToString(),
                                                                    Message = dr["Message"].ToString(),
                                                                    CodingTips = dr["CodingTips"].ToString(),
                                                                    MemberID = dr["MemberID"].ToString(),
                                                                    EmiratesID = dr["EmiratesIDNumber"].ToString(),
                                                                    MRN = dr["PatientID"].ToString(),
                                                                    ClaimBasketID = claimBasket.ClaimBasketID,
                                                                    ErrorDate = claimBasket.CreatedDate,
                                                                    CreatedBy = claimBasket.CreatedBy.Value,
                                                                    Start = dr["EncounterStart"].ToString(),
                                                                    ActivityStart = dr["ActivityStart"].ToString(),
                                                                    ActivityID = dr["ActivityID"].ToString(),
                                                                    SenderID = claimBasket.SenderID,
                                                                    ReceiverID = claimBasket.ReceiverID,
                                                                    TransactionDate = claimBasket.TransactionDate,
                                                                    RecordCount = claimBasket.RecordCount,
                                                                    DispositionFlag = claimBasket.DispositionFlag,
                                                                    FileName = claimBasket.FileName,
                                                                    ErrorHit = dr["ErrorHit"].ToString(),
                                                                    RoleID = roleID,
                                                                    IsValidated = true,
                                                                    IsHit = true,
                                                                    XMLClaimTagID = Convert.ToInt32(dr["Claim_Id"]),
                                                                    ScrubberErrorCategory = Convert.ToInt32(dr["ScrubberErrorCategory"]),
                                                                    ScrubberPrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                                                    EncounterType = Convert.ToInt32(dr["EncounterType"]),
                                                                    EncounterStartType = dr["EncounterStartType"].ToString(),
                                                                    EncounterEndType = dr["EncounterEndType"].ToString(),
                                                                    EncounterEnd = dr["EncounterEnd"].ToString(),
                                                                    ClaimGross = Convert.ToDecimal(dr["ClaimGross"]),
                                                                    PatientShare = Convert.ToDecimal(dr["PatientShare"]),
                                                                    Gender = Convert.ToString(dr["Gender"]),
                                                                    Age = Convert.ToString(dr["Age"]),
                                                                    ClaimNet = Convert.ToDecimal(dr["ClaimNet"]),
                                                                    ServiceCode = dr["ServiceCode"].ToString(),
                                                                    Quantity = Convert.ToDecimal(dr["Quantity"]),
                                                                    Net = Convert.ToDecimal(dr["Net"]),
                                                                    VAT = Convert.ToDecimal(dr["VAT"]),
                                                                    ActivityType = Convert.ToInt32(dr["ActivityType"]),
                                                                    PriorAuthorizationID = dr["PriorAuthorizationID"].ToString(),
                                                                    OrderingClinician = dr["OrderingClinician"].ToString(),
                                                                    ActivityClinician = dr["ActivityClinician"].ToString(),
                                                                    CPTS = dr["CPTS"].ToString(),
                                                                    PrimaryICD = dr["PrimaryICD"].ToString(),
                                                                    SecondaryICDS = dr["SecondaryICDS"].ToString(),
                                                                    ReasonForVisitICDS = dr["ReasonForVisitICDS"].ToString()
                                                                }).ToList());

            return scrubberErrorEntityList;
        }
        public async Task<List<ScrubberErrorEntity>> GetScrubberObservationErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token)
        {

            DataTable observationtDataTable = await ExecuteQueryFromStoreProcedure("SPScrubberActivityObservationRules", claimBasket.ClaimBasketID, claimBasket.FacilityID.Value, claimBasket.PayerReceiverID, clinicID);
            var scrubberErrorEntityList = await Task.Run(() => (from DataRow dr in observationtDataTable.Rows
                                                                select new ScrubberErrorEntity()
                                                                {
                                                                    ClaimID = dr["ClaimID"].ToString(),
                                                                    ErrorCode1 = dr["RuleActivity"].ToString(),
                                                                    ErrorCode2 = dr["TransActivity"].ToString(),
                                                                    Message = dr["Message"].ToString(),
                                                                    CodingTips = dr["CodingTips"].ToString(),
                                                                    MemberID = dr["MemberID"].ToString(),
                                                                    EmiratesID = dr["EmiratesIDNumber"].ToString(),
                                                                    MRN = dr["PatientID"].ToString(),
                                                                    ClaimBasketID = claimBasket.ClaimBasketID,
                                                                    ErrorDate = claimBasket.CreatedDate,
                                                                    Start = dr["EncounterStart"].ToString(),
                                                                    ActivityStart = dr["ActivityStart"].ToString(),
                                                                    ActivityID = dr["ActivityID"].ToString(),
                                                                    CreatedBy = claimBasket.CreatedBy.Value,
                                                                    SenderID = claimBasket.SenderID,
                                                                    ReceiverID = claimBasket.ReceiverID,
                                                                    TransactionDate = claimBasket.TransactionDate,
                                                                    RecordCount = claimBasket.RecordCount,
                                                                    DispositionFlag = claimBasket.DispositionFlag,
                                                                    FileName = claimBasket.FileName,
                                                                    ErrorHit = dr["ErrorHit"].ToString(),
                                                                    RoleID = roleID,
                                                                    IsValidated = true,
                                                                    IsHit = true,
                                                                    XMLClaimTagID = Convert.ToInt32(dr["Claim_Id"]),
                                                                    ScrubberErrorCategory = Convert.ToInt32(dr["ScrubberErrorCategory"]),
                                                                    ScrubberPrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                                                    EncounterType = Convert.ToInt32(dr["EncounterType"]),
                                                                    EncounterStartType = dr["EncounterStartType"].ToString(),
                                                                    EncounterEndType = dr["EncounterEndType"].ToString(),
                                                                    EncounterEnd = dr["EncounterEnd"].ToString(),
                                                                    ClaimGross = Convert.ToDecimal(dr["ClaimGross"]),
                                                                    PatientShare = Convert.ToDecimal(dr["PatientShare"]),
                                                                    Gender = Convert.ToString(dr["Gender"]),
                                                                    Age = Convert.ToString(dr["Age"]),
                                                                    ClaimNet = Convert.ToDecimal(dr["ClaimNet"]),
                                                                    ServiceCode = dr["ServiceCode"].ToString(),
                                                                    Quantity = Convert.ToDecimal(dr["Quantity"]),
                                                                    Net = Convert.ToDecimal(dr["Net"]),
                                                                    VAT = Convert.ToDecimal(dr["VAT"]),
                                                                    ActivityType = Convert.ToInt32(dr["ActivityType"]),
                                                                    PriorAuthorizationID = dr["PriorAuthorizationID"].ToString(),
                                                                    OrderingClinician = dr["OrderingClinician"].ToString(),
                                                                    ActivityClinician = dr["ActivityClinician"].ToString(),
                                                                    CPTS = dr["CPTS"].ToString(),
                                                                    PrimaryICD = dr["PrimaryICD"].ToString(),
                                                                    SecondaryICDS = dr["SecondaryICDS"].ToString(),
                                                                    ReasonForVisitICDS = dr["ReasonForVisitICDS"].ToString()
                                                                }).ToList());
            return scrubberErrorEntityList;
        }
        public async Task<List<ScrubberErrorEntity>> GetScrubberNonHitErrorsAsync(ClaimBasketEntity claimBasket, CancellationToken token)
        {
            DataTable nonHitTable = await ExecuteQueryFromStoreProcedure("SPScrubberClaimSupport", claimBasket.ClaimBasketID, claimBasket.FacilityID.Value);
            var scrubberErrorEntityList = await Task.Run(() => (from DataRow dr in nonHitTable.Rows
                                                                select new ScrubberErrorEntity()
                                                                {
                                                                    ErrorHit = dr["ErrorHit"].ToString(),
                                                                    IsHit = false,
                                                                    IsValidated = true,
                                                                    XMLClaimTagID = Convert.ToInt32(dr["Claim_Id"]),
                                                                    ScrubberErrorCategory = 0,
                                                                    ScrubberPrefixType = 0,
                                                                    ClaimID = dr["ClaimID"].ToString(),
                                                                    ErrorDate = claimBasket.CreatedDate,
                                                                    CreatedBy = claimBasket.CreatedBy.Value,
                                                                    RoleID = roleID,
                                                                    SenderID = claimBasket.SenderID,
                                                                    ReceiverID = claimBasket.ReceiverID,
                                                                    TransactionDate = claimBasket.TransactionDate,
                                                                    RecordCount = claimBasket.RecordCount,
                                                                    DispositionFlag = claimBasket.DispositionFlag,
                                                                    FileName = claimBasket.FileName,
                                                                    ErrorCode1 = dr["RuleActivity"].ToString(),
                                                                    ErrorCode2 = dr["TransActivity"].ToString(),
                                                                    Message = dr["Message"].ToString(),
                                                                    CodingTips = "",
                                                                    MemberID = dr["MemberID"].ToString(),
                                                                    EmiratesID = dr["EmiratesIDNumber"].ToString(),
                                                                    MRN = dr["PatientID"].ToString(),
                                                                    Start = dr["EncounterStart"].ToString(),
                                                                    EncounterEnd = dr["EncounterEnd"].ToString(),
                                                                    EncounterType = Convert.ToInt32(dr["EncounterType"]),
                                                                    EncounterStartType = dr["EncounterStartType"].ToString(),
                                                                    EncounterEndType = dr["EncounterEndType"].ToString(),
                                                                    ClaimGross = Convert.ToDecimal(dr["ClaimGross"]),
                                                                    PatientShare = Convert.ToDecimal(dr["PatientShare"]),
                                                                    Gender = Convert.ToString(dr["Gender"]),
                                                                    Age = Convert.ToString(dr["Age"]),
                                                                    ClaimNet = Convert.ToDecimal(dr["ClaimNet"]),
                                                                    ActivityID = dr["ActivityID"].ToString(),
                                                                    ServiceCode = dr["ServiceCode"].ToString(),
                                                                    ActivityStart = dr["ActivityStart"].ToString(),
                                                                    Quantity = Convert.ToDecimal(dr["Quantity"]),
                                                                    Net = Convert.ToDecimal(dr["Net"]),
                                                                    VAT = Convert.ToDecimal(dr["VAT"]),
                                                                    ActivityType = Convert.ToInt32(dr["ActivityType"]),
                                                                    PriorAuthorizationID = dr["PriorAuthorizationID"].ToString(),
                                                                    OrderingClinician = dr["OrderingClinician"].ToString(),
                                                                    ActivityClinician = dr["ActivityClinician"].ToString(),
                                                                    CPTS = dr["CPTS"].ToString(),
                                                                    PrimaryICD = dr["PrimaryICD"].ToString(),
                                                                    SecondaryICDS = dr["SecondaryICDS"].ToString(),
                                                                    ReasonForVisitICDS = dr["ReasonForVisitICDS"].ToString(),
                                                                    ClaimBasketID = claimBasket.ClaimBasketID
                                                                }).ToList());
            return scrubberErrorEntityList;
        }
    }
}
