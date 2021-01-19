using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{

    public class LoginValidator : AbstractValidator<LoginApiModel>
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly IRecaptchaService _recaptcha;

        private DbUser _user;

        public LoginValidator(UserManager<DbUser> userManager, IRecaptchaService recaptcha)
        {
            _userManager = userManager;
            _recaptcha = recaptcha;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RecaptchaToken)
                .NotNull().WithMessage("Recaptcha є обов'язковою!")
                .Must(_recaptcha.IsValid).WithMessage("Роботи атакують!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Електронна пошта є обов'язковою!")
                .EmailAddress().WithMessage("Введіть дійсну електронну пошту!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим!")
                .Length(8, 20).WithMessage("Пароль має містити мінімум 8 символів і максимум 20 (включно)!")
                .Matches(@"[A-Z]+").WithMessage("Пароль має містити щонайменше одну літеру верхнього регістру!")
                .Matches(@"[a-z]+").WithMessage("Пароль має містити щонайменше одну літеру нижнього регістру!")
                .Matches(@"[0-9]+").WithMessage("Пароль має містити щонайменше одну цифру!")
                .Matches(@"[\W_]+").WithMessage("Пароль має містити щонайменше один спеціальний символ!");

            RuleFor(x => x.Email).Must(IsEmailExist).WithMessage("Логін або пароль неправильний!");
            RuleFor(x => x.Password).Must(IsPasswordCorrect).WithMessage("Логін або пароль неправильний!");
        }

        private bool IsEmailExist(string email)
        {
            _user = _userManager.FindByEmailAsync(email).Result;
            return _user != null;
        }

        private bool IsPasswordCorrect(string password)
        {
            return _userManager.CheckPasswordAsync(_user, password).Result;
        }
    }

    public class RegisterValidator : AbstractValidator<RegisterApiModel>
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly IRecaptchaService _recaptcha;

        public RegisterValidator(UserManager<DbUser> userManager, IRecaptchaService recaptcha)
        {
            _userManager = userManager;
            _recaptcha = recaptcha;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RecaptchaToken)
                .NotNull().WithMessage("Recaptcha є обов'язковою!")
                .Must(_recaptcha.IsValid).WithMessage("Роботи атакують!");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Електронна пошта є обов'язковою!")
                .EmailAddress().WithMessage("Введіть дійсну електронну пошту!");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Ім'я користувача є обов'язковим!")
                .Length(2, 100).WithMessage("Ім'я користувача має містити мінімум 2 символа і максимум 100 (включно)!")
                .Matches(@"[a-zA-z]+").WithMessage("Ім'я користувача має містити щонайменше одну латинську літеру!");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим!")
                .Length(8, 20).WithMessage("Пароль має містити мінімум 8 символів і максимум 20 (включно)!")
                .Matches(@"[A-Z]+").WithMessage("Пароль має містити щонайменше одну латинську літеру верхнього регістру!")
                .Matches(@"[a-z]+").WithMessage("Пароль має містити щонайменше одну латинську літеру нижнього регістру!")
                .Matches(@"[0-9]+").WithMessage("Пароль має містити щонайменше одну цифру!")
                .Matches(@"[\W_]+").WithMessage("Пароль має містити щонайменше один спеціальний символ!");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Паролі не співпадають!");

            RuleFor(x => x.Email).Must(IsEmailNotExist).WithMessage("Електронна пошта вже існує!");
            RuleFor(x => x.Username).Must(IsUsernameNotExist).WithMessage("Ім'я користувача вже існує!");
        }

        private bool IsEmailNotExist(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            return user == null;
        }

        private bool IsUsernameNotExist(string username)
        {
            var user = _userManager.FindByNameAsync(username).Result;
            return user == null;
        }
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
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
    }
}