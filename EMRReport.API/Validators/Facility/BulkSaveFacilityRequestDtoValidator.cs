using EMRReport.API.DataTranserObject.Facility;
using FluentValidation;
using System.IO;

namespace EMRReport.API.Validators.Facility
{
    public sealed class BulkSaveFacilityRequestDtoValidator : AbstractValidator<BulkSaveFacilityRequestDto>
    {
        private const string excelfileNotNull = "File Missing";
        private const string excelfileType = "Excel file Required";
        public BulkSaveFacilityRequestDtoValidator()
        {
            RuleFor(validator => validator.Excelfile).NotNull().WithMessage(excelfileNotNull);
            RuleFor(x => x.Excelfile.FileName).Must(x => Path.GetExtension(x.ToLower()).Equals(".xlsx"))
                .WithMessage(excelfileType);
        }
    }
}
