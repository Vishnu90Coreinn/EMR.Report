using EMRReport.API.DataTranserObject.PayerReceiver;
using FluentValidation;
using System.IO;

namespace EMRReport.API.Validators.PayerReceiver
{
    public sealed class BulkSavePayerReceiverRequestDtoValidator : AbstractValidator<BulkSavePayerReceiverRequestDto>
    {
        private const string excelfileNotNull = "File Missing";
        private const string excelfileType = "Excel file Required";
        public BulkSavePayerReceiverRequestDtoValidator()
        {
            RuleFor(validator => validator.Excelfile).NotNull().WithMessage(excelfileNotNull);
            RuleFor(x => x.Excelfile.FileName).Must(x => Path.GetExtension(x.ToLower()).Equals(".xlsx"))
                .WithMessage(excelfileType);
        }
    }
}
