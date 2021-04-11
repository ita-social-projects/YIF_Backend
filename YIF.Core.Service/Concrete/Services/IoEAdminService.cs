using System.Resources;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using SendGrid.Helpers.Errors.Model;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class IoEAdminService : IIoEAdminService
    {
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _ioERepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToIoERepository;
        private readonly ResourceManager _resourceManager;
        private readonly ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> _specialtyToIoEDescriptionRepository;
        private readonly IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> _directionToIoERepository;
        private readonly IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> _examRequirementRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;

        public IoEAdminService(
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> ioERepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToIoERepository,
            IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> directionToIoERepository,
            IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> institutionOfEducationAdminRepository,
            ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> specialtyToIoEDescriptionRepository,
            IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> examRequirementRepository,
            IMapper mapper,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ResourceManager resourceManager)
        {
            _specialtyRepository = specialtyRepository;
            _ioERepository = ioERepository;
            _specialtyToIoERepository = specialtyToIoERepository;
            _institutionOfEducationAdminRepository = institutionOfEducationAdminRepository;
            _directionToIoERepository = directionToIoERepository;
            _specialtyToIoEDescriptionRepository = specialtyToIoEDescriptionRepository;
            _examRequirementRepository = examRequirementRepository;
            _mapper = mapper;
            _resourceManager = resourceManager;
            _env = env;
            _configuration = configuration;
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToIoe(
            SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            var specialty = await _specialtyRepository.ContainsById(specialtyToIoE.SpecialtyId);
            var institutionOfEducation = await _ioERepository.ContainsById(specialtyToIoE.InstitutionOfEducationId);
            var entity = new SpecialtyToInstitutionOfEducation()
            {
                SpecialtyId = specialtyToIoE.SpecialtyId,
                InstitutionOfEducationId = specialtyToIoE.InstitutionOfEducationId
            };

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));

            await _specialtyToIoERepository.AddSpecialty(entity);
            return result.Set(new DescriptionResponseApiModel("Specialty was successfully added to the Institution of Education"), true);
        }

        public async Task DeleteSpecialtyToIoe(SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE)
        {
            var specialty = await _specialtyRepository.ContainsById(specialtyToIoE.SpecialtyId);
            var institutionOfEducation = await _ioERepository.ContainsById(specialtyToIoE.InstitutionOfEducationId);
            var entity = await _specialtyToIoERepository.Find(s => s.SpecialtyId == specialtyToIoE.SpecialtyId && s.InstitutionOfEducationId == specialtyToIoE.InstitutionOfEducationId);

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));

            if (entity == null)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyInInstitutionOfEducationNotFound"));

            await _specialtyToIoERepository.Update(new SpecialtyToInstitutionOfEducation
            {
                SpecialtyId = specialtyToIoE.SpecialtyId,
                InstitutionOfEducationId = specialtyToIoE.InstitutionOfEducationId,
                IsDeleted = true
            }); ;
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

            await _ioERepository.Update(_mapper.Map<InstitutionOfEducation>(institutionOfEducationDTONew));

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("InformationChanged")), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> UpdateSpecialtyDescription(SpecialtyDescriptionUpdateApiModel specialtyDescriptionUpdateApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var specialtyToIoEDescriptionDTO = _mapper.Map<SpecialtyToIoEDescriptionDTO>(specialtyDescriptionUpdateApiModel);

            await _examRequirementRepository.DeleteRangeByDescriptionId(specialtyDescriptionUpdateApiModel.Id);

            return result.Set(
                new DescriptionResponseApiModel(_resourceManager.GetString("SpecialtyDescriptionUpdated")),
                await _specialtyToIoEDescriptionRepository.Update(_mapper.Map<SpecialtyToIoEDescription>(specialtyToIoEDescriptionDTO)));
        }

        public async Task<IEnumerable<DirectionToIoEResponseApiModel>> GetAllDirectionsAndSpecialitiesOfAdmin(string adminId)
        {
            var admin = await _institutionOfEducationAdminRepository.GetById(adminId);

            if (admin == null)
                throw new BadRequestException(_resourceManager.GetString("IoEAdminNotFound"));

            var institutionOfEducation = await _ioERepository.ContainsById(admin.InstitutionOfEducationId);

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            var ioEdirections = await _directionToIoERepository.Find(x => x.InstitutionOfEducationId == admin.InstitutionOfEducationId);
            var response = _mapper.Map<IEnumerable<DirectionToIoEResponseApiModel>>(ioEdirections);
            foreach (DirectionToIoEResponseApiModel responseApiModel in response)
            {
                responseApiModel.Specialties = _mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>>
                    (await _specialtyToIoERepository.Find(s => s.Specialty.DirectionId == responseApiModel.Id && s.InstitutionOfEducationId == admin.InstitutionOfEducationId));
            }
            return response;
        }
    }
}
