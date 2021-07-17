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
using YIF.Core.Domain.ApiModels.Validators;
using YIF.Core.Data.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using YIF.Shared;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace YIF.Core.Service.Concrete.Services
{
    public class IoEAdminService : IIoEAdminService
    {
        private readonly IUserService<DbUser> _userService;
        private readonly UserManager<DbUser> _userManager;
        private readonly IUserRepository<DbUser, UserDTO> _userRepository;
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _ioERepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToIoERepository;
        private readonly ResourceManager _resourceManager;
        private readonly ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> _specialtyToIoEDescriptionRepository;
        private readonly IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> _examRequirementRepository;
        private readonly IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> _ioEModeratorRepository;
        private readonly ILectorRepository<Lector, LectorDTO> _lectorRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;
        private readonly IDeleteRepository _deleteRepository;
        private readonly IIoEBufferRepository<IoEBuffer, IoEBufferDTO> _ioEBufferRepository;

        public IoEAdminService(
            IUserService<DbUser> userService,
            UserManager<DbUser> userManager,
            IUserRepository<DbUser, UserDTO> userRepository,
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> ioERepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToIoERepository,
            IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO> institutionOfEducationAdminRepository,
            ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> specialtyToIoEDescriptionRepository,
            IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> examRequirementRepository,
            IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> ioEModeratorRepository,
            ILectorRepository<Lector, LectorDTO> lectorRepository,
            IMapper mapper,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ResourceManager resourceManager,
            IDeleteRepository deleteRepository,
            IIoEBufferRepository<IoEBuffer, IoEBufferDTO> ioEBufferRepository
        )
        {
            _userService = userService;
            _userManager = userManager;
            _userRepository = userRepository;
            _specialtyRepository = specialtyRepository;
            _ioERepository = ioERepository;
            _specialtyToIoERepository = specialtyToIoERepository;
            _institutionOfEducationAdminRepository = institutionOfEducationAdminRepository;
            _specialtyToIoEDescriptionRepository = specialtyToIoEDescriptionRepository;
            _examRequirementRepository = examRequirementRepository;
            _ioEModeratorRepository = ioEModeratorRepository;
            _lectorRepository = lectorRepository;
            _mapper = mapper;
            _resourceManager = resourceManager;
            _env = env;
            _configuration = configuration;
            _deleteRepository = deleteRepository;
            _ioEBufferRepository = ioEBufferRepository;
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

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyInstitution(string userId, InstitutionOfEducationPostApiModel institutionOfEducationPostApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            var validator = new InstitutionOfEducationPostApiModelValidator();
            var validResult = await validator.ValidateAsync(institutionOfEducationPostApiModel);

            if (!validResult.IsValid)
            {
                throw new BadRequestException(validResult.ToString());
            }

            string ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;
            var ioEBuffer = await _ioEBufferRepository.Get(ioEId);
            var ioEBufferDTO = _mapper.Map<IoEBufferDTO>(institutionOfEducationPostApiModel);
            var currentInstitutionOfEducationDTO = await _ioERepository.Get(ioEId);

            ioEBufferDTO.Id = ioEId;
            ioEBufferDTO.IoEStatus = IoEStatus.Modified;
            ioEBufferDTO.StartOfCampaign = currentInstitutionOfEducationDTO.StartOfCampaign;
            ioEBufferDTO.EndOfCampaign = currentInstitutionOfEducationDTO.EndOfCampaign;

            #region imageSaving
            if (institutionOfEducationPostApiModel.ImageApiModel != null)
            {
                var serverPath = _env.ContentRootPath;
                var folderName = _configuration.GetValue<string>("ImagesPath");
                var path = Path.Combine(serverPath, folderName);

                var fileName = ConvertImageApiModelToPath.FromBase64ToImageFilePath(institutionOfEducationPostApiModel.ImageApiModel.Photo, path);
                currentInstitutionOfEducationDTO.ImagePath = fileName;
            }
            #endregion

            if (ioEBuffer != null)
            {
                await _ioEBufferRepository.Update(_mapper.Map<IoEBuffer>(ioEBufferDTO));

                return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("InformationChanged")), true);
            }

            await _ioEBufferRepository.Add(_mapper.Map<IoEBuffer>(ioEBufferDTO));

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

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ChangeBannedStatusOfIoEModerator(string moderatorId, string userId)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var adminId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).Id;
            var ioEModerator = await _ioEModeratorRepository.GetByAdminId(moderatorId, adminId);
            if (ioEModerator == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("IoEModeratorNotExists")}: {moderatorId}");
            }
            string res;
            if (ioEModerator.IsBanned == false)
            {
                res = await _ioEModeratorRepository.Disable(_mapper.Map<InstitutionOfEducationModerator>(ioEModerator));
            }
            else
            {
                res = await _ioEModeratorRepository.Enable(_mapper.Map<InstitutionOfEducationModerator>(ioEModerator));
            }
            return result.Set(new DescriptionResponseApiModel(res), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteIoEModerator(string moderatorId, string userId)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var adminId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).Id;
            var moderatorDTO = await _ioEModeratorRepository.GetModeratorForAdmin(moderatorId, adminId);

            if (moderatorDTO == null)
                throw new NotFoundException(_resourceManager.GetString("IoEModeratorNotFoundForThisAdmin"));
            if (moderatorDTO.IsDeleted)
                throw new BadRequestException(_resourceManager.GetString("IoEModeratorWasAlreadyDeleted"));
            else
                await _deleteRepository.Delete(_mapper.Map<InstitutionOfEducationModerator>(moderatorDTO));

            var dbUser = await _userRepository.GetUserWithRoles(moderatorDTO.User.Id);
            await _userManager.RemoveFromRoleAsync(dbUser, ProjectRoles.InstitutionOfEducationModerator);

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("IoEModeratorIsDeleted")), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddIoEModerator( [NotNull] string moderatorEmail,
           [NotNull] string userId,
           [NotNull] HttpRequest request)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var dbUser = new DbUser
            {
                Email = moderatorEmail,
                UserName = moderatorEmail
            };
            var adminId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).Id;
            var searchUser = await _userManager.FindByEmailAsync(moderatorEmail);

            if (searchUser != null)
            {
                var ifUserAlreadyModerator = (await _ioEModeratorRepository.GetAll()).SingleOrDefault(x => x.UserId == searchUser.Id);

                if (ifUserAlreadyModerator != null)
                {
                    throw new BadRequestException(_resourceManager.GetString("IoEModeratorFailedUserAlreadyModerator"));
                }

                throw new BadRequestException(_resourceManager.GetString("UserWithSuchEmailAlreadyExists"));
            }

            var registerResult = await _userRepository.Create(dbUser, null, null, ProjectRoles.InstitutionOfEducationModerator);

            if (registerResult != string.Empty)
            {
                throw new BadRequestException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
            }

            var resultResetPasswordByEmail = await _userService.ResetPasswordByEmail(moderatorEmail, request);

            if (!resultResetPasswordByEmail.Success)
            {
                throw new BadRequestException($"{_resourceManager.GetString("ResetPasswordByEmailFailed")}: {resultResetPasswordByEmail.Message}");
            }

            await _ioEModeratorRepository.AddUniModerator(new InstitutionOfEducationModerator { AdminId = adminId, UserId = dbUser.Id });

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("IoEModeratorAdded")), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddLectorToIoE(
            [NotNull] string userId,
            [NotNull] EmailApiModel email,
            [NotNull] HttpRequest request)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var dbUser = new DbUser
            {
                Email = email.UserEmail,
                UserName = email.UserEmail
            };

            var ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;
            var searchUser = await _userManager.FindByEmailAsync(email.UserEmail);
            if (searchUser == null)
            {
                var registerResult = await _userRepository.Create(dbUser, null, null, ProjectRoles.Lector);
                if (registerResult != string.Empty)
                {
                    throw new BadRequestException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
                }

                var resultResetPasswordByEmail = await _userService.ResetPasswordByEmail(email.UserEmail, request);
                if (!resultResetPasswordByEmail.Success)
                {
                    throw new BadRequestException($"{_resourceManager.GetString("ResetPasswordByEmailFailed")}: {resultResetPasswordByEmail.Message}");
                }
            }

            else
            {
                var lectorExist = await _lectorRepository.GetLectorByUserAndIoEIds(searchUser.Id, ioEId);
                if (lectorExist != null)
                {
                    throw new BadRequestException(_resourceManager.GetString("IoEAlreadyHasLector"));
                }
                throw new BadRequestException(_resourceManager.GetString("UserAlreadyExists"));
            }

            var newLector = new Lector { InstitutionOfEducationId = ioEId, UserId = dbUser.Id };
            await _lectorRepository.Add(newLector);

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("LectorWasAdded")), true);
        }

        public async Task<ResponseApiModel<IEnumerable<LectorResponseApiModel>>> GetIoELectorsByUserId(string userId)
        {
            string ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;

            return new ResponseApiModel<IEnumerable<LectorResponseApiModel>>
            {
                Object = _mapper.Map<IEnumerable<LectorResponseApiModel>>(await _lectorRepository.GetLectorsByIoEId(ioEId)),
                Success = true
            };
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteIoELector(string lectorId, string userId)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            string ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;
            var lector = await _lectorRepository.GetLectorInIoE(lectorId, ioEId);

            if (lector == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("IoELectorWithSuchIdNotFound")}: {lectorId}");
            }

            else if( lector.IsDeleted == true)
            {
                throw new BadRequestException(_resourceManager.GetString("IoELectorWasAlreadyDeleted"));
            }

            var searchUser = await _userManager.FindByIdAsync(lector.UserId);
            await _lectorRepository.Delete(lector.Id);
            await _userManager.RemoveFromRoleAsync(searchUser, ProjectRoles.Lector);
            await _userRepository.Delete(searchUser.Id);
            
            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("IoELectorIsDeleted")), true);
        }
    }
}
