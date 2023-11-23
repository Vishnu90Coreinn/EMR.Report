using EMRReport.API.DataTranserObject.ValidatorError;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.ValidatorError
{
    public sealed class GetValidatorErrorRequestDtoValidator : AbstractValidator<GetValidatorErrorRequestDto>
    {
        private const string CaseNumberempty = "CaseNumber Required";
        private const string ICDCPTEmpty = "ICD and CPT both Cannot be Empty";
        public GetValidatorErrorRequestDtoValidator()
        {
            RuleFor(validator => validator.CaseNumber)
            .NotNull().WithMessage(CaseNumberempty)
            .NotEmpty().WithMessage(CaseNumberempty)
            .MinimumLength(5)
            .MaximumLength(10);
            When(validator => string.IsNullOrEmpty(validator.CPTS) && string.IsNullOrEmpty(validator.ICDS), () =>
          {
              RuleFor(validator => validator.CPTS).NotNull().WithMessage(ICDCPTEmpty);
              RuleFor(validator => validator.CPTS).NotEmpty().WithMessage(ICDCPTEmpty);
              RuleFor(validator => validator.ICDS).NotNull().WithMessage(ICDCPTEmpty);
              RuleFor(validator => validator.ICDS).NotEmpty().WithMessage(ICDCPTEmpty);
          }).Otherwise(() => { });
        }
    }
}
