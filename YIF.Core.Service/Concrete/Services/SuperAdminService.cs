using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
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
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _institutionOfEducationRepository;
        private readonly IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO> _institutionOfEducationModeratorRepository;
        private readonly ISchoolRepository<SchoolDTO> _schoolRepository;
        private readonly ISchoolAdminRepository<SchoolAdminDTO> _schoolAdminRepository;
        private readonly ISchoolModeratorRepository<SchoolModeratorDTO> _schoolModeratorRepository;
        private readonly ITokenRepository<TokenDTO> _tokenRepository;
        private readonly ResourceManager _resourceManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public SuperAdminService(
            IUserService<DbUser> userService,
            IUserRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> institutionOfEducationRepository,
            IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> institutionOfEducationAdminRepository,
            IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO> institutionOfEducationModeratorRepository,
            ISchoolRepository<SchoolDTO> schoolRepository,
            ISchoolAdminRepository<SchoolAdminDTO> schoolAdminRepository,
            ISchoolModeratorRepository<SchoolModeratorDTO> schoolModeratorRepository,
            ITokenRepository<TokenDTO> tokenRepository,
            ResourceManager resourceManager, 
            IWebHostEnvironment env,
            IConfiguration configuration)
        {
            _userService = userService;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
            _institutionOfEducationAdminRepository = institutionOfEducationAdminRepository;
            _institutionOfEducationRepository = institutionOfEducationRepository;
            _institutionOfEducationModeratorRepository = institutionOfEducationModeratorRepository;
            _schoolRepository = schoolRepository;
            _schoolAdminRepository = schoolAdminRepository;
            _schoolModeratorRepository = schoolModeratorRepository;
            _tokenRepository = tokenRepository;
            _resourceManager = resourceManager;
            _env = env;
            _configuration = configuration;
        }
        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> AddInstitutionOfEducationAdmin(InstitutionOfEducationAdminApiModel institutionOfEducationAdminModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            //take uni
            var institutionOfEducations = await _institutionOfEducationRepository.Find(x => x.Name == institutionOfEducationAdminModel.InstitutionOfEducationName);
            if (institutionOfEducations.Count() == 0)
            {
                throw new NotFoundException($"{_resourceManager.GetString("InstitutionOfEducationWithSuchNameNotFound")}: {institutionOfEducationAdminModel.InstitutionOfEducationName}");
            }
            var institutionOfEducation = institutionOfEducations.First();
            var adminCheck = await _institutionOfEducationAdminRepository.GetByInstitutionOfEducationId(institutionOfEducation.Id);
            if (adminCheck != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("AdministratorOfThisSchoolAlreadyExists"));
            }

            var searchUser = _userManager.FindByEmailAsync(institutionOfEducationAdminModel.Email);
            if (searchUser.Result != null && searchUser.Result.IsDeleted == false)
            {
                throw new InvalidOperationException(_resourceManager.GetString("UserAlreadyExists"));
            }

            var dbUser = new DbUser
            {
                Email = institutionOfEducationAdminModel.Email,
                UserName = institutionOfEducationAdminModel.Email
            };

            var registerResult = await _userRepository.Create(dbUser, null, null, ProjectRoles.InstitutionOfEducationAdmin);
            if (registerResult != string.Empty)
            {                
                throw new InvalidOperationException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
            }

            await _institutionOfEducationAdminRepository.AddUniAdmin(new InstitutionOfEducationAdmin { InstitutionOfEducationId = institutionOfEducation.Id });
            var admin = await _institutionOfEducationAdminRepository.GetByInstitutionOfEducationIdWithoutIsDeletedCheck(institutionOfEducation.Id);

            InstitutionOfEducationModerator toAdd = new InstitutionOfEducationModerator
            {
                //InstitutionOfEducationId = institutionOfEducation.Id,
                UserId = dbUser.Id,
                AdminId = admin.Id
            };
            await _institutionOfEducationModeratorRepository.AddUniModerator(toAdd);

            return null;
            ////to do change logic and response model


            //var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            //var refreshToken = _jwtService.CreateRefreshToken();

            //var tokenDb = await _tokenRepository.FindUserToken(dbUser.Id);
            //if (tokenDb == null)
            //{
            //    tokenDb = new TokenDTO
            //    {
            //        Id = dbUser.Id,
            //        RefreshToken = refreshToken,
            //        RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
            //    };
            //    await _tokenRepository.AddUserToken(tokenDb);
            //}
            //else await _tokenRepository.UpdateUserToken(dbUser.Id, refreshToken);

            //await _signInManager.SignInAsync(dbUser, isPersistent: false);

            //return result.Set(new AuthenticateResponseApiModel(token, refreshToken), true);
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
            var ch = await _institutionOfEducationAdminRepository.GetUserByAdminId(adminId);
            if (ch == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("UserWithSuchIdNotFound")}: {adminId}");
            }
            await _userRepository.Delete(ch.User.Id);
            return result.Set(new DescriptionResponseApiModel("User IsDeleted was updated"), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DisableInstitutionOfEducationAdmin(string adminId)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var ch = await _institutionOfEducationAdminRepository.GetById(adminId);
            if (ch == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("UserWithSuchIdNotFound")}: {adminId}");
            }
            var res = await _institutionOfEducationAdminRepository.Disable(_mapper.Map<InstitutionOfEducationAdmin>(ch));
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

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddInstitutionOfEducationAndAdmin(InstitutionOfEducationPostApiModel institutionOfEducationPostApiModel, HttpRequest request)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            var ch = await _institutionOfEducationRepository.GetByName(institutionOfEducationPostApiModel.Name);
            if (ch != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("InstitutionOfEducationWithSuchNameAlreadyExists"));
            }

            var searchUser = _userManager.FindByEmailAsync(institutionOfEducationPostApiModel.InstitutionOfEducationAdminEmail);
            if (searchUser.Result != null && searchUser.Result.IsDeleted == false)
            {
                throw new InvalidOperationException(_resourceManager.GetString("UserAlreadyExists"));
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

            var dbUser = new DbUser
            {
                Email = institutionOfEducationPostApiModel.InstitutionOfEducationAdminEmail,
                UserName = institutionOfEducationPostApiModel.InstitutionOfEducationAdminEmail
            };

            var registerResult = await _userRepository.Create(dbUser, null, null, ProjectRoles.InstitutionOfEducationAdmin);
            if (registerResult != string.Empty)
            {
                throw new InvalidOperationException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
            }

            await _institutionOfEducationAdminRepository.AddUniAdmin(new InstitutionOfEducationAdmin { InstitutionOfEducationId = institutionOfEducation.Id, UserId = dbUser.Id });

            var admin = await _institutionOfEducationAdminRepository.GetByInstitutionOfEducationIdWithoutIsDeletedCheck(institutionOfEducation.Id);
            if (admin == null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("InstitutionOfEducationAdminFailed"));
            }

            //To check
            var resultResetPasswordByEmail = await _userService.ResetPasswordByEmail(institutionOfEducationPostApiModel.InstitutionOfEducationAdminEmail, request);
            if (!resultResetPasswordByEmail.Success)
            {
                throw new InvalidOperationException($"{_resourceManager.GetString("ResetPasswordByEmailFailed")}: {resultResetPasswordByEmail.Message}");
            }
            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("InstitutionOfEducationAndAdminAdded")), true);
        }

        public async Task<ResponseApiModel<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>> GetAllInstitutionOfEducationAdmins()
        {
            var result = new ResponseApiModel<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>();
            var admins = await _institutionOfEducationAdminRepository.GetAllUniAdmins();
            result.Object = _mapper.Map<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(admins);
            return result.Set(true);
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
    }
}
