using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class LogPayLoadServiceService : ILogPayLoadServiceService
    {

        private readonly ILogger<LogPayLoadServiceService> _logger;

        private readonly string _configuration;
        public LogPayLoadServiceService(ILogger<LogPayLoadServiceService> logger, IConfiguration configuration)
        {
            _configuration = configuration.GetConnectionString("ScrubberDBConnectionString");
            _logger = logger;
        }
        public async Task SavePayLoad(LogPayLoadServiceObject logPayLoadServiceObject)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_configuration))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("SPLogPayLog", sqlConnection))
                    {
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        if (!string.IsNullOrEmpty(logPayLoadServiceObject.ApiEndPoint))
                            sqlCommand.Parameters.Add(new SqlParameter("@apiEndPoints", logPayLoadServiceObject.ApiEndPoint));
                        else
                            sqlCommand.Parameters.Add(new SqlParameter("@apiEndPoints", DBNull.Value));
                        if (!string.IsNullOrEmpty(logPayLoadServiceObject.FunctionName))
                            sqlCommand.Parameters.Add(new SqlParameter("@funcName", logPayLoadServiceObject.FunctionName));
                        else
                            sqlCommand.Parameters.Add(new SqlParameter("@funcName", DBNull.Value));
                        if (!string.IsNullOrEmpty(logPayLoadServiceObject.RequestStatus))
                            sqlCommand.Parameters.Add(new SqlParameter("@requestStatus", logPayLoadServiceObject.RequestStatus));
                        else
                            sqlCommand.Parameters.Add(new SqlParameter("@requestStatus", DBNull.Value));
                        if (!string.IsNullOrEmpty(logPayLoadServiceObject.RequestData))
                            sqlCommand.Parameters.Add(new SqlParameter("@requestData", logPayLoadServiceObject.RequestData));
                        else
                            sqlCommand.Parameters.Add(new SqlParameter("@requestData", DBNull.Value));
                        sqlCommand.Parameters.Add(new SqlParameter("@RequestStartTime", logPayLoadServiceObject.RequestStartTime));
                        sqlCommand.Parameters.Add(new SqlParameter("@RequestCompletedTime", logPayLoadServiceObject.RequestCompletedTime));
                        if (!string.IsNullOrEmpty(logPayLoadServiceObject.RequestResponse))
                            sqlCommand.Parameters.Add(new SqlParameter("@requestResponse", logPayLoadServiceObject.RequestResponse));
                        else
                            sqlCommand.Parameters.Add(new SqlParameter("@requestResponse", DBNull.Value));
                        sqlCommand.Parameters.Add(new SqlParameter("@createdBy", logPayLoadServiceObject.CreatedBy));
                        if (!string.IsNullOrEmpty(logPayLoadServiceObject.ExceptionMessage))
                            sqlCommand.Parameters.Add(new SqlParameter("@exceptionMessage", logPayLoadServiceObject.ExceptionMessage));
                        else
                            sqlCommand.Parameters.Add(new SqlParameter("@exceptionMessage", DBNull.Value));
                        sqlConnection.Open();
                        await sqlCommand.ExecuteNonQueryAsync();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception e)
            {
                await Task.Run(() => _logger.LogError(e.Message, e));
            }
        }
    }
}
