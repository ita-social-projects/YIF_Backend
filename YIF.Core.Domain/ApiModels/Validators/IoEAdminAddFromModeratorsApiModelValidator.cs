using System.Linq;
using System.Resources;
using FluentValidation;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class IoEAdminAddFromModeratorsApiModelValidator:AbstractValidator<IoEAdminAddFromModeratorsApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resourceManager;

        public IoEAdminAddFromModeratorsApiModelValidator(EFDbContext context, ResourceManager resourceManager)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            _context = context;
            _resourceManager = resourceManager;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.IoEId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.UserId)
                .Must(x => _context.InstitutionOfEducationModerators.Any(y => y.UserId == x))
                .WithMessage(_resourceManager.GetString("IoEModeratorNotFound"));

            RuleFor(x => x.IoEId)
                .Must(x => _context.InstitutionOfEducations.Any(y => y.Id == x))
                .WithMessage(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            RuleFor(x => x.IoEId)
                .Must(x => _context.InstitutionOfEducationAdmins
                    .Where(y => y.InstitutionOfEducationId == x)
                    .Any(z => z.IsDeleted == false))
                .WithMessage(_resourceManager.GetString("IoEAlreadyHasAnAdmin"));
        }
    }
}
