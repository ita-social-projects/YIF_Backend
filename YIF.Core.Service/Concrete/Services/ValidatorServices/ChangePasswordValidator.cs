using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services.ValidatorServices
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordApiModel>
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly IRecaptchaService _recaptcha;

        private DbUser User = new DbUser();

        public ChangePasswordValidator(UserManager<DbUser> userManager, IRecaptchaService recaptcha)
        {
            _userManager = userManager;
            _recaptcha = recaptcha;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RecaptchaToken)
               .NotNull().WithMessage("Recaptcha є обов'язковою!")
               .Must(_recaptcha.IsValid).WithMessage("Роботи атакують!");

            RuleFor(x => x.UserId)
                .Must(x => IsUserExist(x)).WithMessage("Такого користувача не існує");

            RuleFor(x => x.OldPassword)
                .Must(x => IsOldPasswordCorrect(GetUserById(User.Id), x)).WithMessage("Старий пароль неправильний");

            RuleFor(x => x.NewPassword)
                 .NotEmpty().WithMessage("Пароль є обов'язковим!")
                 .Length(8, 20).WithMessage("Пароль має містити мінімум 8 символів і максимум 20 (включно)!")
                 .Matches(@"[A-Z]+").WithMessage("Пароль має містити щонайменше одну літеру верхнього регістру!")
                 .Matches(@"[a-z]+").WithMessage("Пароль має містити щонайменше одну літеру нижнього регістру!")
                 .Matches(@"[0-9]+").WithMessage("Пароль має містити щонайменше одну цифру!")
                 .Matches(@"[\W_]+").WithMessage("Пароль має містити щонайменше один спеціальний символ!");

            RuleFor(x => x.NewPassword)
                .Equal(x => x.ConfirmNewPassword).WithMessage("Пароль і новий пароль не співпадають");
        }

        private bool IsUserExist(string id)
        {
            User.Id = id;
            var user = Task.FromResult(_userManager.FindByIdAsync(id)).Result.Result;

            return user == null ? false : true;
        }
    
        private bool IsOldPasswordCorrect(DbUser user,string passwordHash)
        {
            var result = _userManager.CheckPasswordAsync(user, passwordHash).Result;

            return result;
        }

        private DbUser GetUserById(string id)
        {
            return Task.FromResult(_userManager.FindByIdAsync(id)).Result.Result;
        }
    }
}
