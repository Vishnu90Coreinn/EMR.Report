using EMRReport.API.DataTranserObject.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.User
{
    public sealed class UpdateUserRejectRequestDtoValidator : AbstractValidator<UpdateUserRejectRequestDto>
    {
        private const string UserIdEmpty = "User Id Required";
        private const string ReasonEmpty = "Reason Required";
        public UpdateUserRejectRequestDtoValidator()
        {
            RuleFor(validator => validator.UserID)
          .GreaterThan(0).WithMessage(UserIdEmpty);
            RuleFor(validator => validator.Reason)
            .NotNull().NotEmpty().WithMessage(ReasonEmpty);
        }
    }
}
