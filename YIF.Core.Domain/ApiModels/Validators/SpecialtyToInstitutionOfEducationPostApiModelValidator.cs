using FluentValidation;
using System.Collections.Generic;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyToInstitutionOfEducationPostApiModelValidator : AbstractValidator<List<SpecialtyToInstitutionOfEducationPostApiModel>>
    {
        private readonly EFDbContext _context;
        public SpecialtyToInstitutionOfEducationPostApiModelValidator(EFDbContext context)
        {
            _context = context;

            RuleForEach(x => x).SetValidator(new SpecialtyToInstitutionOfEducationPostApiModelValidatorCollection(_context));
        }
    }
}
