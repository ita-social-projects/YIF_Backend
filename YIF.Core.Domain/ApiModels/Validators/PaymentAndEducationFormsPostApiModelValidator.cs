using FluentValidation;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class PaymentAndEducationFormsPostApiModelValidator : AbstractValidator<PaymentAndEducationFormsPostApiModel>
    {
        public PaymentAndEducationFormsPostApiModelValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.PaymentForm)
                .IsInEnum();

            RuleFor(x => x.EducationForm)
                .IsInEnum();
        }
    }
}
