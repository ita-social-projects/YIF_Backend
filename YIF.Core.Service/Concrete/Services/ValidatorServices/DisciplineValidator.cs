using FluentValidation;
using Microsoft.AspNetCore.Identity;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Data.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace YIF.Core.Service.Concrete.Services.ValidatorServices
{
    public class DisciplineValidator : AbstractValidator<DisciplinePostApiModel>
    {
        private readonly IDisciplineRepository<Discipline, DisciplineDTO> _disciplineRepository;
        public DisciplineValidator(IDisciplineRepository<Discipline, DisciplineDTO> disciplineRepository)
        {
            _disciplineRepository = disciplineRepository;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotNull().NotEmpty().WithMessage("Назва дисципліни повинна є обовязковою!");

            RuleFor(x => x.Description)
                .NotNull().NotEmpty().WithMessage("Опис дисципліни є обовязковим!");

            RuleFor(x => x.LectorId)
                .NotNull().NotEmpty().WithMessage("Id лектора є обовязковим!");

            RuleFor(x => x.SpecialityId)
                .NotNull().NotEmpty().WithMessage("Id спеціальності є обовязковим!");

            RuleFor(x => x)
                .Must(DisciplineNotExist).WithMessage("Така дисципліна вже існує");
        }

        private bool DisciplineNotExist(DisciplinePostApiModel discipline)
        {
            var disciplines = Task.FromResult(_disciplineRepository
               .Find(x => x.Name.Equals(discipline.Name) || x.Description.Equals(discipline.Description))).Result.Result;
            return disciplines == null;
        }
    }
}
