using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System;
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

        public LectorService(
            IDepartmentRepository<Department, DepartmentDTO> departmentRepository,
            ResourceManager resourceManager,
            IMapper mapper
        )
        {
            _departmentRepository = departmentRepository;
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
    }
}
