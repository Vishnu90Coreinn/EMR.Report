using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.User
{
    public sealed class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
    {
        private const string UserIdEmpty = "User Id Required";
        private const string UserNameEmpty = "User Name Required";
        private const string UserNameLength = "Length must be between 6 ad 200 chars";
        private const string PasswordEmpty = "Password Required";
        private const string PasswordLength = "Length must be between 6 ad 26 chars";
        private const string PasswordExpression = "Password must have at least one smaill letter and capital and numeric charater";
        private const string FristNameEmpty = "First Name Required";
        private const string LastNameEmpty = "Last Name Required";
        private const string UseRoleEmpty = "User Role Required";
        private const string UseTypeEmpty = "User Type Required";
        private const string EmailEmpty = "Email Required";
        private const string EmailInvalid = "Email Invalid";
        public UpdateUserRequestDtoValidator()
        {
            RuleFor(validator => validator.UserID)
          .GreaterThan(0).WithMessage(UserIdEmpty);
            RuleFor(validator => validator.UserName)
           .NotNull().WithMessage(UserNameEmpty)
           .NotEmpty().WithMessage(UserNameEmpty)
           .Length(6, 200).WithMessage(UserNameLength);
            RuleFor(validator => validator.Password)
            .NotNull().WithMessage(PasswordEmpty)
            .NotEmpty().WithMessage(PasswordEmpty)
            .Length(6, 26).WithMessage(PasswordLength)
            .Matches("(?!^[0-9]*$)(?!^[a-z]*$)(?!^[A-Z]*$)^(.{6,26})$").WithMessage(PasswordExpression);
            RuleFor(validator => validator.FirstName)
           .NotNull().WithMessage(FristNameEmpty)
           .NotEmpty().WithMessage(FristNameEmpty);
            RuleFor(validator => validator.LastName)
             .NotNull().WithMessage(LastNameEmpty)
             .NotEmpty().WithMessage(LastNameEmpty);
            RuleFor(validator => validator.UserRoleId)
            .GreaterThan(0).WithMessage(UseRoleEmpty);
            RuleFor(validator => validator.UserTypeId)
            .GreaterThan(0).WithMessage(UseTypeEmpty);
            RuleFor(validator => validator.Email)
               .NotNull().WithMessage(EmailEmpty)
            .NotEmpty().WithMessage(EmailEmpty)
            .Matches("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$").WithMessage(EmailInvalid);
        }
    }
}
