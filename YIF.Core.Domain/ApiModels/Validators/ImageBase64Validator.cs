using FluentValidation;
using System;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
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
