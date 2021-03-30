using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
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
    public class InstitutionAdminService : IInstitutionAdminService
    {
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _institutionOfEducationRepository;
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public InstitutionAdminService(
                        IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> institutionOfEducationRepository,
                        IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> institutionOfEducationAdminRepository,
                        ResourceManager resourceManager,
                        IMapper mapper,
                        IWebHostEnvironment env,
                        IConfiguration configuration)
        {
            _institutionOfEducationRepository = institutionOfEducationRepository;
            _institutionOfEducationAdminRepository = institutionOfEducationAdminRepository;
            _resourceManager = resourceManager;
            _mapper = mapper;
            _env = env;
            _configuration = configuration;
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyDescriptionOfInstitution(string userId, InstitutionOfEducationPostApiModel institutionOfEducationPostApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var admins = await _institutionOfEducationAdminRepository.GetAllUniAdmins();
            var admin = admins.SingleOrDefault(x => x.UserId == userId);

            if (admin == null)
            {                
                return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("AdminWithSuchIdNotFound")), false);
            }

            var institutionOfEducationDTONew = _mapper.Map<InstitutionOfEducationDTO>(institutionOfEducationPostApiModel);

            #region imageSaving
            if (institutionOfEducationPostApiModel.ImageApiModel != null)
            {
                var serverPath = _env.ContentRootPath;
                var folerName = _configuration.GetValue<string>("ImagesPath");
                var path = Path.Combine(serverPath, folerName);

                var fileName = ConvertImageApiModelToPath.FromBase64ToImageFilePath(institutionOfEducationPostApiModel.ImageApiModel.Photo, path);
                institutionOfEducationDTONew.ImagePath = fileName;
            }
            #endregion

            institutionOfEducationDTONew.Id = admin.InstitutionOfEducationId;

            await _institutionOfEducationRepository.Update(_mapper.Map<InstitutionOfEducation>(institutionOfEducationDTONew));

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("InformationChanged")), true);
        }
    }
}
