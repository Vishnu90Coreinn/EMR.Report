namespace EMRReport.API.DataTranserObject.ValidatorError
{
    public sealed class GetValidatorErrorDetailResponseDto
    {
        public string ErrorCode1 { get; set; }
        public string ErrorCode2 { get; set; }
        public int PrefixType { get; set; }
        public string Message { get; set; }
        public string CodingTips { get; set; }
    }
}
