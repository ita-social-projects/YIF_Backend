using System.Resources;
using System.Threading.Tasks;
using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository<Department, DepartmentDTO> _departmentRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IMapper _mapper;

        public DepartmentService(
            IDepartmentRepository<Department, DepartmentDTO> departmentRepository,
            ResourceManager resourceManager,
            IMapper mapper
        )
        {
            _departmentRepository = departmentRepository;
            _resourceManager = resourceManager;
            _mapper = mapper;
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddDepartment(DepartmentApiModel departmentApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var departments = await _departmentRepository
                .Find(x => x.Name.Equals(departmentApiModel.Name) && x.Description.Equals(departmentApiModel.Description));

            if (departments != null)
            {
                throw new BadRequestException(_resourceManager.GetString("DepartmentAlreadyExist"));
            }

            var department = _mapper.Map<Department>(departmentApiModel);
            await _departmentRepository.Add(department);

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("DepartmentWasAdded")), true);
        }
    }
}
