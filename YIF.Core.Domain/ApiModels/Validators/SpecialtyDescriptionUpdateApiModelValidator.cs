using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.Validators;

namespace YIF.Core.Domain.Validators
{

    public class SpecialtyDescriptionUpdateApiModelValidator : AbstractValidator<SpecialtyDescriptionUpdateApiModel>
    {
        private readonly EFDbContext _context;
        private readonly string NotExistInDbMessage = "Such {PropertyName} doesn't exist in the database";

        public SpecialtyDescriptionUpdateApiModelValidator(EFDbContext context)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            _context = context;

            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.SpecialtyToInstitutionOfEducationId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.EducationForm)
                .IsInEnum();

            RuleFor(x => x.PaymentForm)
                .IsInEnum();

            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.EducationalProgramLink)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Id)
                .Must(x => _context.SpecialtyToIoEDescriptions.Any(y => y.Id == x)).WithMessage(NotExistInDbMessage);

            RuleFor(x => x.SpecialtyToInstitutionOfEducationId)
                .Must(x => _context.SpecialtyToInstitutionOfEducations.Any(y => y.Id == x)).WithMessage(NotExistInDbMessage).WithName("Specialty to IoE description id");

            RuleForEach(x => x.ExamRequirements).SetValidator(new ExamRequirementUpdateApiModelValidator(_context));
        }
    }
}
