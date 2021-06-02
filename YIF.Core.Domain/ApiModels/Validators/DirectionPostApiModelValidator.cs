using FluentValidation;
using System.Linq;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class DirectionPostApiModelValidator : AbstractValidator<DirectionPostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resourceManager;
        public DirectionPostApiModelValidator(EFDbContext context, ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Name)
               .Must(x => _context.Directions.All(n => n.Name != x))
               .WithMessage(_resourceManager.GetString("AlreadyExistsInDbMessage"));
        }
    }
}
