using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System.IO;

namespace EMRReport.API.Validators.User
{
    public sealed class BulkSaveUserRequestDtoValidator : AbstractValidator<BulkSaveUserRequestDto>
    {
        private const string excelfileNotNull = "File Missing";
        private const string excelfileType = "Excel file Required";
        public BulkSaveUserRequestDtoValidator()
        {
            RuleFor(validator => validator.Excelfile).NotNull().WithMessage(excelfileNotNull);
            RuleFor(x => x.Excelfile.FileName).Must(x => Path.GetExtension(x.ToLower()).Equals(".xlsx"))
                .WithMessage(excelfileType);
        }
    }
}
