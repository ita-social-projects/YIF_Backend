using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ApiModels.Validators;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class LectorService : ILectorService
    {
        private readonly IMapper _mapper; 
        private readonly ResourceManager _resourceManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly ILectorRepository<Lector, LectorDTO> _lectorRepository;
        private IDepartmentRepository<Department, DepartmentDTO> _departmentRepository;


        public LectorService(IMapper mapper, 
            ResourceManager resourceManager, 
            IWebHostEnvironment env, 
            IConfiguration configuration, 
            ILectorRepository<Lector, LectorDTO> lectorRepository,
            IDepartmentRepository<Department, DepartmentDTO> departmentRepository) 
        {
            _mapper = mapper;
            _resourceManager = resourceManager;
            _env = env;
            _configuration = configuration;
            _lectorRepository = lectorRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyLector(string userId, JsonPatchDocument<LectorApiModel> lectorApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            LectorApiModel request = new LectorApiModel();
            lectorApiModel.ApplyTo(request);

            var validator = new LectorApiModelValidator();
            var validResult = await validator.ValidateAsync(request);

            if (!validResult.IsValid)
            {
                throw new BadRequestException(validResult.ToString());
            }

            var lectorDTO = await _lectorRepository.GetLectorByUserId(userId);
            var newLectorDTO = _mapper.Map<LectorDTO>(request);
            newLectorDTO.Id = lectorDTO.Id;
            newLectorDTO.User.Id = lectorDTO.User.Id;
            newLectorDTO.UserId = lectorDTO.UserId;

            #region imageSaving
            if (request.ImageApiModel != null)
            {
                var serverPath = _env.ContentRootPath;
                var folderName = _configuration.GetValue<string>("ImagesPath");
                var path = Path.Combine(serverPath, folderName);

                var fileName = ConvertImageApiModelToPath.FromBase64ToImageFilePath(request.ImageApiModel.Photo, path);
                newLectorDTO.ImagePath = fileName;
            }
            #endregion

            await _lectorRepository.Update(_mapper.Map<Lector>(newLectorDTO));
           
            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("InformationChanged")), true);
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
