using EMRReport.API.DataTranserObject.Activity;
using FluentValidation;

namespace EMRReport.API.Validators.Activity
{
    public sealed class CreateActivityRequestDtoValidator : AbstractValidator<CreateActivityRequestDto>
    {
        private const string activityNameEmpty = "Activity Name Required";
        private const string activityNumberEmpty = "Activity Name Required";
        public CreateActivityRequestDtoValidator()
        {
            RuleFor(validator => validator.ActivityName).NotEmpty().NotNull().WithMessage(activityNameEmpty)
           .NotEmpty().WithMessage(activityNameEmpty);
            RuleFor(validator => validator.ActivityNumber).GreaterThan(0).WithMessage(activityNumberEmpty)
           .NotEmpty().WithMessage(activityNumberEmpty);
        }
    }
}
