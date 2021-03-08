﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
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
        private readonly IUniversityAdminRepository<UniversityAdminDTO> _universityAdminRepository;
        private readonly IUniversityRepository<University, UniversityDTO> _universityRepository;
        private readonly IUniversityModeratorRepository<UniversityModeratorDTO> _universityModeratorRepository;
        private readonly ISchoolRepository<SchoolDTO> _schoolRepository;
        private readonly ISchoolAdminRepository<SchoolAdminDTO> _schoolAdminRepository;
        private readonly ISchoolModeratorRepository<SchoolModeratorDTO> _schoolModeratorRepository;
        private readonly ITokenRepository<TokenDTO> _tokenRepository;
        private readonly ResourceManager _resourceManager;

        public SuperAdminService(
            IUserService<DbUser> userService,
            IUserRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IUniversityRepository<University, UniversityDTO> universityRepository,
            IUniversityAdminRepository<UniversityAdminDTO> universityAdminRepository,
            IUniversityModeratorRepository<UniversityModeratorDTO> universityModeratorRepository,
            ISchoolRepository<SchoolDTO> schoolRepository,
            ISchoolAdminRepository<SchoolAdminDTO> schoolAdminRepository,
            ISchoolModeratorRepository<SchoolModeratorDTO> schoolModeratorRepository,
            ITokenRepository<TokenDTO> tokenRepository,
            ResourceManager resourceManager)
        {
            _userService = userService;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
            _universityAdminRepository = universityAdminRepository;
            _universityRepository = universityRepository;
            _universityModeratorRepository = universityModeratorRepository;
            _schoolRepository = schoolRepository;
            _schoolAdminRepository = schoolAdminRepository;
            _schoolModeratorRepository = schoolModeratorRepository;
            _tokenRepository = tokenRepository;
            _resourceManager = resourceManager;
        }
        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> AddUniversityAdmin(UniversityAdminApiModel universityAdminModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            //take uni
            var universities = await _universityRepository.Find(x => x.Name == universityAdminModel.UniversityName);
            if (universities.Count() == 0)
            {
                throw new NotFoundException($"{_resourceManager.GetString("UniversityWithSuchNameNotFound")}: {universityAdminModel.UniversityName}");
            }
            var university = universities.First();
            var adminCheck = await _universityAdminRepository.GetByUniversityId(university.Id);
            if (adminCheck != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("AdministratorOfThisSchoolAlreadyExists"));
            }

            var searchUser = _userManager.FindByEmailAsync(universityAdminModel.Email);
            if (searchUser.Result != null && searchUser.Result.IsDeleted == false)
            {
                throw new InvalidOperationException(_resourceManager.GetString("UserAlreadyExists"));
            }

            var dbUser = new DbUser
            {
                Email = universityAdminModel.Email,
                UserName = universityAdminModel.Email
            };

            var registerResult = await _userRepository.Create(dbUser, null, null, ProjectRoles.UniversityAdmin);
            if (registerResult != string.Empty)
            {                
                throw new InvalidOperationException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
            }

            await _universityAdminRepository.AddUniAdmin(new UniversityAdmin { UniversityId = university.Id });
            var admin = await _universityAdminRepository.GetByUniversityIdWithoutIsDeletedCheck(university.Id);

            UniversityModerator toAdd = new UniversityModerator
            {
                //UniversityId = university.Id,
                UserId = dbUser.Id,
                AdminId = admin.Id
            };
            await _universityModeratorRepository.AddUniModerator(toAdd);

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

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteUniversityAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            string ch = await _universityAdminRepository.Delete(schoolUniAdminDeleteApi.Id);
            if (ch == null)
            {
                throw new NotFoundException($"{_resourceManager.GetString("UserWithSuchIdNotFound")}: {schoolUniAdminDeleteApi.Id}");
            }
            return result.Set(new DescriptionResponseApiModel(ch), true);
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

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddUniversityAndAdmin(UniversityPostApiModel universityPostApiModel, HttpRequest request)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            var ch = await _universityRepository.GetByName(universityPostApiModel.Name);
            if (ch != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("UniversityWithSuchNameAlreadyExists"));
            }
            
            var university = await _universityRepository.AddUniversity(_mapper.Map<University>(universityPostApiModel));

            var adminCheck = await _universityAdminRepository.GetByUniversityId(university.Id);
            if (adminCheck != null)
            {
                throw new InvalidOperationException(_resourceManager.GetString("AdministratorOfThisSchoolAlreadyExists"));
            }

            var searchUser = _userManager.FindByEmailAsync(universityPostApiModel.UniversityAdminEmail);
            if (searchUser.Result != null && searchUser.Result.IsDeleted == false)
            {
                throw new InvalidOperationException(_resourceManager.GetString("UserAlreadyExists"));
            }

            var dbUser = new DbUser
            {
                Email = universityPostApiModel.UniversityAdminEmail,
                UserName = universityPostApiModel.UniversityAdminEmail
            };

            var registerResult = await _userRepository.Create(dbUser, null, null, ProjectRoles.UniversityAdmin);
            if (registerResult != string.Empty)
            {
                throw new InvalidOperationException($"{_resourceManager.GetString("UserCreationFailed")}: {registerResult}");
            }

            await _universityAdminRepository.AddUniAdmin(new UniversityAdmin { UniversityId = university.Id });
            var admin = await _universityAdminRepository.GetByUniversityIdWithoutIsDeletedCheck(university.Id);

            //
            var resultResetPasswordByEmail = await _userService.ResetPasswordByEmail(universityPostApiModel.UniversityAdminEmail, request);
            if (!resultResetPasswordByEmail.Success)
            {
                throw new InvalidOperationException($"{_resourceManager.GetString("ResetPasswordByEmailFailed")}: {resultResetPasswordByEmail.Message}");
            }
            return result.Set(new DescriptionResponseApiModel(_resourceManager.GetString("UniversityAdded")), true);
        }

        public async Task<ResponseApiModel<IEnumerable<UniversityAdminResponseApiModel>>> GetAllUniversityAdmins()
        {
            var result = new ResponseApiModel<IEnumerable<UniversityAdminResponseApiModel>>();
            var admins = await _universityAdminRepository.GetAllUniAdmins();
            if (admins.Count() < 1)
            {
                throw new NotFoundException(_resourceManager.GetString("AdminsNotFound"));
            }
            result.Object = _mapper.Map<IEnumerable<UniversityAdminResponseApiModel>>(admins);
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
