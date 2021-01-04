using FluentValidation;
using Microsoft.AspNetCore.Identity;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Service.Concrete.Services
{

    public class LoginValidator : AbstractValidator<LoginApiModel>
    {
        private readonly UserManager<DbUser> _userManager;
        private DbUser _user;

        public LoginValidator(UserManager<DbUser> userManager)
        {
            _userManager = userManager;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email).NotNull().WithMessage("Електронна пошта є обов'язковою!")
                .EmailAddress().WithMessage("Введіть дійсну електронну пошту!")
                .Must(IsEmailExist).WithMessage("Логін або пароль неправильний!");

            RuleFor(x => x.Password).NotNull().WithMessage("Пароль є обов'язковим!")
                .Length(8, 20).WithMessage("Пароль має містити мінімум 8 символів і максимум 20 (включно)!")
                .Matches(@"[A-Z]+").WithMessage("Пароль має містити щонайменше одну літеру верхнього регістру!")
                .Matches(@"[a-z]+").WithMessage("Пароль має містити щонайменше одну літеру нижнього регістру!")
                .Matches(@"[0-9]+").WithMessage("Пароль має містити щонайменше одну цифру!")
                .Matches(@"[\W_]+").WithMessage("Пароль має містити щонайменше один спеціальний символ!")
                .Must(IsPasswordCorrect).WithMessage("Логін або пароль неправильний!");

            RuleFor(x => x.RecaptchaToken).NotNull().WithMessage("Recaptcha є обов'язковою!");

            //^A-Za-z0-9
        }

        private bool IsEmailExist(string email)
        {
            _user = _userManager.FindByEmailAsync(email).Result;
            return _user != null ? true : false;
        }

        private bool IsPasswordCorrect(string password)
        {
            return _userManager.CheckPasswordAsync(_user, password).Result;
        }
    }

    public class RegisterValidator : AbstractValidator<RegisterApiModel>
    {
        private readonly UserManager<DbUser> _userManager;
        public RegisterValidator(UserManager<DbUser> userManager)
        {
            _userManager = userManager;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Password).NotNull().WithMessage("Пароль є обов'язковим!")
                .Length(8, 20).WithMessage("Пароль має містити мінімум 8 символів і максимум 20 (включно)!")
                .Matches(@"[A-Z]+").WithMessage("Пароль має містити щонайменше одну літеру верхнього регістру!")
                .Matches(@"[a-z]+").WithMessage("Пароль має містити щонайменше одну літеру нижнього регістру!")
                .Matches(@"[0-9]+").WithMessage("Пароль має містити щонайменше одну цифру!")
                .Matches(@"[\W_]+").WithMessage("Пароль має містити щонайменше один спеціальний символ!");

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Паролі не збігаються!");

            RuleFor(x => x.Email).NotNull().WithMessage("Електронна пошта є обов'язковою!")
                .EmailAddress().WithMessage("Введіть дійсну електронну пошта!")
                .Must(IsEmailNotExist).WithMessage("Електронна пошта вже існує!");
        }

        private bool IsEmailNotExist(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            return user == null ? true : false;
        }
    }
}