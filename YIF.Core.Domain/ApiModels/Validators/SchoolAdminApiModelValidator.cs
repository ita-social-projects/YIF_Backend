using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using System.Resources;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SchoolAdminApiModelValidator : AbstractValidator<SchoolAdminApiModel>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resourceManager;


        public SchoolAdminApiModelValidator(EFDbContext context, ResourceManager resourceManager)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            
            _context = context;
            _resourceManager = resourceManager;

            RuleFor(x => x.SchoolName)
               .NotEmpty()
               .NotNull();

            RuleFor(x => x.Email)
               .NotEmpty()
               .NotNull();
            
            RuleFor(x => x.Password)
              .NotEmpty()
              .NotNull()
              .MinimumLength(6);

            RuleFor(x => x.Email)
              .Must(x => _context.Users.Any(z => z.Email == x))
              .WithMessage(_resourceManager.GetString("UserWithSuchEmailAlreadyExists"));

            RuleFor(x => x.SchoolName)
              .Must(x => _context.Schools.Any(z => z.Name == x))
              .WithMessage(_resourceManager.GetString("SchoolWithSuchNameNotFound"));
        }
    }
}