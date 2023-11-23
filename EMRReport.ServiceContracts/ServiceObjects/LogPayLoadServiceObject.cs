using System;

namespace EMRReport.ServiceContracts.ServiceObjects
{
    public sealed class LogPayLoadServiceObject
    {
        public string ApiEndPoint { get; set; }
        public string FunctionName { get; set; }
        public string IPAddress { get; set; }
        public string RequestStatus { get; set; }
        public string RequestData { get; set; }
        public string RequestResponse { get; set; }
        public string ExceptionMessage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime RequestStartTime { get; set; }
        public DateTime RequestCompletedTime { get; set; }
    }
}
