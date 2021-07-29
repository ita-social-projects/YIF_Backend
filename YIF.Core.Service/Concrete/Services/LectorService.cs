using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class LectorService : ILectorService
    {
        private IDepartmentRepository<Department, DepartmentDTO> _departmentRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IMapper _mapper;
        private readonly IDisciplineRepository<Discipline, DisciplineDTO> _disciplineRepository;

        public LectorService(
            IDepartmentRepository<Department, DepartmentDTO> departmentRepository,
            IDisciplineRepository<Discipline, DisciplineDTO> disciplineRepository,
            ResourceManager resourceManager,
            IMapper mapper
        )
        {
            _departmentRepository = departmentRepository;
            _disciplineRepository = disciplineRepository;
            _resourceManager = resourceManager;
            _mapper = mapper;
        }

        public async Task<ResponseApiModel<IEnumerable<DepartmentApiModel>>> GetAllDepartments()
        {
            var result = new ResponseApiModel<IEnumerable<DepartmentApiModel>>();
            var departments = await _departmentRepository.GetAll();
            if (departments.Count() == 0)
            {
                throw new NotFoundException(_resourceManager.GetString("DepartmentsNotFound"));
            }
            result.Object = _mapper.Map<IEnumerable<DepartmentApiModel>>(departments);
            return result.Set(true);
        }

        public async Task<ResponseApiModel<IEnumerable<DisciplinePostApiModel>>> GetAllDisciplines()
        {
            var result = new ResponseApiModel<IEnumerable<DisciplinePostApiModel>>();
            var disciplines = await _disciplineRepository.GetAll();
            if(disciplines.Count() == 0)
            {
                throw new NotFoundException(_resourceManager.GetString("DisciplinesNotFound"));
            }
            result.Object = _mapper.Map<IEnumerable<DisciplinePostApiModel>>(disciplines);
            return result.Set(true);
        }
    }
}
