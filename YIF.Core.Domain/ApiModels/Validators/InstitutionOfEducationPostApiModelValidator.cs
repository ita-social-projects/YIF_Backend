using FluentValidation;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class InstitutionOfEducationPostApiModelValidator : AbstractValidator<InstitutionOfEducationPostApiModel>
    {
        public InstitutionOfEducationPostApiModelValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotEmpty().When(x => x.Name != null)
                .MaximumLength(255);

            RuleFor(x => x.Abbreviation)
                .NotEmpty().When(x => x.Abbreviation != null)
                .MaximumLength(255);

            RuleFor(x => x.Site)
                .NotEmpty().When(x => x.Site != null)
                .MaximumLength(255);

            RuleFor(x => x.Address)
                .NotEmpty().When(x => x.Address != null)
                .MaximumLength(255);

            RuleFor(x => x.Phone)
                .NotEmpty().When(x => x.Phone != null);

            RuleFor(x => x.Email)
                .EmailAddress().When(x => x.Email != null)
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotEmpty().When(x => x.Description != null)
                .MaximumLength(255);

            RuleFor(x => x.Lat)
                .NotEmpty().When(x => x.Lat != 0);

            RuleFor(x => x.Lon)
                .NotEmpty().When(x => x.Lon != 0);

            RuleFor(x => x.InstitutionOfEducationType)
                .IsInEnum();

            RuleFor(x => x.ImageApiModel)
                .SetValidator(new ImageBase64Validator()).When(x => x.ImageApiModel != null);
        }
    }
}
