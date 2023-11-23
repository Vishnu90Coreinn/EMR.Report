using EMRReport.API.DataTranserObject.Regulatory;
using FluentValidation;

namespace EMRReport.API.Validators.Regulatory
{
    public sealed class UpdateRegulatoryRequestDtoValidator : AbstractValidator<UpdateRegulatoryRequestDto>
    {
        private const string RegulatoryIdRequired = "Regulatory Id Required";
        private const string RegulatoryNameEmpty = "Regulatory Name Required";
        public UpdateRegulatoryRequestDtoValidator()
        {
            RuleFor(validator => validator.RegulatoryID)
            .GreaterThan(0).WithMessage(RegulatoryIdRequired);
            RuleFor(validator => validator.RegulatoryName)
           .NotNull().WithMessage(RegulatoryNameEmpty)
           .NotEmpty().WithMessage(RegulatoryNameEmpty);
        }
    }
}
