using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class InstitutionOfEducationPostApiModelValidator : AbstractValidator<InstitutionOfEducationPostApiModel>
    {
        public InstitutionOfEducationPostApiModelValidator()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Abbreviation)
                //.NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Site)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Phone)
                .NotNull()
                
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress()
                .MaximumLength(255);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Lat)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Lon)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.InstitutionOfEducationType)
                .NotNull()
                .NotEmpty()
                .IsInEnum();

            RuleFor(x => x.ImageApiModel)
                .SetValidator(new ImageBase64Validator());
        }
        public class ImageBase64Validator : AbstractValidator<ImageApiModel>
        {
            public ImageBase64Validator()
            {
                CascadeMode = CascadeMode.Stop;

                RuleFor(x => x.Photo)
                    .NotEmpty().WithMessage("Фото є обов'язковим.")
                    .Must(e => e.Contains("image")).WithMessage("Введіть фото у форматі base64 з типом image.")
                    .Must(IsBase64).WithMessage("Введіть фото у форматі base64.");
            }

            private bool IsBase64(string imagebase64)
            {
                string base64 = imagebase64;
                if (base64.Contains(","))
                {
                    base64 = base64.Split(',')[1];
                }
                Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
                return Convert.TryFromBase64String(base64, buffer, out _);
            }
        }
    }
}
