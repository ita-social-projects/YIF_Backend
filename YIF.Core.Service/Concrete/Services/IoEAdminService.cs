using System.Resources;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
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
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using YIF.Core.Domain.ApiModels.Validators;
using YIF.Core.Data.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using YIF.Shared;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF.Core.Service.Concrete.Services
{
    public class IoEAdminService : IIoEAdminService
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly IUserRepository<DbUser, UserDTO> _userRepository;
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _ioERepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToIoERepository;
        private readonly ResourceManager _resourceManager;
        private readonly ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> _specialtyToIoEDescriptionRepository;
        private readonly IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> _examRequirementRepository;
        private readonly IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> _ioEModeratorRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;

        public IoEAdminService(
            UserManager<DbUser> userManager,
            IUserRepository<DbUser, UserDTO> userRepository,
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> ioERepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToIoERepository,
            IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO> institutionOfEducationAdminRepository,
            ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> specialtyToIoEDescriptionRepository,
            IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> examRequirementRepository,
            IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> ioEModeratorRepository,
            IMapper mapper,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ResourceManager resourceManager
        )
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _specialtyRepository = specialtyRepository;
            _ioERepository = ioERepository;
            _specialtyToIoERepository = specialtyToIoERepository;
            _institutionOfEducationAdminRepository = institutionOfEducationAdminRepository;
            _specialtyToIoEDescriptionRepository = specialtyToIoEDescriptionRepository;
            _examRequirementRepository = examRequirementRepository;
            _ioEModeratorRepository = ioEModeratorRepository;
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

            var specialtyToInstitutionOfEducationDTO = _mapper.Map<SpecialtyToInstitutionOfEducationDTO>(specialtyToIoE);
            var specialtyToInstitutionOfEducation = _mapper.Map<SpecialtyToInstitutionOfEducation>(specialtyToInstitutionOfEducationDTO);

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));

            await _specialtyToIoERepository.AddSpecialty(specialtyToInstitutionOfEducation);
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

            var specialtyToInstitutionOfEducationDTO = _mapper.Map<SpecialtyToInstitutionOfEducationDTO>(specialtyToIoE);
            var specialtyToInstitutionOfEducation = _mapper.Map<SpecialtyToInstitutionOfEducation>(specialtyToInstitutionOfEducationDTO);
            specialtyToInstitutionOfEducation.IsDeleted = true;

            await _specialtyToIoERepository.Update(specialtyToInstitutionOfEducation);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyDescriptionOfInstitution(string userId, JsonPatchDocument<InstitutionOfEducationPostApiModel> institutionOfEducationPostApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            InstitutionOfEducationPostApiModel request = new InstitutionOfEducationPostApiModel();
            institutionOfEducationPostApiModel.ApplyTo(request);

            var validator = new InstitutionOfEducationPostApiModelValidator();
            var validResult = await validator.ValidateAsync(request);
            if (!validResult.IsValid)
                throw new BadRequestException(validResult.ToString());

            string ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;
            var currentInstitutionOfEducationDTO = await _ioERepository.Get(ioEId);

            var newInstitutionOfEducationDTO = _mapper.Map<JsonPatchDocument<InstitutionOfEducationDTO>>(institutionOfEducationPostApiModel);

            newInstitutionOfEducationDTO.ApplyTo(currentInstitutionOfEducationDTO);

            #region imageSaving
            if (request.ImageApiModel != null)
            {
                var serverPath = _env.ContentRootPath;
                var folderName = _configuration.GetValue<string>("ImagesPath");
                var path = Path.Combine(serverPath, folderName);

                var fileName = ConvertImageApiModelToPath.FromBase64ToImageFilePath(request.ImageApiModel.Photo, path);
                currentInstitutionOfEducationDTO.ImagePath = fileName;
            }
            #endregion

            await _ioERepository.Update(_mapper.Map<InstitutionOfEducation>(currentInstitutionOfEducationDTO));

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

        public async Task<ResponseApiModel<IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>>> GetIoEModeratorsByUserId(string userId)
        {
            string ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;

            return new ResponseApiModel<IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>>
            {
                Object = _mapper.Map<IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>>(await _ioEModeratorRepository.GetByIoEId(ioEId)),
                Success = true
            };
        }

        public async Task<ResponseApiModel<IoEInformationResponseApiModel>> GetIoEInfoByUserId(string userId) 
        {
            string ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;

            return new ResponseApiModel<IoEInformationResponseApiModel> 
            { 
               Object = _mapper.Map<IoEInformationResponseApiModel>(await _ioERepository.Get(ioEId)),
               Success = true
            };
        }

        public async Task<ResponseApiModel<SpecialtyToInstitutionOfEducationResponseApiModel>> GetSpecialtyToIoEDescription(string userId, string specialtyId)
        {
            var specialty = await _specialtyRepository.ContainsById(specialtyId);
            var institutionOfEducationId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;
            var institutionOfEducation = await _ioERepository.ContainsById(institutionOfEducationId);
            var specialtyToIoE = await _specialtyToIoERepository
                .Find(s => s.SpecialtyId == specialtyId && s.InstitutionOfEducationId == institutionOfEducationId);

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new NotFoundException(_resourceManager.GetString("SpecialtyNotFound"));

            if (specialtyToIoE.Count() == 0)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyInInstitutionOfEducationNotFound"));

            return new ResponseApiModel<SpecialtyToInstitutionOfEducationResponseApiModel>
            {
                Object = _mapper.Map<SpecialtyToInstitutionOfEducationResponseApiModel>(specialtyToIoE.FirstOrDefault()),
                Success = true
            };
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteIoEModerator(string moderatorId, string userId)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var adminId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).Id;
            var moderator = await _ioEModeratorRepository.GetModeratorForAdmin(moderatorId, adminId);

            if (moderator == null)
                throw new NotFoundException(_resourceManager.GetString("IoEModeratorNotFoundForThisAdmin"));
            if (moderator.IsDeleted)
                throw new BadRequestException(_resourceManager.GetString("IoEModeratorWasAlreadyDeleted"));
            else
                await _ioEModeratorRepository.Delete(moderatorId);

                var dbUser = await _userRepository.GetUserWithRoles(moderator.User.Id);
                await _userManager.RemoveFromRoleAsync(dbUser, ProjectRoles.InstitutionOfEducationModerator);

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("IoEModeratorIsDeleted")), true);
        }
    }
}