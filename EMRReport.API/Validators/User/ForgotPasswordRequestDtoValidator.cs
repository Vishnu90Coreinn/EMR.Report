using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.User
{
    public sealed class ForgotPasswordRequestDtoValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        private const string UserNameEmpty = "User Name Required";
        private const string EmailEmpty = "Email Required";
        public ForgotPasswordRequestDtoValidator()
        {
            RuleFor(validator => validator.UserName)
           .NotNull().WithMessage(UserNameEmpty)
           .NotEmpty().WithMessage(UserNameEmpty);
            RuleFor(validator => validator.Email)
            .NotNull().WithMessage(EmailEmpty)
            .NotEmpty().WithMessage(EmailEmpty);
        }
    }
}
