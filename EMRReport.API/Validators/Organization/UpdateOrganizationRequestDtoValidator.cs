using EMRReport.API.DataTranserObject.Organization;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.Organization
{
    public sealed class UpdateOrganizationRequestDtoValidator : AbstractValidator<UpdateOrganizationRequestDto>
    {
        private const string OrganizationNameEmpty = "Organization Name Required";
        public UpdateOrganizationRequestDtoValidator()
        {
            RuleFor(validator => validator.OrganizationName)
           .NotNull().WithMessage(OrganizationNameEmpty)
           .NotEmpty().WithMessage(OrganizationNameEmpty);
            RuleFor(validator => validator.OrganizationID)
          .GreaterThan(0);
        }
    }
}
