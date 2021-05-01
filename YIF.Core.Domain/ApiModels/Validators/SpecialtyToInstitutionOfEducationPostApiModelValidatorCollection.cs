using FluentValidation;
using System.Linq;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyToInstitutionOfEducationPostApiModelValidatorCollection : AbstractValidator<SpecialtyToInstitutionOfEducationAddRangePostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resourceManager;
        public SpecialtyToInstitutionOfEducationPostApiModelValidatorCollection(EFDbContext context, ResourceManager resourceManager)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            _context = context;
            _resourceManager = resourceManager;

            RuleFor(x => x.SpecialtyId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.PaymentAndEducationForms)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.SpecialtyId)
                .Must(x => _context.Specialties.Any(y => y.Id == x))
                .WithMessage(_resourceManager.GetString("SpecialtyNotFound"));

            RuleForEach(x => x.PaymentAndEducationForms).SetValidator(new PaymentAndEducationFormsPostApiModelValidator());
        }
    }
}
