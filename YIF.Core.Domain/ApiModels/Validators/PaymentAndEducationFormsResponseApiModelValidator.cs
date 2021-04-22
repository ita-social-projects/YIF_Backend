using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class PaymentAndEducationFormsResponseApiModelValidator : AbstractValidator<PaymentAndEducationFormsResponseApiModel>
    {
        private readonly EFDbContext _context;
        private readonly string NotValid = "Such {PropertyName} isn't valid";

        public PaymentAndEducationFormsResponseApiModelValidator(EFDbContext context)
        {
            _context = context;

            RuleFor(x => x.EducationForm)
                .IsInEnum();

            RuleFor(x => x.PaymentForm)
                .IsInEnum();

            RuleFor(x => x.PaymentForm)
               .Must(x => _context.SpecialtyToIoEDescriptions.All(y => y.PaymentForm != PaymentForm.Contract || y.PaymentForm != PaymentForm.Govermental))
               .WithMessage(NotValid);

            RuleFor(x => x.EducationForm)
               .Must(x => _context.SpecialtyToIoEDescriptions.All(y => y.EducationForm != EducationForm.Daily || y.EducationForm != EducationForm.Remote))
               .WithMessage(NotValid);
        }
    }
}
