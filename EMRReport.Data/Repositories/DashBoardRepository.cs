using EMRReport.Common.ProjectEnums;
using EMRReport.Data;
using EMRReport.DataContracts.IRepositories;
using EMRReport.DataContracts.NotMappedEntities;
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
    public sealed class DashBoardRepository : IDashBoardRepository
    {
        private int userID = 0;
        private string connectionString = "";
        public DashBoardRepository(ScrubberDbContext dbContext, IConfiguration configuration)
        {
            userID = 1;
            connectionString = configuration.GetValue<string>("ConnectionStrings:ScrubberDBConnectionString");
        }
        private async Task<DataTable> ExecuteQueryFromStoreProcedure(string SpName, DateTime? dateFrom, DateTime? dateTo, int type)
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
                    cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
                    cmd.Parameters.AddWithValue("@dateTo", dateTo);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@UserID", DBNull.Value);
                    cmd.Parameters.AddWithValue("@RoleID", DBNull.Value);
                    sda.SelectCommand = cmd;
                    await Task.Run(() => sda.Fill(data));
                }
            }

            return data;
        }
        public async Task<List<DashBoardNMEntity>> GetEncounterWiseAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token)
        {
            DataTable encounterDt = await ExecuteQueryFromStoreProcedure("spEncounterWiseErrors", dateFrom, dateTo, type);
            return await Task.Run(() => (from DataRow dr in encounterDt.Rows
                                         select new DashBoardNMEntity()
                                         {
                                             SenderID = dr["SenderID"].ToString(),
                                             EncounterType = Convert.ToInt32(dr["EncounterType"]),
                                             ClaimCountHavingErrors = Convert.ToInt32(dr["ClaimCountHavingErrors"]),
                                             ClaimCountWithOutErrors = Convert.ToInt32(dr["ClaimCountWithOutErrors"]),
                                             TotalClaimCount = Convert.ToInt32(dr["ClaimCountHavingErrors"]) + Convert.ToInt32(dr["ClaimCountWithOutErrors"]),
                                             ClaimAmountHavingErrors = Convert.ToDecimal(dr["ClaimAmountHavingErrors"]),
                                             ClaimAmountWithOutErrors = Convert.ToDecimal(dr["ClaimAmountWithOutErrors"]),
                                             TotalClaimAmount = Convert.ToDecimal(dr["ClaimAmountHavingErrors"]) + Convert.ToDecimal(dr["ClaimAmountWithOutErrors"])
                                         }).ToList());
        }

        public async Task<List<DashBoardNMEntity>> GeErrorCategoryWiseAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token)
        {
            DataTable categoryDt = await ExecuteQueryFromStoreProcedure("spErrorCategory", dateFrom, dateTo, type);
            return await Task.Run(() => (from DataRow dr in categoryDt.Rows
                                         select new DashBoardNMEntity()
                                         {
                                             SenderID = dr["SenderID"].ToString(),
                                             ScrubberErrorCategory = ((ScrubberErrorCategoryEnum)Convert.ToInt32(dr["ScrubberErrorCategory"])).ToString(),
                                             ScrubberPrefixType = ((ScrubberRulePrefixTypeEnum)Convert.ToInt32(dr["ScrubberPrefixType"])).ToString(),
                                             ErrorCount = Convert.ToInt32(dr["ErrorCount"]),
                                         }).ToList());
        }

        public async Task<List<DashBoardNMEntity>> GeErrorSummaryWiseAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token)
        {
            DataTable summaryDt = await ExecuteQueryFromStoreProcedure("spErrorSummary", dateFrom, dateTo, type);
            return await Task.Run(() => (from DataRow dr in summaryDt.Rows
                                         select new DashBoardNMEntity()
                                         {
                                             ValidateFiles = Convert.ToDecimal(dr["ValidateFiles"]),
                                             NonValidatedFiles = Convert.ToDecimal(dr["NonValidatedFiles"]),
                                             TotalUploadedFiles = Convert.ToDecimal(dr["ValidateFiles"]) + Convert.ToDecimal(dr["NonValidatedFiles"]),
                                             ClaimErrorCount = Convert.ToInt32(dr["ClaimErrorCount"]),
                                             ClaimWithOutErrorCount = Convert.ToInt32(dr["ClaimWithOutErrorCount"]),
                                             TotalClaimCount = Convert.ToInt32(dr["ClaimErrorCount"]) + Convert.ToInt32(dr["ClaimWithOutErrorCount"]),
                                             ClaimErrorNet = Convert.ToDecimal(dr["ClaimErrorNet"]),
                                             ClaimWithOutErrorNet = Convert.ToDecimal(dr["ClaimWithOutErrorNet"]),
                                             TotalClaimNET = Convert.ToDecimal(dr["ClaimErrorNet"]) + Convert.ToDecimal(dr["ClaimWithOutErrorNet"])
                                         }).ToList());
        }
        public async Task<List<DashBoardNMEntity>> GeClaimCounterAsync(DateTime? dateFrom, DateTime? dateTo, int type, CancellationToken token)
        {
            DataTable claimCounterDt = await ExecuteQueryFromStoreProcedure("spClaimCounter", dateFrom, dateTo, type);
            return await Task.Run(() => (from DataRow dr in claimCounterDt.Rows
                                         select new DashBoardNMEntity()
                                         {
                                             SenderID = dr["SenderID"].ToString(),
                                             ClaimCount = Convert.ToInt32(dr["ClaimCount"]),
                                             DateField = dr["Type"].ToString()
                                         }).ToList());
        }

    }
}
