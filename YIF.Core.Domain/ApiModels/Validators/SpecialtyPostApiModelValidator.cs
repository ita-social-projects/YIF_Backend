using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyPostApiModelValidator : AbstractValidator<SpecialtyPostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly string AlreadyExistsInDbMessage = "Such {PropertyName} is exists in the database";
        private readonly string NotFoundInDbMessage = "Such {PropertyName} wasn't found in the database";
        public SpecialtyPostApiModelValidator(EFDbContext context)
        {
            _context = context;
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

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

            RuleFor(x => x.DirectionId)
                .Must(x => _context.Directions.Any(y => y.Id == x))
                .WithMessage(NotFoundInDbMessage);

            RuleFor(x => x.Name)
                .Must(x => _context.Specialties.Any(y => y.Name == x))
                .WithMessage(AlreadyExistsInDbMessage);

            RuleFor(x => x.Code)
                .Must(x => _context.Specialties.Any(y => y.Code == x))
                .WithMessage(AlreadyExistsInDbMessage);
        }
    }
}
