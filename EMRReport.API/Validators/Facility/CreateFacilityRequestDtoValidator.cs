using EMRReport.API.DataTranserObject.Facility;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.Facility
{
    public sealed class CreateFacilityRequestDtoValidator : AbstractValidator<CreateFacilityRequestDto>
    {
        private const string FacilityNameEmpty = "Facility Name Required";
        private const string FacilityCodeEmpty = "Facility Code Required";
        public CreateFacilityRequestDtoValidator()
        {
            RuleFor(validator => validator.FacilityName)
           .NotNull().WithMessage(FacilityNameEmpty)
           .NotEmpty().WithMessage(FacilityNameEmpty);
            RuleFor(validator => validator.FacilityCode)
           .NotNull().WithMessage(FacilityCodeEmpty)
           .NotEmpty().WithMessage(FacilityCodeEmpty);
            RuleFor(validator => validator.FacilityTypeID)
           .GreaterThan(0);
            RuleFor(validator => validator.RegulatoryID)
           .GreaterThan(0);
        }
    }
}
