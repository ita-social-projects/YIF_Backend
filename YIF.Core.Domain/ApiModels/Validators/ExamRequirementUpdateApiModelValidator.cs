using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class ExamRequirementUpdateApiModelValidator : AbstractValidator<ExamRequirementUpdateApiModel>
    {
        private readonly EFDbContext _context;
        private readonly string  NotExistInDbMessage = "Such {PropertyName} doesn't exist in the database";

        public ExamRequirementUpdateApiModelValidator(EFDbContext context)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            _context = context;

            RuleFor(x => x.ExamId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.SpecialtyToIoEDescriptionId)
                .NotEmpty()
                .NotNull().WithName("Specialty to IoE description id");

            RuleFor(x => x.Coefficient)
                .NotEmpty()
                .NotNull()
                .ExclusiveBetween(0, 1);

            RuleFor(x => x.MinimumScore)
                .NotEmpty()
                .NotNull()
                .InclusiveBetween(100, 200);

            RuleFor(x => x.ExamId)
                .Must(x => _context.Exams.Any(y => y.Id == x)).WithMessage(NotExistInDbMessage);

            RuleFor(x => x.SpecialtyToIoEDescriptionId)
                .Must(x => _context.SpecialtyToIoEDescriptions.Any(y => y.Id == x)).WithMessage(NotExistInDbMessage).WithName("Specialty to IoE description id");
        }
    }
}
