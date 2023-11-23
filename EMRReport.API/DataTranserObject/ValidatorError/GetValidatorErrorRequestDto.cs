namespace EMRReport.API.DataTranserObject.ValidatorError
{
    public sealed class GetValidatorErrorRequestDto
    {
        public string CaseNumber { get; set; }
        public string CPTS { get; set; }
        public string ICDS { get; set; }
    }
}
