namespace EMRReport.API.DataTranserObject.ValidatorError
{
    public sealed class GetExternalValidatorErrorRequestDto
    {
        public string CaseNumber { get; set; }
        public string CPTS { get; set; }
        public string ICDS { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
