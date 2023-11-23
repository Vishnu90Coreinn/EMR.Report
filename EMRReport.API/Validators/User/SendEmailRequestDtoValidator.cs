using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.User
{
    public sealed class SendEmailRequestDtoValidator : AbstractValidator<SendEmailRequestDto>
    {
        private const string UserIdEmpty = "User Id Required";
        public SendEmailRequestDtoValidator()
        {
            RuleFor(validator => validator.UserID)
          .GreaterThan(0).WithMessage(UserIdEmpty);
        }
    }
}
