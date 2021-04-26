using FluentValidation;
using System.Linq;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyToInstitutionOfEducationPostApiModelValidatorCollection : AbstractValidator<SpecialtyToInstitutionOfEducationPostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resouseManager;
        public SpecialtyToInstitutionOfEducationPostApiModelValidatorCollection(EFDbContext context, ResourceManager resouseManager)
        {
            _context = context;
            _resouseManager = resouseManager;

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
                .WithMessage(_resouseManager.GetString("InstitutionOfEducationNotFound"));

            RuleFor(x => x.SpecialtyId)
                .Must(x => _context.Specialties.Any(y => y.Id == x))
                .WithMessage(_resouseManager.GetString("SpecialtyNotFound"));

            RuleForEach(x => x.PaymentAndEducationForms).SetValidator(new PaymentAndEducationFormsPostApiModelValidator(_context, _resouseManager));
        }
    }
}
