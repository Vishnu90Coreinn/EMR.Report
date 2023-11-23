using EMRReport.API.DataTranserObject.ScrubberError;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.Validators.ScrubberError
{
    public sealed class GetScrubberErrorRequestDtoValidator : AbstractValidator<GetScrubberErrorRequestDto>
    {
        private const string XMLNull = "File Required";
        private const string XMLFilesOnly = "Can validate XML files only";
        private const string XMLFilesCountLimit = "Number of files cannot be exceet more than 25";
        private const string XMLFilesSizetLimit = "Total size of files cannot be exceet more than 25MB";
        public GetScrubberErrorRequestDtoValidator()
        {
            RuleFor(validator => validator.XMLfiles).NotNull().WithMessage(XMLNull);
            RuleFor(validator => validator.XMLfiles.Where(x => !(x.ContentType.Equals("text/xml") || x.ContentType.Equals("application/xml"))).Count())
                .Equal(0).WithMessage(XMLFilesOnly);
            RuleFor(validator => validator.XMLfiles.Count).LessThan(26).WithMessage(XMLFilesCountLimit);
            RuleFor(validator => validator.XMLfiles.Sum(x => x.Length)).LessThan(21092851).WithMessage(XMLFilesSizetLimit);
        }
    }
}
