using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.EntityDTO;
using AutoMapper;
using System.Resources;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Service.Concrete.Services.ValidatorServices;
using SendGrid.Helpers.Errors.Model;

namespace YIF.Core.Service.Concrete.Services
{
    public class DisciplineService: IDisciplineService
    {
        private IDisciplineRepository<Discipline, DisciplineDTO> _disciplineRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IMapper _mapper;
        private readonly DisciplineValidator _disciplineValidator;

        public DisciplineService(
            IDisciplineRepository<Discipline, DisciplineDTO> disciplineRepository,
            ResourceManager resourceManager,
            IMapper mapper
        )
        {
            _disciplineRepository = disciplineRepository;
            _resourceManager = resourceManager;
            _disciplineValidator = new DisciplineValidator(_disciplineRepository);
            _mapper = mapper;
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddDiscipline(DisciplinePostApiModel disciplineApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var validResults = _disciplineValidator.Validate(disciplineApiModel);
            if (!validResults.IsValid)
            {
                throw new BadRequestException(validResults.ToString());
            }

            var discipline = _mapper.Map<Discipline>(disciplineApiModel);
            await _disciplineRepository.Add(discipline);

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("DisciplinetWasAdded")), true);
        }
    }
}
