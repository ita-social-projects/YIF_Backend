using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.EntityDTO;
using AutoMapper;
using System.Resources;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using SendGrid.Helpers.Errors.Model;
using YIF.Core.Domain.ApiModels.Validators;

namespace YIF.Core.Service.Concrete.Services
{
    public class DisciplineService: IDisciplineService
    {
        private IDisciplineRepository<Discipline, DisciplineDTO> _disciplineRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IMapper _mapper;
        

        public DisciplineService(
            IDisciplineRepository<Discipline, DisciplineDTO> disciplineRepository,
            ResourceManager resourceManager,
            IMapper mapper
        )
        {
            _disciplineRepository = disciplineRepository;
            _resourceManager = resourceManager;
            _mapper = mapper;
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddDiscipline(DisciplinePostApiModel disciplinePostApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            
            var discipline = _mapper.Map<Discipline>(disciplinePostApiModel);
            var disciplines = Task.FromResult(_disciplineRepository
                .Find(x => x.Name.Equals(discipline.Name))).Result.Result;
            if(disciplines != null)
            {
                throw new BadRequestException(_resourceManager.GetString("DisciplineAlreadyExist"));
            }

            await _disciplineRepository.Add(discipline);

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("DisciplineWasAdded")), true);
        }
    }
}
