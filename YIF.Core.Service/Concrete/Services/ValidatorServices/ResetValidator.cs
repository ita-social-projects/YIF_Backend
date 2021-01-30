using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services.ValidatorServices
{
    public class ResetValidator : AbstractValidator<RestoreApiModel>
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly IRecaptchaService _recaptcha;

        public ResetValidator(UserManager<DbUser> userManager, IRecaptchaService recaptcha)
        {
            _userManager = userManager;
            _recaptcha = recaptcha;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RecaptchaToken)
               .NotNull().WithMessage("Recaptcha є обов'язковою!")
               .Must(_recaptcha.IsValid).WithMessage("Роботи атакують!");

            RuleFor(x => x.UserId)
                .Must(x => IsUserExist(x)).WithMessage("Такого користувача не існує");

            RuleFor(x => x.NewPassword)
                 .NotEmpty().WithMessage("Пароль є обов'язковим!")
                 .Length(8, 20).WithMessage("Пароль має містити мінімум 8 символів і максимум 20 (включно)!")
                 .Matches(@"[A-Z]+").WithMessage("Пароль має містити щонайменше одну літеру верхнього регістру!")
                 .Matches(@"[a-z]+").WithMessage("Пароль має містити щонайменше одну літеру нижнього регістру!")
                 .Matches(@"[0-9]+").WithMessage("Пароль має містити щонайменше одну цифру!")
                 .Matches(@"[\W_]+").WithMessage("Пароль має містити щонайменше один спеціальний символ!");

            RuleFor(x => x.NewPassword)
                .Equal(x => x.ConfirmNewPassword).WithMessage("Паролі не співпадають");
        }

        private bool IsUserExist(string id)
        {
            var user = Task.FromResult(_userManager.FindByIdAsync(id)).Result.Result;
            return user != null;
        }
    }
}
