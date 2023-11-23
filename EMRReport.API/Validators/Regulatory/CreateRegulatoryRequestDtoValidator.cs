using EMRReport.API.DataTranserObject.Regulatory;
using FluentValidation;

namespace EMRReport.API.Validators.Regulatory
{
    public sealed class CreateRegulatoryRequestDtoValidator : AbstractValidator<CreateRegulatoryRequestDto>
    {
        private const string RegulatoryNameEmpty = "Regulatory Name Required";
        public CreateRegulatoryRequestDtoValidator()
        {
            RuleFor(validator => validator.RegulatoryName)
           .NotNull().WithMessage(RegulatoryNameEmpty)
           .NotEmpty().WithMessage(RegulatoryNameEmpty);
        }
    }
}
