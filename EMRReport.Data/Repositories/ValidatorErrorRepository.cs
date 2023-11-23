using EFCore.BulkExtensions;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class ValidatorErrorRepository : IValidatorErrorRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        private string connectionString = "";
        public ValidatorErrorRepository(ScrubberDbContext dbContext, IConfiguration configuration)
        {
            userID = 1;
            _dbContext = dbContext;
            connectionString = configuration.GetValue<string>("ConnectionStrings:ScrubberDBConnectionString");
        }
        private async Task<DataTable> ExecuteQueryFromStoreProcedure(string SpName, string CaseNumber, string EncryptedCPTS, string EncryptedICDS, int DiagnosisCount = -1, int SPType = -1)
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
                    cmd.Parameters.AddWithValue("@CaseNumber", CaseNumber);
                    if (!string.IsNullOrEmpty(EncryptedCPTS))
                        cmd.Parameters.AddWithValue("@CPTCodes", EncryptedCPTS);
                    if (DiagnosisCount != -1)
                        cmd.Parameters.AddWithValue("@DiagnosisCount", DiagnosisCount);
                    if (!string.IsNullOrEmpty(EncryptedICDS))
                        cmd.Parameters.AddWithValue("@DiagnosisCodes", EncryptedICDS);
                    else if (SPType == 3)
                        cmd.Parameters.AddWithValue("@DiagnosisCodes", DBNull.Value);
                    sda.SelectCommand = cmd;
                    await Task.Run(() => sda.Fill(data));
                }
            }

            return data;
        }
        private async Task<DataTable> ExecuteAPPQueryFromStoreProcedure(string SpName, string CaseNumber, string EncryptedCPTS, string EncryptedICDS, int RuleVersion, int AuthorityType, string Classification, int DiagnosisCount = -1, int SPType = -1)
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
                    cmd.Parameters.AddWithValue("@CaseNumber", CaseNumber);
                    if (!string.IsNullOrEmpty(EncryptedCPTS))
                        cmd.Parameters.AddWithValue("@CPTCodes", EncryptedCPTS);
                    if (DiagnosisCount != -1)
                        cmd.Parameters.AddWithValue("@DiagnosisCount", DiagnosisCount);
                    if (!string.IsNullOrEmpty(EncryptedICDS))
                        cmd.Parameters.AddWithValue("@DiagnosisCodes", EncryptedICDS);
                    else if (SPType == 3)
                        cmd.Parameters.AddWithValue("@DiagnosisCodes", DBNull.Value);
                    cmd.Parameters.AddWithValue("@RuleVersion", RuleVersion);
                    cmd.Parameters.AddWithValue("@AuthorityType", AuthorityType);
                    cmd.Parameters.AddWithValue("@Classification", Classification);
                    sda.SelectCommand = cmd;
                    await Task.Run(() => sda.Fill(data));
                }
            }

            return data;
        }
        private async Task<DataTable> ExecuteDetaildQueryFromStoreProcedure(string SpName, int ValidatorTransactionID, int RuleVersion, int AuthorityType, string Classification)
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
                    cmd.Parameters.AddWithValue("@ValidatorTransactionID", ValidatorTransactionID);
                    cmd.Parameters.AddWithValue("@RuleVersion", RuleVersion);
                    cmd.Parameters.AddWithValue("@AuthorityType", AuthorityType);
                    cmd.Parameters.AddWithValue("@Classification", Classification);
                    sda.SelectCommand = cmd;
                    await Task.Run(() => sda.Fill(data));
                }
            }

            return data;
        }
        public async Task<List<ValidatorErrorEntity>> GetValidatorActivityErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, DateTime date, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(EncryptedCPTS))
            {
                int DiagnosisCount = !string.IsNullOrEmpty(EncryptedICDS) ? EncryptedICDS.Split(',').Count() : 0;
                DataTable dtErrorCPT = await ExecuteQueryFromStoreProcedure("CPTValidatorUpdated", CaseNumber, EncryptedCPTS, null, DiagnosisCount, -1);
                return await Task.Run(() => (from DataRow dr in dtErrorCPT.Rows
                                             select new ValidatorErrorEntity()
                                             {
                                                 CaseNumber = CaseNumber,
                                                 ValidatedBy = userID,
                                                 ErrorCode1 = dr["RuleActivity"].ToString().Length > 400 ? dr["RuleActivity"].ToString().Substring(0, 399) + ".." : dr["RuleActivity"].ToString(),
                                                 ErrorCode2 = dr["TransActivity"].ToString().Length > 400 ? dr["TransActivity"].ToString().Substring(0, 399) + ".." : dr["TransActivity"].ToString(),
                                                 Message = dr["Message"].ToString(),
                                                 CodingTips = dr["CodingTips"].ToString(),
                                                 ValidatedDate = date,
                                                 Status = true
                                             }).ToList());
            }
            else
                return new List<ValidatorErrorEntity>();
        }
        public async Task<List<ValidatorErrorEntity>> GetAPPValidatorActivityErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, string Classification, DateTime date, CancellationToken token, int RuleVersion, int AuthorityType)
        {
            if (!string.IsNullOrEmpty(EncryptedCPTS))
            {
                int DiagnosisCount = !string.IsNullOrEmpty(EncryptedICDS) ? EncryptedICDS.Split(',').Count() : 0;
                DataTable dtErrorCPT = await ExecuteAPPQueryFromStoreProcedure("VerAuh_CPTValidatorUpdated", CaseNumber, EncryptedCPTS, null, RuleVersion, AuthorityType, Classification, DiagnosisCount, -1);
                return await Task.Run(() => (from DataRow dr in dtErrorCPT.Rows
                                             select new ValidatorErrorEntity()
                                             {
                                                 CaseNumber = CaseNumber,
                                                 ValidatedBy = userID,
                                                 ErrorCode1 = dr["RuleActivity"].ToString().Length > 400 ? dr["RuleActivity"].ToString().Substring(0, 399) + ".." : dr["RuleActivity"].ToString(),
                                                 ErrorCode2 = dr["TransActivity"].ToString().Length > 400 ? dr["TransActivity"].ToString().Substring(0, 399) + ".." : dr["TransActivity"].ToString(),
                                                 Message = dr["Message"].ToString(),
                                                 CodingTips = dr["CodingTips"].ToString(),
                                                 ValidatedDate = date,
                                                 Status = true
                                             }).ToList());
            }
            else
                return new List<ValidatorErrorEntity>();
        }

        public async Task<List<ValidatorErrorEntity>> CreateBulkValidatorErrorAsync(List<ValidatorErrorEntity> validatorErrorEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(validatorErrorEntityList);
            return validatorErrorEntityList;
        }
        public async Task<List<ValidatorErrorEntity>> GetValidatorICDErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, DateTime date, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(EncryptedICDS))
            {
                DataTable dtErrorICD = await ExecuteQueryFromStoreProcedure("ICDValidatorUpdated", CaseNumber, null, EncryptedICDS, -1, -1);
                return await Task.Run(() => (from DataRow dr in dtErrorICD.Rows
                                             select new ValidatorErrorEntity()
                                             {
                                                 CaseNumber = CaseNumber,
                                                 ValidatedBy = userID,
                                                 ErrorCode1 = dr["RuleICD"].ToString().Length > 400 ? dr["RuleICD"].ToString().Substring(0, 399) + ".." : dr["RuleICD"].ToString(),
                                                 ErrorCode2 = dr["TransICD"].ToString().Length > 400 ? dr["TransICD"].ToString().Substring(0, 399) + ".." : dr["TransICD"].ToString(),
                                                 Message = dr["Message"].ToString(),
                                                 CodingTips = dr["CodingTips"].ToString(),
                                                 ValidatedDate = date,
                                                 Status = true
                                             }).ToList());
            }
            else
                return new List<ValidatorErrorEntity>();
        }
        public async Task<List<ValidatorErrorEntity>> GetAPPValidatorICDErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, string Classification, DateTime date, CancellationToken token, int RuleVersion, int AuthorityType)
        {
            if (!string.IsNullOrEmpty(EncryptedICDS))
            {
                DataTable dtErrorICD = await ExecuteAPPQueryFromStoreProcedure("VerAuh_ICDValidatorUpdated", CaseNumber, null, EncryptedICDS, RuleVersion, AuthorityType, Classification, -1, -1);
                return await Task.Run(() => (from DataRow dr in dtErrorICD.Rows
                                             select new ValidatorErrorEntity()
                                             {
                                                 CaseNumber = CaseNumber,
                                                 ValidatedBy = userID,
                                                 ErrorCode1 = dr["RuleICD"].ToString().Length > 400 ? dr["RuleICD"].ToString().Substring(0, 399) + ".." : dr["RuleICD"].ToString(),
                                                 ErrorCode2 = dr["TransICD"].ToString().Length > 400 ? dr["TransICD"].ToString().Substring(0, 399) + ".." : dr["TransICD"].ToString(),
                                                 Message = dr["Message"].ToString(),
                                                 CodingTips = dr["CodingTips"].ToString(),
                                                 ValidatedDate = date,
                                                 Status = true
                                             }).ToList());
            }
            else
                return new List<ValidatorErrorEntity>();
        }
        public async Task<List<ValidatorErrorEntity>> GetValidatorICDCPTErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, DateTime date, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(EncryptedCPTS) || !string.IsNullOrEmpty(EncryptedCPTS) && !string.IsNullOrEmpty(EncryptedICDS))
            {
                DataTable dtErrorICDCPT = await ExecuteQueryFromStoreProcedure("ICDCPTValidator", CaseNumber, EncryptedCPTS, EncryptedICDS, -1, 3);
                return await Task.Run(() => (from DataRow dr in dtErrorICDCPT.Rows
                                             select new ValidatorErrorEntity()
                                             {
                                                 CaseNumber = CaseNumber,
                                                 ValidatedBy = userID,
                                                 ErrorCode1 = dr["RuleCPT"].ToString().Length > 400 ? dr["RuleCPT"].ToString().Substring(0, 399) + ".." : dr["RuleCPT"].ToString(),
                                                 ErrorCode2 = dr["TransICD"].ToString().Length > 400 ? dr["TransICD"].ToString().Substring(0, 399) + ".." : dr["TransICD"].ToString(),
                                                 Message = dr["Message"].ToString(),
                                                 CodingTips = dr["CodingTips"].ToString(),
                                                 ValidatedDate = date,
                                                 PrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                                 Status = true
                                             }).ToList());
            }
            else
                return new List<ValidatorErrorEntity>();
        }
        public async Task<List<ValidatorErrorEntity>> GetAPPValidatorICDCPTErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, string Classification, DateTime date, CancellationToken token, int RuleVersion, int AuthorityType)
        {
            if (!string.IsNullOrEmpty(EncryptedCPTS) || !string.IsNullOrEmpty(EncryptedCPTS) && !string.IsNullOrEmpty(EncryptedICDS))
            {
                DataTable dtErrorICDCPT = await ExecuteAPPQueryFromStoreProcedure("VerAuh_ICDCPTValidator", CaseNumber, EncryptedCPTS, EncryptedICDS, RuleVersion, AuthorityType, Classification, -1, 3);
                return await Task.Run(() => (from DataRow dr in dtErrorICDCPT.Rows
                                             select new ValidatorErrorEntity()
                                             {
                                                 CaseNumber = CaseNumber,
                                                 ValidatedBy = userID,
                                                 ErrorCode1 = dr["RuleCPT"].ToString().Length > 400 ? dr["RuleCPT"].ToString().Substring(0, 399) + ".." : dr["RuleCPT"].ToString(),
                                                 ErrorCode2 = dr["TransICD"].ToString().Length > 400 ? dr["TransICD"].ToString().Substring(0, 399) + ".." : dr["TransICD"].ToString(),
                                                 Message = dr["Message"].ToString(),
                                                 CodingTips = dr["CodingTips"].ToString(),
                                                 ValidatedDate = date,
                                                 PrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                                 Status = true
                                             }).ToList());
            }
            else
                return new List<ValidatorErrorEntity>();
        }
        public async Task<List<ValidatorErrorEntity>> GetDetailValidateActivityErrorsAsync(int ValidatorTransactionID, int RuleVersion, int AuthorityType, string Classification, DateTime date, CancellationToken token)
        {
            DataTable dtErrorCPT = await ExecuteDetaildQueryFromStoreProcedure("SPValidatorActivityRules", ValidatorTransactionID, RuleVersion, AuthorityType, Classification);
            return await Task.Run(() => (from DataRow dr in dtErrorCPT.Rows
                                         select new ValidatorErrorEntity()
                                         {
                                             ValidatedBy = userID,
                                             PrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                             ErrorCategory = Convert.ToInt32(dr["ScrubberErrorCategory"]),
                                             ErrorCode1 = dr["RuleActivity"].ToString(),
                                             ErrorCode2 = dr["TransActivity"].ToString(),
                                             Message = dr["Message"].ToString(),
                                             ValidatedDate = date,
                                             CodingTips = dr["CodingTips"].ToString(),
                                             ValidatorTransactionID = ValidatorTransactionID,
                                             Status = true,

                                         }).ToList());

        }
        public async Task<List<ValidatorErrorEntity>> GetDetailValidateICDErrorsAsync(int ValidatorTransactionID, int RuleVersion, int AuthorityType, string Classification, DateTime date, CancellationToken token)
        {
            DataTable dtErrorICD = await ExecuteDetaildQueryFromStoreProcedure("SPValidatorICDRules", ValidatorTransactionID, RuleVersion, AuthorityType, Classification);
            return await Task.Run(() => (from DataRow dr in dtErrorICD.Rows
                                         select new ValidatorErrorEntity()
                                         {
                                             ValidatedBy = userID,
                                             PrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                             ErrorCategory = Convert.ToInt32(dr["ScrubberErrorCategory"]),
                                             ErrorCode1 = dr["RuleICD"].ToString(),
                                             ErrorCode2 = dr["TransICD"].ToString(),
                                             Message = dr["Message"].ToString(),
                                             ValidatedDate = date,
                                             CodingTips = dr["CodingTips"].ToString(),
                                             ValidatorTransactionID = ValidatorTransactionID,
                                             Status = true
                                         }).ToList());

        }
        public async Task<List<ValidatorErrorEntity>> GetDetailValidateICDCPTErrorsAsync(int ValidatorTransactionID, int RuleVersion, int AuthorityType, string Classification, DateTime date, CancellationToken token)
        {
            DataTable dtErrorICDCPT = await ExecuteDetaildQueryFromStoreProcedure("SPValidatorICDCPTRules", ValidatorTransactionID, RuleVersion, AuthorityType, Classification);
            return await Task.Run(() => (from DataRow dr in dtErrorICDCPT.Rows
                                         select new ValidatorErrorEntity()
                                         {
                                             ValidatedBy = userID,
                                             PrefixType = Convert.ToInt32(dr["ScrubberPrefixType"]),
                                             ErrorCategory = Convert.ToInt32(dr["ScrubberErrorCategory"]),
                                             ErrorCode1 = dr["RuleCPT"].ToString(),
                                             ErrorCode2 = dr["TransICD"].ToString(),
                                             Message = dr["Message"].ToString(),
                                             CodingTips = dr["CodingTips"].ToString(),
                                             ValidatedDate = date,
                                             ValidatorTransactionID = ValidatorTransactionID,
                                             Status = true
                                         }).ToList());

        }


    }
}
