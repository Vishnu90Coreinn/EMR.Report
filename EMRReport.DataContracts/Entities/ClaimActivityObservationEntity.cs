using System;
namespace EMRReport.DataContracts.Entities
{
    public sealed class ClaimActivityObservationEntity
    {
        public int ClaimActivityObservationID { get; set; }
        public int ClaimBasketID { get; set; }
        public int XMLActivityTagID { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public int FileStatus { get; set; }
    }
}