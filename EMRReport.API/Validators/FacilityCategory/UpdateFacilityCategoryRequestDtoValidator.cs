using EMRReport.API.DataTranserObject.FacilityCategory;
using FluentValidation;

namespace EMRReport.API.Validators.FacilityCategory
{
    public sealed class UpdateFacilityCategoryRequestDtoValidator : AbstractValidator<UpdateFacilityCategoryRequestDto>
    {
        private const string FacilityCategoryIdRequired = "Facility Category Id Required";
        private const string RegulatoryNameEmpty = "Facility Category Name Required";
        public UpdateFacilityCategoryRequestDtoValidator()
        {
            RuleFor(validator => validator.FacilityCategoryID)
            .GreaterThan(0).WithMessage(FacilityCategoryIdRequired);
            RuleFor(validator => validator.FacilityCategoryName)
           .NotNull().WithMessage(RegulatoryNameEmpty)
           .NotEmpty().WithMessage(RegulatoryNameEmpty);
        }
    }
}
