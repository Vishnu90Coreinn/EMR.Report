using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.User
{
    public sealed class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        private const string UserNameEmpty = "User Name Required";
        private const string CurrentPasswordEmpty = "Current Password Required";
        private const string NewPasswordEmpty = "New Password Required";
        private const string ConfirmPasswordEmpty = "Confirm Password Required";
        private const string ConfirmPasswordMismach = "Password Mismatch";
        public ChangePasswordRequestDtoValidator()
        {
            RuleFor(validator => validator.UserName)
           .NotNull().WithMessage(UserNameEmpty)
           .NotEmpty().WithMessage(UserNameEmpty);
            RuleFor(validator => validator.CurrentPassword)
            .NotNull().WithMessage(CurrentPasswordEmpty)
            .NotEmpty().WithMessage(CurrentPasswordEmpty);
            RuleFor(validator => validator.NewPassword)
            .NotNull().WithMessage(NewPasswordEmpty)
            .NotEmpty().WithMessage(NewPasswordEmpty)
            .MinimumLength(6).MaximumLength(26);
            RuleFor(user => user.ConfirmPassword)
            .NotNull().WithMessage(ConfirmPasswordEmpty)
            .NotNull().WithMessage(ConfirmPasswordEmpty)
            .Equal(user => user.NewPassword).WithMessage(ConfirmPasswordMismach);
        }
    }
}
