using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyPutApiModelValidator : AbstractValidator<SpecialtyPutApiModel>
    {
        private readonly EFDbContext _context;
        private readonly string NotFoundInDbMessage = "Such {PropertyName} wasn't found in the database";
        public SpecialtyPutApiModelValidator(EFDbContext context) 
        {
            _context = context;
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.DirectionId)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Id)
                .Must(x => _context.Specialties.Any(y => y.Id == x))
                .WithMessage(NotFoundInDbMessage);

            RuleFor(x => x.DirectionId)
                .Must(x => _context.Directions.Any(y => y.Id == x))
                .WithMessage(NotFoundInDbMessage);
        }
    }
}
