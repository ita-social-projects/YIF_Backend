﻿using FluentValidation;
using System.Linq;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class DirectionPostApiModelValidator : AbstractValidator<DirectionPostApiModel>
    {
        private readonly EFDbContext _context;
        private readonly string AlreadyExistsInDbMessage = "Such {PropertyName} is exists in the database";
        private readonly string NotFoundInDbMessage = "Such {PropertyName} wasn't found in the database";
        public DirectionPostApiModelValidator(EFDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Description)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull();

            RuleFor(x => x.Name)
               .Must(x => _context.Directions.All(n => n.Name != x))
               .WithMessage(AlreadyExistsInDbMessage);

            RuleFor(x => x.Code)
                .Must(x => _context.Directions.All(n => n.Code != x))
                .WithMessage(AlreadyExistsInDbMessage);
        }
    }
}
