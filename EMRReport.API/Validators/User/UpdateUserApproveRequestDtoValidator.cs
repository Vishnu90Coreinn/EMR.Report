using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.User
{
    public sealed class UpdateUserApproveRequestDtoValidator : AbstractValidator<UpdateUserApproveRequestDto>
    {
        private const string UserIdEmpty = "User Id Required";
        private const string UseRoleEmpty = "User Role Required";
        private const string UseTypeEmpty = "User Type Required";
        private const string RuleVersionEmpty = "Rule Version Required";
        private const string AuthorityTypeEmpty = "Authority Type Required";
        private const string ApplicationTypeEmpty = "Application Type Required";

        public UpdateUserApproveRequestDtoValidator()
        {
            RuleFor(validator => validator.UserID)
            .GreaterThan(0).WithMessage(UserIdEmpty);
            RuleFor(validator => validator.UserRoleId)
            .GreaterThan(0).WithMessage(UseRoleEmpty);
            RuleFor(validator => validator.UserTypeId)
            .GreaterThan(0).WithMessage(UseTypeEmpty);
            RuleFor(validator => validator.RuleVersion)
            .GreaterThan(0).WithMessage(RuleVersionEmpty);
            RuleFor(validator => validator.AuthorityType)
          .GreaterThan(0).WithMessage(AuthorityTypeEmpty);
            RuleFor(validator => validator.ApplicationType)
         .GreaterThan(0).WithMessage(ApplicationTypeEmpty);
        }
    }
}
