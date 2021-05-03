using FluentValidation;
using System.Linq;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class LectorPostApiModelValidator : AbstractValidator<LectorPostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resourceManager;

        public LectorPostApiModelValidator(EFDbContext context, ResourceManager resourceManager)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            _context = context;
            _resourceManager = resourceManager;

            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.Email)
                .Must(x => _context.Users.Any(y => y.Email == x && y.IsDeleted == false))
                .WithMessage(_resourceManager.GetString("UserDoesNotExist"));

            RuleFor(x => x.Email)
                .Must(x => _context.Lectures.Any(y => y.User.Email != x))
                .WithMessage(_resourceManager.GetString("IoEAlreadyHasLector"));
        }
    }
}
