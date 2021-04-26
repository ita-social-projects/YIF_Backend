using FluentValidation;
using System.Collections.Generic;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyToInstitutionOfEducationPostApiModelValidator : AbstractValidator<List<SpecialtyToInstitutionOfEducationPostApiModel>>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resouseManager;
        public SpecialtyToInstitutionOfEducationPostApiModelValidator(EFDbContext context, ResourceManager resouseManager)
        {
            _context = context;
            _resouseManager = resouseManager;

            RuleForEach(x => x).SetValidator(new SpecialtyToInstitutionOfEducationPostApiModelValidatorCollection(_context, _resouseManager));
        }
    }
}
