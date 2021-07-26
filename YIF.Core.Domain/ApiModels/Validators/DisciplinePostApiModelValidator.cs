using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using System.Resources;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class DisciplinePostApiModelValidator : AbstractValidator<DisciplinePostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resourceManager;
        public DisciplinePostApiModelValidator(EFDbContext context, ResourceManager resourceManager)
        {
            _context = context;
            _resourceManager = resourceManager;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.LectorId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.SpecialityId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Name)
                .Must(x => _context.Disciplines.All(n => n.Name != x))
                .WithMessage(_resourceManager.GetString("DisciplineAlreadyExist"));
            
        }
    }
}
