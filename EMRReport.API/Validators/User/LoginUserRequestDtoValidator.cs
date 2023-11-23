using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.User
{
    public sealed class LoginUserRequestDtoValidator : AbstractValidator<LoginUserRequestDto>
    {
        private const string UserNameEmpty = "User Name Required";
        private const string PasswordEmpty = "Password Required";
        public LoginUserRequestDtoValidator()
        {
            RuleFor(validator => validator.UserName)
           .NotNull().WithMessage(UserNameEmpty)
           .NotEmpty().WithMessage(UserNameEmpty);
            RuleFor(validator => validator.Password)
            .NotNull().WithMessage(PasswordEmpty)
            .NotEmpty().WithMessage(PasswordEmpty);
        }
    }
}
