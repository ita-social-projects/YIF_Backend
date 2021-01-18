﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Others;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;
using YIF.Core.Domain.DtoModels.SchoolAdmin;
using YIF.Core.Domain.DtoModels.SchoolModerator;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly IUserRepository<DbUser, UserDTO> _userRepository;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUniversityAdminRepository<UniversityAdminDTO> _universityAdminRepository;
        private readonly IRepository<University, UniversityDTO> _universityRepository;
        private readonly IUniversityModeratorRepository<UniversityModeratorDTO> _universityModeratorRepository;
        private readonly ISchoolRepository<SchoolDTO> _schoolRepository;
        private readonly ISchoolAdminRepository<SchoolAdminDTO> _schoolAdminRepository;
        private readonly ISchoolModeratorRepository<SchoolModeratorDTO> _schoolModeratorRepository;
        private readonly ITokenRepository _tokenRepository;
        public SuperAdminService(IUserRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IRepository<University, UniversityDTO> universityRepository,
            IUniversityAdminRepository<UniversityAdminDTO> universityAdminRepository,
            IUniversityModeratorRepository<UniversityModeratorDTO> universityModeratorRepository,
            ISchoolRepository<SchoolDTO> schoolRepository,
            ISchoolAdminRepository<SchoolAdminDTO> schoolAdminRepository,
            ISchoolModeratorRepository<SchoolModeratorDTO> schoolModeratorRepository,
            ITokenRepository tokenRepository)
        {
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
        }
        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> AddUniversityAdmin(UniversityAdminApiModel universityAdminModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            //take uni
            var universities = await _universityRepository.Find(x => x.Name == universityAdminModel.UniversityName);
            var university = universities.First();

            if (university == null)
            {
                return result.Set(false, "В базі даних немає університету із назвою: " + universityAdminModel.UniversityName);
            }

            var adminCheck = await _universityAdminRepository.GetByUniversityId(university.Id);
            if (adminCheck != null)
            {
                return result.Set(false, "Адміністратор цього університету вже існує (1 університет може мати лише 1 адміністратора)");
            }

            var searchUser = _userManager.FindByEmailAsync(universityAdminModel.Email);
            if (searchUser.Result != null && searchUser.Result.IsDeleted == false)
            {
                return result.Set(false, "Користувач вже існує");
            }

            var dbUser = new DbUser
            {
                Email = universityAdminModel.Email,
                UserName = universityAdminModel.Email
            };

            var registerResult = await _userRepository.Create(dbUser, null, universityAdminModel.Password, ProjectRoles.UniversityAdmin);

            if (registerResult != string.Empty)
            {
                return result.Set(false, registerResult);
            }


            await _universityAdminRepository.AddUniAdmin(new UniversityAdmin { UniversityId = university.Id });
            var admin = await _universityAdminRepository.GetByUniversityIdWithoutIsDeletedCheck(university.Id);

            UniversityModerator toAdd = new UniversityModerator();
            toAdd.UniversityId = university.Id;
            toAdd.UserId = dbUser.Id;
            toAdd.AdminId = admin.Id;
            await _universityModeratorRepository.AddUniModerator(toAdd);


            //to do change logic and response model


            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            await _tokenRepository.UpdateUserToken(dbUser, refreshToken);

            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            return result.Set(new AuthenticateResponseApiModel(token, refreshToken), true);
        }

        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> AddSchoolAdmin(SchoolAdminApiModel schoolAdminModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            //take uni
            var school = await _schoolRepository.GetByName(schoolAdminModel.SchoolName);
            if (school == null)
            {
                return result.Set(false, "В базі даних немає школи із назвою: " + schoolAdminModel.SchoolName);
            }

            var adminCheck = await _schoolAdminRepository.GetBySchoolId(school.Id);
            if (adminCheck != null)
            {
                return result.Set(false, "Адміністратор цього університету вже існує (1 університет може мати лише 1 адміністратора)");
            }

            var searchUser = _userManager.FindByEmailAsync(schoolAdminModel.Email);
            if (searchUser.Result != null && searchUser.Result.IsDeleted == false)
            {
                return result.Set(false, "Користувач вже існує");
            }

            var dbUser = new DbUser
            {
                Email = schoolAdminModel.Email,
                UserName = schoolAdminModel.Email
            };

            //TO DO зачем його передавать то шо више
            var registerResult = await _userRepository.Create(dbUser, null, schoolAdminModel.Password, ProjectRoles.SchoolAdmin);

            if (registerResult != string.Empty)
            {
                return result.Set(false, registerResult);
            }


            await _schoolAdminRepository.AddSchoolAdmin(new SchoolAdmin { SchoolId = school.Id });
            var admin = await _schoolAdminRepository.GetBySchoolIdWithoutIsDeletedCheck(school.Id);

            SchoolModerator toAdd = new SchoolModerator();
            toAdd.SchoolId = school.Id;
            toAdd.UserId = dbUser.Id;
            toAdd.AdminId = admin.Id;
            await _schoolModeratorRepository.AddSchoolModerator(toAdd);


            //to do change logic and response model


            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            await _tokenRepository.UpdateUserToken(dbUser, refreshToken);

            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            return result.Set(new AuthenticateResponseApiModel(token, refreshToken), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteUniversityAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            string ch = await _universityAdminRepository.Delete(schoolUniAdminDeleteApi.Id);
            if (ch == null)
            {
                return result.Set(false, "Не знайдено користувача з таким ідентифікатором: " + schoolUniAdminDeleteApi.Id);
            }
            return result.Set(new DescriptionResponseApiModel(ch), true);
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> DeleteSchoolAdmin(SchoolUniAdminDeleteApiModel schoolUniAdminDeleteApi)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            string ch = await _schoolAdminRepository.Delete(schoolUniAdminDeleteApi.Id);
            if (ch == null)
            {
                return result.Set(false, "Не знайдено користувача з таким ідентифікатором: " + schoolUniAdminDeleteApi.Id);
            }
            return result.Set(new DescriptionResponseApiModel(ch), true);
        }
    }
}
