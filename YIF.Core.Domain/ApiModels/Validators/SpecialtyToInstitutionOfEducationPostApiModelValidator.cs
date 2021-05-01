using FluentValidation;
using System.Collections.Generic;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Domain.ApiModels.RequestApiModels;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class SpecialtyToInstitutionOfEducationPostApiModelValidator : AbstractValidator<List<SpecialtyToInstitutionOfEducationAddRangePostApiModel>>
    {
        private readonly EFDbContext _context;
        private readonly ResourceManager _resourceManager;
        public SpecialtyToInstitutionOfEducationPostApiModelValidator(EFDbContext context, ResourceManager resourceManager)
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            _context = context;
            _resourceManager = resourceManager;

            RuleForEach(x => x).SetValidator(new SpecialtyToInstitutionOfEducationPostApiModelValidatorCollection(_context, _resourceManager));
        }
    }
}
