using EMRReport.API.DataTranserObject.Organization;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.Organization
{
    public sealed class CreateOrganizationRequestDtoValidator : AbstractValidator<CreateOrganizationRequestDto>
    {
        private const string OrganizationNameEmpty = "Organization Name Required";
        public CreateOrganizationRequestDtoValidator()
        {
            RuleFor(validator => validator.OrganizationName)
           .NotNull().WithMessage(OrganizationNameEmpty)
           .NotEmpty().WithMessage(OrganizationNameEmpty);
        }
    }
}
