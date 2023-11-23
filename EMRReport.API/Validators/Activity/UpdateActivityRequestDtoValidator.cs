using EMRReport.API.DataTranserObject.Activity;
using FluentValidation;

namespace EMRReport.API.Validators.Activity
{
    public sealed class UpdateActivityRequestDtoValidator : AbstractValidator<UpdateActivityRequestDto>
    {
        private const string activityNameEmpty = "Activity Name Required";
        private const string activityNumberNotZero = "Activity Name Required";
        private const string activityIDEmpty = "Activity Name Required";
        public UpdateActivityRequestDtoValidator()
        {
            RuleFor(validator => validator.ActivityName).NotEmpty().NotNull().WithMessage(activityNameEmpty)
           .NotEmpty().WithMessage(activityNameEmpty);
            RuleFor(validator => validator.ActivityNumber).GreaterThan(0).WithMessage(activityNumberNotZero)
           .NotEmpty().WithMessage(activityNumberNotZero);
            RuleFor(validator => validator.ActivityID).GreaterThan(0).WithMessage(activityIDEmpty)
            .NotEmpty().WithMessage(activityIDEmpty);
        }
    }
}
