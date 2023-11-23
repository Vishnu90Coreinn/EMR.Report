using EMRReport.API.DataTranserObject.ValidatorError;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.ValidatorError
{
    public sealed class GetValidatorTransactionRequestDtoValidator : AbstractValidator<GetValidatorTransactionRequestDto>
    {
        private const string ICDCPTEmpty = "ICD and CPT Cannot be Empty";
        private const string ICDEmpty = "ICD Cannot be Empty";
        private const string PrimaryEmpty = "Primary Cannot be Empty";
        private const string SecondaryEmpty = "Secondary Cannot be Empty";
        private const string ReasonForVisitEmpty = "ReasonForVisit Cannot be Empty";
        private const string CPTEmpty = "CPT Cannot be Empty";
        private const string QuantityZero = "Quantity Cannot be Zero";
        private const string ActivityTypeZero = "Activity Type Cannot be Zero";
        public GetValidatorTransactionRequestDtoValidator()
        {
            When(validator => validator.ValidatorCPTList == null && validator.ValidatorICDList == null, () =>
            {
                RuleFor(x => x.ValidatorCPTList).NotNull().WithMessage(ICDCPTEmpty);
                RuleFor(x => x.ValidatorICDList).NotNull().WithMessage(ICDCPTEmpty);
            }).Otherwise(() =>
            {
                RuleForEach(x => x.ValidatorICDList).ChildRules(ICDList =>
                {
                    ICDList.RuleFor(x => x.ICD).NotNull().NotEmpty().WithMessage(ICDEmpty);
                    ICDList.RuleFor(x => x.Primary).NotNull().WithMessage(PrimaryEmpty);
                    ICDList.RuleFor(x => x.Secondary).NotNull().WithMessage(SecondaryEmpty);
                    ICDList.RuleFor(x => x.ReasonForVisit).NotNull().WithMessage(ReasonForVisitEmpty);
                });
                RuleForEach(x => x.ValidatorCPTList).ChildRules(CPTList =>
                {
                    CPTList.RuleFor(x => x.CPT).NotNull().NotEmpty().WithMessage(CPTEmpty);
                    CPTList.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage(QuantityZero);
                    CPTList.RuleFor(x => x.ActivityType).GreaterThan(0).WithMessage(ActivityTypeZero);
                });
            });
        }
    }
}
