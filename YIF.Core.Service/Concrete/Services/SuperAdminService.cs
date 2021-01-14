using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Others;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly IRepository<DbUser, UserDTO> _userRepository;
        private readonly UserManager<DbUser> _userManager;
        private readonly SignInManager<DbUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IUniversityAdminRepository<UniversityAdminDTO> _universityAdminRepository;
        private readonly IRepository<University,UniversityDTO> _universityRepository;
        private readonly IUniversityModeratorRepository<UniversityModeratorDTO> _universityModeratorRepository;
        public SuperAdminService(IRepository<DbUser, UserDTO> userRepository,
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IJwtService _IJwtService,
            IMapper mapper,
            IRepository<University, UniversityDTO> universityRepository,
            IUniversityAdminRepository<UniversityAdminDTO> universityAdminRepository,
            IUniversityModeratorRepository<UniversityModeratorDTO> universityModeratorRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = _IJwtService;
            _mapper = mapper;
            _universityAdminRepository = universityAdminRepository;
            _universityRepository = universityRepository;
            _universityModeratorRepository = universityModeratorRepository;
        }
        public async Task<ResponseApiModel<AuthenticateResponseApiModel>> AddUniversityAdmin(UniversityAdminApiModel universityAdminModel)
        {
            var result = new ResponseApiModel<AuthenticateResponseApiModel>();

            //take uni
            var universities = await _universityRepository.Find(x=> x.Name == universityAdminModel.UniversityName);
            var university = universities.First();

            if (university == null)
            {
                return result.Set(false, "There is no university with name" + universityAdminModel.UniversityName + "in our database");
            }

            var adminCheck = await _universityAdminRepository.GetByUniversityId(university.Id);
            if (adminCheck != null)
            {
                return result.Set(409, "Admin for this uni already exists( 1 university can have only 1 admin )");
            }

            var searchUser = _userManager.FindByEmailAsync(universityAdminModel.Email);
            if (searchUser.Result != null)
            {
                return result.Set(409, "User already exist");
            }

            var dbUser = new DbUser
            {
                Email = universityAdminModel.Email,
                UserName = universityAdminModel.Email
            };

            var universityModerator = new UniversityModerator();
            //TO DO зачем його передавать то шо више
            var registerResult = await _userRepository.Create(dbUser, universityModerator, universityAdminModel.Password, ProjectRoles.UniversityModerator);

            if (registerResult != string.Empty)
            {
                return result.Set(409, registerResult);
            }


            await _universityAdminRepository.AddUniAdmin(new UniversityAdmin { UniversityId = university.Id });
            var admin = await _universityAdminRepository.GetByUniversityId(university.Id);

            UniversityModerator toAdd = new UniversityModerator();
            toAdd.UniversityId = university.Id;
            toAdd.UserId = dbUser.Id;
            toAdd.AdminId = admin.Id;
            await _universityModeratorRepository.AddUniModerator(toAdd);


            //to do change logic and response model


            var token = _jwtService.CreateToken(_jwtService.SetClaims(dbUser));
            var refreshToken = _jwtService.CreateRefreshToken();

            await _userRepository.UpdateUserToken(dbUser, refreshToken);

            await _signInManager.SignInAsync(dbUser, isPersistent: false);

            result.Object = new AuthenticateResponseApiModel { Token = token, RefreshToken = refreshToken };

            return result.Set(201);
        }

    }
}
