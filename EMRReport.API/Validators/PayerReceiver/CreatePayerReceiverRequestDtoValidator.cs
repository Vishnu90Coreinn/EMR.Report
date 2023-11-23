using EMRReport.API.DataTranserObject.PayerReceiver;
using FluentValidation;

namespace EMRReport.API.Validators.PayerReceiver
{
    public sealed class CreatePayerReceiverRequestDtoValidator : AbstractValidator<CreatePayerReceiverRequestDto>
    {
        private const string PayerReceiverNameEmpty = "Payer Receiver Name Required";
        private const string PayerReceiverIdentification = "Identification Code Required";
        private const string ShortNameNameEmpty = "Short Name Required";
        private const string InsuranceClassificationIDMissing = "Insurance Classification Required";
        private const string FacilityIDMissing = "Facility Required";
        private const string RegulatoryIDMissing = "Regulatory Required";
        public CreatePayerReceiverRequestDtoValidator()
        {
            RuleFor(validator => validator.PayerReceiverName)
            .NotNull().WithMessage(PayerReceiverNameEmpty).NotEmpty().WithMessage(PayerReceiverNameEmpty);
            RuleFor(validator => validator.PayerReceiverIdentification)
            .NotNull().WithMessage(PayerReceiverIdentification).NotEmpty().WithMessage(PayerReceiverIdentification);
            RuleFor(validator => validator.PayerReceiverShortName)
            .NotNull().WithMessage(ShortNameNameEmpty).NotEmpty().WithMessage(ShortNameNameEmpty);
            RuleFor(validator => validator.InsuranceClassificationID).GreaterThan(0)
                .WithMessage(InsuranceClassificationIDMissing);
            RuleFor(validator => validator.FacilityID).GreaterThan(0)
                .WithMessage(FacilityIDMissing);
            RuleFor(validator => validator.RegulatoryID).GreaterThan(0)
                .WithMessage(RegulatoryIDMissing);
        }
    }
}
