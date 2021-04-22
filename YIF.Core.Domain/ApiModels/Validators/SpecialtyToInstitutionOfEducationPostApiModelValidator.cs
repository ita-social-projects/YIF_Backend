using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyToInstitutionOfEducationPostApiModelValidator : AbstractValidator<SpecialtyToInstitutionOfEducationPostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly string NotFoundInDbMessage = "Such {PropertyName} wasn't found in the database";

        public SpecialtyToInstitutionOfEducationPostApiModelValidator(EFDbContext context)
        {
            _context = context;

            RuleFor(x => x.InstitutionOfEducationId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.SpecialtyId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.PaymentAndEducationForms)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.InstitutionOfEducationId)
                .Must(x => _context.InstitutionOfEducations.Any(y => y.Id == x))
                .WithMessage(NotFoundInDbMessage);

            RuleFor(x => x.SpecialtyId)
                .Must(x => _context.Specialties.Any(y => y.Id == x))
                .WithMessage(NotFoundInDbMessage);

            RuleForEach(x => x.PaymentAndEducationForms).SetValidator(new PaymentAndEducationFormsResponseApiModelValidator(_context));
        }
    }
}
