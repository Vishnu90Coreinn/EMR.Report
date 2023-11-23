using EMRReport.API.DataTranserObject.FacilityCategory;
using FluentValidation;

namespace EMRReport.API.Validators.FacilityCategory
{
    public sealed class CreateFacilityCategoryRequestDtoValidator : AbstractValidator<CreateFacilityCategoryRequestDto>
    {
        private const string FacilityCategoryNameEmpty = "FacilityCategory Name Required";
        public CreateFacilityCategoryRequestDtoValidator()
        {
            RuleFor(validator => validator.FacilityCategoryName)
           .NotNull().WithMessage(FacilityCategoryNameEmpty)
           .NotEmpty().WithMessage(FacilityCategoryNameEmpty);
        }
    }
}
