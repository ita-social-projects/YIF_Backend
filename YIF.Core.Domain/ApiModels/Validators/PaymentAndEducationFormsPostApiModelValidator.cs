using FluentValidation;
using System.Linq;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class PaymentAndEducationFormsPostApiModelValidator : AbstractValidator<PaymentAndEducationFormsPostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resouseManager;

        public PaymentAndEducationFormsPostApiModelValidator(EFDbContext context, ResourceManager resouseManager)
        {
            _context = context;
            _resouseManager = resouseManager;

            RuleFor(x => x.EducationForm)
                .IsInEnum();

            RuleFor(x => x.PaymentForm)
                .IsInEnum();

            RuleFor(x => x.PaymentForm)
               .Must(x => _context.SpecialtyToIoEDescriptions.All(y => y.PaymentForm != PaymentForm.Contract || y.PaymentForm != PaymentForm.Govermental))
               .WithMessage(_resouseManager.GetString("ModelIsInvalid"));

            RuleFor(x => x.EducationForm)
               .Must(x => _context.SpecialtyToIoEDescriptions.All(y => y.EducationForm != EducationForm.Daily || y.EducationForm != EducationForm.Remote))
               .WithMessage(_resouseManager.GetString("ModelIsInvalid"));
        }
    }
}
