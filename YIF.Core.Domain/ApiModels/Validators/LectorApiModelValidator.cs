using FluentValidation;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class LectorApiModelValidator: AbstractValidator<LectorApiModel>
    {
        public LectorApiModelValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            // Will be implemented when discussed with a productOwner

            //RuleFor(x => x.Name)
            //    .NotEmpty()
            //    .When(x => x.Name != null)
            //    .MaximumLength(255);

            //RuleFor(x => x.Description)
            //    .NotEmpty()
            //    .When(x => x.Description != null)
            //    .NotNull();

            //RuleFor(x => x.InstitutionOfEducationId)
            //    .NotEmpty()
            //    .When(x => x.InstitutionOfEducationId != null)
            //    .NotNull();

            RuleFor(x => x.ImageApiModel)
                .SetValidator(new ImageBase64Validator()).When(x => x.ImageApiModel != null);
        }
    }
}
