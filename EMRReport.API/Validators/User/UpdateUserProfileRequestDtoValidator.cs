using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;

namespace EMRReport.API.Validators.User
{
    public sealed class UpdateUserProfileRequestDtoValidator : AbstractValidator<UpdateUserProfileRequestDto>
    {
        private const string UserIdEmpty = "User Id Required";
        private const string FristNameEmpty = "First Name Required";
        private const string LastNameEmpty = "Last Name Required";
        private const string EmailEmpty = "Email Required";
        private const string EmailInvalid = "Email Invalid";
        public UpdateUserProfileRequestDtoValidator()
        {
            RuleFor(validator => validator.UserID)
          .GreaterThan(0).WithMessage(UserIdEmpty);
            RuleFor(validator => validator.FirstName)
           .NotNull().WithMessage(FristNameEmpty)
           .NotEmpty().WithMessage(FristNameEmpty);
            RuleFor(validator => validator.LastName)
             .NotNull().WithMessage(LastNameEmpty)
             .NotEmpty().WithMessage(LastNameEmpty);
            RuleFor(validator => validator.Email)
               .NotNull().WithMessage(EmailEmpty)
            .NotEmpty().WithMessage(EmailEmpty)
            .Matches("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$").WithMessage(EmailInvalid);
        }
    }
}
