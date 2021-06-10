using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ApiModels.Validators;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Shared;

namespace YIF.Core.Service.Concrete.Services
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly IUserService<DbUser> _userService;
        private readonly IUserRepository<DbUser, UserDTO> _userRepository;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IDirectionRepository<Direction, DirectionDTO> _directionRepository;
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _institutionOfEducationRepository;
        private readonly ISchoolRepository<SchoolDTO> _schoolRepository;
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly ISchoolAdminRepository<SchoolAdminDTO> _schoolAdminRepository;
        private readonly ISchoolModeratorRepository<SchoolModeratorDTO> _schoolModeratorRepository;
        private readonly IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> _ioEModeratorRepository;
        private readonly ITokenRepository<TokenDTO> _tokenRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IPaginationService _paginationService;

        public SuperAdminService(
            IUserService<DbUser> userService,
            IUserRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IDirectionRepository<Direction, DirectionDTO> directionRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> institutionOfEducationRepository,
            IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO> institutionOfEducationAdminRepository,
            ISchoolRepository<SchoolDTO> schoolRepository,
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            ISchoolAdminRepository<SchoolAdminDTO> schoolAdminRepository,
            ISchoolModeratorRepository<SchoolModeratorDTO> schoolModeratorRepository,
            IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> ioEModeratorRepository,
            ITokenRepository<TokenDTO> tokenRepository,
            ResourceManager resourceManager, 
            IWebHostEnvironment env,
            IConfiguration configuration,
            IPaginationService paginationService)
        {
            _userService = userService;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
            _directionRepository = directionRepository;
            _institutionOfEducationAdminRepository = institutionOfEducationAdminRepository;
            _institutionOfEducationRepository = institutionOfEducationRepository;
            _schoolRepository = schoolRepository;
            _specialtyRepository = specialtyRepository;
            _schoolAdminRepository = schoolAdminRepository;
            _schoolModeratorRepository = schoolModeratorRepository;
            _ioEModeratorRepository = ioEModeratorRepository;
            _tokenRepository = tokenRepository;
            _resourceManager = resourceManager;
            _env = env;
            _configuration = configuration;
            _paginationService = paginationService;
            _specialtyRepository = specialtyRepository;
        }

        ///<inheritdoc/>
        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddInstitutionOfEducationAdmin(
            [NotNull] string InstitutionOfEducationId,
            [NotNull] string AdminEmail,
            [NotNull] HttpRequest request)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var dbUser = new DbUser
            {
                Email = AdminEmail,
                UserName = AdminEmail
            };

            #region Check
            var institutionOfEducation = await _institutionOfEducationRepository.ContainsById(InstitutionOfEducationId);
            if(!institutionOfEducation)
            {
                throw new NotFoundException(_resourceManager.GetString("InstitutionOfEducationNotFound"));
            }

            var admin = await _institutionOfEducationAdminRepository.GetByInstitutionOfEducationIdWithoutIsDeletedCheck(InstitutionOfEducationId);
            if (admin != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("InstitutionOfEducationAdminFailed"));
            }
            #endregion

            var searchUser = _userManager.FindByEmailAsync(AdminEmail);
            if (searchUser.Result == null)
            {
                var registerResult = await _userRepository.Create(dbUser, null, null, ProjectRoles.InstitutionOfEducationAdmin);
                if (registerResult != string.Empty)
                {
                    throw new InvalidOperationException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
                }

                var resultResetPasswordByEmail = await _userService.ResetPasswordByEmail(AdminEmail, request);
                if (!resultResetPasswordByEmail.Success)
                {
                    throw new InvalidOperationException($"{_resourceManager.GetString("ResetPasswordByEmailFailed")}: {resultResetPasswordByEmail.Message}");
                }
            }
            else 
            {
                var ifUserAlreadyAdmin = (await _institutionOfEducationAdminRepository.GetAllUniAdmins()).SingleOrDefault(x => x.UserId == searchUser.Result.Id);
                if (ifUserAlreadyAdmin != null)
                {
                    throw new InvalidOperationException(_resourceManager.GetString("InstitutionOfEducationAdminFailedUserAlreadyAdmin"));
                }
                
                dbUser = searchUser.Result;
            }

            await _institutionOfEducationAdminRepository.AddUniAdmin(new InstitutionOfEducationAdmin { InstitutionOfEducationId = InstitutionOfEducationId, UserId = dbUser.Id });

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("AdminAdded")), true);
        }

        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> AddSchoolAdmin(SchoolAdminApiModel schoolAdminModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            //take uni
            var school = await _schoolRepository.GetByName(schoolAdminModel.SchoolName);
            if (school == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("SchoolWithSuchNameNotFound")}: {schoolAdminModel.SchoolName}");
            }

            var adminCheck = await _schoolAdminRepository.GetBySchoolId(school.Id);// check if exists schoolAdmin with AspNetUser isdeleted = false
            if (adminCheck != null)
            {                
                throw new InvalidOperationException(_resourceManager.GetString("AdministratorOfThisSchoolAlreadyExists"));
            }

            var searchUser = _userManager.FindByEmailAsync(schoolAdminModel.Email);
            if (searchUser.Result != null && searchUser.Result.IsDeleted == true)
            {
                throw new InvalidOperationException(_resourceManager.GetString("UserAlreadyExists"));
            }
            var dbUser = new DbUser
            {
                Email = schoolAdminModel.Email,
                UserName = schoolAdminModel.Email
            };

            var registerResult = await _userRepository.Create(dbUser, null, schoolAdminModel.Password, ProjectRoles.SchoolAdmin);
            if (registerResult != string.Empty)
            {
                throw new InvalidOperationException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
            }


            await _schoolAdminRepository.AddSchoolAdmin(new SchoolAdmin { SchoolId = school.Id });
            var admin = await _schoolAdminRepository.GetBySchoolIdWithoutIsDeletedCheck(school.Id);

            SchoolModerator toAdd = new SchoolModerator
            {
                SchoolId = school.Id,
                UserId = dbUser.Id,
                AdminId = admin.Id
            };
            await _schoolModeratorRepository.AddSchoolModerator(toAdd);


            //to do change logic and response model


            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            var tokenDb = await _tokenRepository.FindUserToken(dbUser.Id);
            if (tokenDb == null)
            {
                tokenDb = new TokenDTO
                {
                    Id = dbUser.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                };
                await _tokenRepository.AddUserToken(tokenDb);
            }
            else await _tokenRepository.UpdateUserToken(dbUser.Id, refreshToken);

            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            return result.Set(new AuthenticateResponseApiModel(token, refreshToken), true);
        }
        
        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteInstitutionOfEducationAdmin(string adminId)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var admin = await _institutionOfEducationAdminRepository.GetUserByAdminId(adminId);
            if (admin == null)
            {
                throw new NotFoundException(_resourceManager.GetString("IoEAdminNotFound"));
            }

            var searchUser = await _userManager.FindByIdAsync(admin.UserId);
            
            await _institutionOfEducationAdminRepository.Delete(adminId);
            await _userManager.RemoveFromRoleAsync(searchUser, ProjectRoles.InstitutionOfEducationAdmin);
            await _userRepository.Delete(searchUser.Id);
            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("IoEAdminDeleted")), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteInstitutionOfEducation(string id)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var ch = await _institutionOfEducationRepository.Get(id);
            if (ch == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("InstitutionOfEducationWithSuchIdNotFound")}: {id}");
            }
            await _institutionOfEducationRepository.Delete(ch.Id);
            return result.Set(new DescriptionResponseApiModel("IoEIsDeleted"), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DisableInstitutionOfEducationAdmin(string adminId)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var ch = await _institutionOfEducationAdminRepository.GetUserByAdminId(adminId);
            if (ch == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("UserWithSuchIdNotFound")}: {adminId}");
            }
            string res;
            if (ch.IsBanned == false)
            {
                res = await _institutionOfEducationAdminRepository.Disable(_mapper.Map<InstitutionOfEducationAdmin>(ch));
            }
            else
            {
                res = await _institutionOfEducationAdminRepository.Enable(_mapper.Map<InstitutionOfEducationAdmin>(ch));
            }
            return result.Set(new DescriptionResponseApiModel(res), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteSchoolAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi)
        {            
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            string ch = await _schoolAdminRepository.Delete(schoolUniAdminDeleteApi.Id);
            if (ch == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("UserWithSuchIdNotFound")}: {schoolUniAdminDeleteApi.Id}");
            }
            return result.Set(new DescriptionResponseApiModel(ch), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddInstitutionOfEducationAndAdmin(
            InstitutionOfEducationCreatePostApiModel institutionOfEducationPostApiModel, 
            HttpRequest request)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            var ch = await _institutionOfEducationRepository.GetByName(institutionOfEducationPostApiModel.Name);
            if (ch != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("InstitutionOfEducationWithSuchNameAlreadyExists"));
            }

            var ifUserAlreadyAdmin = (await _institutionOfEducationAdminRepository.GetAllUniAdmins()).SingleOrDefault(x => x.User.Email == institutionOfEducationPostApiModel.InstitutionOfEducationAdminEmail);
            if (ifUserAlreadyAdmin != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("InstitutionOfEducationAdminFailedUserAlreadyAdmin"));
            }

            #region imageSaving
            var serverPath = _env.ContentRootPath;
            var folerName = _configuration.GetValue<string>("ImagesPath");
            var path = Path.Combine(serverPath, folerName);

            var fileName = ConvertImageApiModelToPath.FromBase64ToImageFilePath(institutionOfEducationPostApiModel.ImageApiModel.Photo, path);
            #endregion

            var institutionOfEducationDTO = _mapper.Map<InstitutionOfEducationDTO>(institutionOfEducationPostApiModel);
            institutionOfEducationDTO.ImagePath = fileName;

            var institutionOfEducation = await _institutionOfEducationRepository.AddInstitutionOfEducation(_mapper.Map<InstitutionOfEducation>(institutionOfEducationDTO));

            var addingAdmin = await AddInstitutionOfEducationAdmin(institutionOfEducation.Id, institutionOfEducationPostApiModel.InstitutionOfEducationAdminEmail, request);

            if (addingAdmin.Success)
            {
                return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("InstitutionOfEducationAndAdminAdded")), true);
            }
            else
            {
                return result.Set(addingAdmin.Object, true);
            }
        }

        public async Task<PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>> GetAllInstitutionOfEducationAdmins(
            InstitutionOfEducationAdminSortingModel institutionOfEducationAdminFilterModel,
            PageApiModel pageModel)
        {            
            var admins = await GetAllInstitutionOfEducationAdminsSorted(institutionOfEducationAdminFilterModel);

            var result = new PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>();
            var mappedInstitution = _mapper.Map<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(admins);

            try
            {
                result = _paginationService.GetPageFromCollection(mappedInstitution, pageModel);
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<ResponseApiModel<IEnumerable<SchoolAdminResponseApiModel>>> GetAllSchoolAdmins()
        {
            var result = new ResponseApiModel<IEnumerable<SchoolAdminResponseApiModel>>();
            var admins = await _schoolAdminRepository.GetAllSchoolAdmins();
            if (admins.Count() < 1)
            {
                throw new NotFoundException(_resourceManager.GetString("AdminsNotFound"));
            }
            result.Object = _mapper.Map<IEnumerable<SchoolAdminResponseApiModel>>(admins);
            return result.Set(true);
        }

        private async Task<IEnumerable<InstitutionOfEducationAdminDTO>> GetAllInstitutionOfEducationAdminsSorted(
            InstitutionOfEducationAdminSortingModel institutionOfEducationAdminFilterModel)
        {
            var admins = await _institutionOfEducationAdminRepository.GetAllUniAdmins();

            if (institutionOfEducationAdminFilterModel.UserName.GetValueOrDefault())
            {
                admins = admins.OrderBy(x => x.User.UserName);
            }
            else if(institutionOfEducationAdminFilterModel.UserName != null)
            {
                admins = admins.OrderByDescending(x => x.User.UserName);
            }
            else
            if (institutionOfEducationAdminFilterModel.Email.GetValueOrDefault())
            {
                admins = admins.OrderBy(x => x.User.Email);
            }
            else if (institutionOfEducationAdminFilterModel.Email != null)
            {
                admins = admins.OrderByDescending(x => x.User.Email);
            }
            else
            if (institutionOfEducationAdminFilterModel.InstitutionOfEducationName.GetValueOrDefault())
            {
                admins = admins.OrderBy(x => x.InstitutionOfEducation.Name);
            }
            else if (institutionOfEducationAdminFilterModel.InstitutionOfEducationName != null)
            {
                admins = admins.OrderByDescending(x => x.InstitutionOfEducation.Name);
            }
            else
            if (institutionOfEducationAdminFilterModel.IsBanned.GetValueOrDefault())
            {
                admins = admins.OrderBy(x => x.IsBanned);
            }
            else if (institutionOfEducationAdminFilterModel.IsBanned != null)
            {
                admins = admins.OrderByDescending(x => x.IsBanned);
            }

            return admins.ToList();
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> UpdateSpecialtyById(SpecialtyPutApiModel model)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var specialtyDTO = _mapper.Map<SpecialtyDTO>(model);

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("SpecialtyWasSuccessefullyChanged")),
                await _specialtyRepository.Update(_mapper.Map<Specialty>(specialtyDTO)));
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToTheListOfAllSpecialties(SpecialtyPostApiModel specialtyPostApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var specialityDTO = _mapper.Map<SpecialtyDTO>(specialtyPostApiModel);
            await _specialtyRepository.Add(_mapper.Map<Specialty>(specialityDTO));

            return result.Set(
                   new DescriptionResponseApiModel(_resourceManager.GetString("SpecialtyWasAdded")), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddDirection(DirectionPostApiModel directionModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var directionDTO = _mapper.Map<DirectionDTO>(directionModel);
            await _directionRepository.Add(_mapper.Map<Direction>(directionDTO));

            return result.Set(
                   new DescriptionResponseApiModel(_resourceManager.GetString("DirectionWasAdded")), true);
        }

        public async Task<ResponseApiModel<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>> GetIoEModeratorsByIoEId(string ioEId)
        {
            return new ResponseApiModel<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>
            {
                Object = _mapper.Map<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>(await _ioEModeratorRepository.GetByIoEId(ioEId)),
                Success = true
            };
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ChooseIoEAdminFromModerators(IoEAdminAddFromModeratorsApiModel ioEAdminAddFromModeratorsApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var moderator = await _ioEModeratorRepository.GetByUserId(ioEAdminAddFromModeratorsApiModel.UserId);
            var dbUser = await _userRepository.GetUserWithRoles(moderator.User.Id);

            await _userManager.RemoveFromRoleAsync(dbUser, ProjectRoles.InstitutionOfEducationModerator);
            await _userManager.AddToRoleAsync(dbUser, ProjectRoles.InstitutionOfEducationAdmin);

            await _ioEModeratorRepository.Delete(moderator.Id);

            await _institutionOfEducationAdminRepository.AddUniAdmin(new InstitutionOfEducationAdmin
            {
                InstitutionOfEducationId = ioEAdminAddFromModeratorsApiModel.IoEId,
                UserId = ioEAdminAddFromModeratorsApiModel.UserId
            });

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("AdminAdded")), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ChangeBannedStatusOfIoE(string id)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var IoE = await _institutionOfEducationRepository.Get(id);
            if (IoE == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("InstitutionOfEducationNotFound")}: {id}");
            }
            string res;
            if (IoE.IsBanned == false)
            {
                res = await _institutionOfEducationRepository.Disable(_mapper.Map<InstitutionOfEducation>(IoE));
            }
            else
            {
                res = await _institutionOfEducationRepository.Enable(_mapper.Map<InstitutionOfEducation>(IoE));
            }
            return result.Set(new DescriptionResponseApiModel(res), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> GetIoEAdminIdByIoEId(string ioEId) 
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var admin = await _institutionOfEducationAdminRepository.GetByInstitutionOfEducationId(ioEId);

            if (admin == null) 
            {
                throw new NotFoundException(_resourceManager.GetString("IoEWasNotFoundOrAdminWasDeleted"));
            }

            return result.Set(new DescriptionResponseApiModel(admin.Id), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> ModifyInstitution(string userId, JsonPatchDocument<InstitutionOfEducationPostApiModel> institutionOfEducationPostApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            InstitutionOfEducationPostApiModel request = new InstitutionOfEducationPostApiModel();
            institutionOfEducationPostApiModel.ApplyTo(request);

            var validator = new InstitutionOfEducationPostApiModelValidator();
            var validResult = await validator.ValidateAsync(request);

            string ioEId = (await _institutionOfEducationAdminRepository.GetByUserId(userId)).InstitutionOfEducationId;
            var currentInstitutionOfEducationDTO = await _institutionOfEducationRepository.Get(ioEId);

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

            await _institutionOfEducationRepository.Update(_mapper.Map<InstitutionOfEducation>(currentInstitutionOfEducationDTO));

            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("InformationChanged")), true);
        }
    }
}
