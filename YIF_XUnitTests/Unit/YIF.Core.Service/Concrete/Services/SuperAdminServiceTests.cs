using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class SuperAdminServiceTests
    {
        private readonly Mock<IUserService<DbUser>> _userService;
        private readonly Mock<IUserRepository<DbUser, UserDTO>> _userRepository;
        private readonly Mock<FakeUserManager<DbUser>> _userManager;
        private readonly FakeSignInManager<DbUser> _signInManager;
        private readonly Mock<IJwtService> _jwtService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUniversityAdminRepository<UniversityAdminDTO>> _universityAdminRepository;
        private readonly Mock<IUniversityRepository<University, UniversityDTO>> _universityRepository;
        private readonly Mock<IUniversityModeratorRepository<UniversityModeratorDTO>> _universityModeratorRepository;
        private readonly Mock<ISchoolRepository<SchoolDTO>> _schoolRepository;
        private readonly Mock<ISchoolAdminRepository<SchoolAdminDTO>> _schoolAdminRepository;
        private readonly Mock<ISchoolModeratorRepository<SchoolModeratorDTO>> _schoolModeratorRepository;
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<ITokenRepository<TokenDTO>> _tokenRepository;
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly SuperAdminService superAdminService;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<IConfiguration> _configuration;

        private readonly DbUser _user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
        private readonly UniversityAdmin uniAdmin = new UniversityAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };
        private readonly University uni = new University { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub", ImagePath = "Image1Path" };
        private readonly UniversityModerator universityModerator = new UniversityModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        private readonly List<UniversityAdmin> _databaseUniAdmins = new List<UniversityAdmin>();
        private readonly List<UniversityAdminDTO> _universityAdminsDTO;
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<University> _databaseUniversities = new List<University>();
        private readonly List<UniversityModerator> _databaseUniversityModerators = new List<UniversityModerator>();
        private readonly List<UniversityAdminResponseApiModel> _listViewModel;

        public readonly UniversityAdminApiModel model = new UniversityAdminApiModel
        {
            UniversityName = "Name",
            Email = "Email",
        };
        public SuperAdminServiceTests()
        {
            _userService = new Mock<IUserService<DbUser>>();
            _userRepository = new Mock<IUserRepository<DbUser, UserDTO>>();
            _userManager = new Mock<FakeUserManager<DbUser>>();
            _signInManager = new FakeSignInManager<DbUser>(_userManager);
            _jwtService = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();
            _universityAdminRepository = new Mock<IUniversityAdminRepository<UniversityAdminDTO>>();
            _universityRepository = new Mock<IUniversityRepository<University, UniversityDTO>>();
            _universityModeratorRepository = new Mock<IUniversityModeratorRepository<UniversityModeratorDTO>>();
            _schoolRepository = new Mock<ISchoolRepository<SchoolDTO>>();
            _schoolAdminRepository = new Mock<ISchoolAdminRepository<SchoolAdminDTO>>();
            _schoolModeratorRepository = new Mock<ISchoolModeratorRepository<SchoolModeratorDTO>>();
            _dbContextMock = new Mock<IApplicationDbContext>();
            _tokenRepository = new Mock<ITokenRepository<TokenDTO>>();
            _resourceManager = new Mock<ResourceManager>();
            _env = new Mock<IWebHostEnvironment>();
            _configuration = new Mock<IConfiguration>();

            superAdminService = new SuperAdminService(
                                                    _userService.Object,
                                                    _userRepository.Object,
                                                    _userManager.Object,
                                                    _signInManager,
                                                    _jwtService.Object,
                                                    _mapperMock.Object,
                                                    _universityRepository.Object,
                                                    _universityAdminRepository.Object,
                                                    _universityModeratorRepository.Object,
                                                    _schoolRepository.Object,
                                                    _schoolAdminRepository.Object,
                                                    _schoolModeratorRepository.Object,
                                                    _tokenRepository.Object,
                                                    _resourceManager.Object,
                                                    _env.Object,
                                                    _configuration.Object);

            _dbContextMock.Setup(p => p.UniversityAdmins).Returns(DbContextMock.GetQueryableMockDbSet<UniversityAdmin>(_databaseUniAdmins));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_databaseDbUsers));
            _dbContextMock.Setup(p => p.Universities).Returns(DbContextMock.GetQueryableMockDbSet<University>(_databaseUniversities));
            _dbContextMock.Setup(p => p.UniversityModerators).Returns(DbContextMock.GetQueryableMockDbSet<UniversityModerator>(_databaseUniversityModerators));


            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            _databaseDbUsers.Add(_user);
            _databaseUniversities.Add(uni);
            _databaseUniAdmins.Add(uniAdmin);
            _databaseUniversityModerators.Add(universityModerator);

            _universityAdminsDTO = new List<UniversityAdminDTO>()
            {
                new UniversityAdminDTO {Id="1", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9" },
                new UniversityAdminDTO {Id="2", UniversityId = "107a43f8-7553-4eec-9e91-898a9cba37c9" },
            };
            _listViewModel = new List<UniversityAdminResponseApiModel>()
            {
                new UniversityAdminResponseApiModel { Id = _universityAdminsDTO[0].Id,  },
                new UniversityAdminResponseApiModel { Id = _universityAdminsDTO[1].Id,  }
            };

        }
        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnAllAdmins_IfThereAreMoreThanOneAdmin()
        {
            //Arrange
            _universityAdminRepository.Setup(x => x.GetAllUniAdmins()).Returns(Task.FromResult(_universityAdminsDTO.AsEnumerable()));
            _mapperMock.Setup(s => s.Map<IEnumerable<UniversityAdminResponseApiModel>>(_universityAdminsDTO)).Returns(_listViewModel);

            // Act
            var result = await superAdminService.GetAllUniversityAdmins();
            var users = result.Object.ToList();

            //Assert
            Assert.True(result.Success);
            Assert.Equal(users[0].Id, _listViewModel[0].Id);
            Assert.Equal(users[1].Id, _listViewModel[1].Id);
        }
        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnEmpty_IfThereAreNoAdmins()
        {
            // Arrange
            _universityAdminRepository.Setup(s => s.GetAllUniAdmins()).Returns(Task.FromResult(new List<UniversityAdminDTO>().AsEnumerable()));

            // Act
            var result = await superAdminService.GetAllUniversityAdmins();
            var users = result.Object.ToList();
            //Assert
            Assert.True(result.Success);
            Assert.Empty(users);
        }
        [Fact]
        public async Task AddUniAdmin_NoUniFound_returnsResult()
        {
            List<UniversityDTO> listNull = new List<UniversityDTO>();
            _universityRepository.Setup(p => p.Find(It.IsAny<Expression<Func<University, bool>>>())).ReturnsAsync(listNull);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.AddUniversityAdmin(model));
        }

        [Fact]
        public async Task AddUniAdmin_AdminAlreadyExists_returnsResult()
        {
            UniversityAdminDTO universityAdmin = new UniversityAdminDTO
            {
                Id = "Id",
                UniversityId = "SomeUniId"
            };
            List<UniversityDTO> list = new List<UniversityDTO>
            {
                new UniversityDTO
                {
                    Id = "id"
                }
            };
            _universityRepository.Setup(p => p.Find(It.IsAny<Expression<Func<University, bool>>>()))
                                                                                                .ReturnsAsync(list);
            _universityAdminRepository.Setup(p => p.GetByUniversityId("id"))
                                                                          .ReturnsAsync(universityAdmin);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => superAdminService.AddUniversityAdmin(model));
        }
        [Fact]
        public async Task AddUniAdmin_UserAlreadyExists_returnsResult()
        {
            DbUser user = new DbUser
            {
                Id = "Id"
            };
            UniversityAdminDTO universityAdmin = new UniversityAdminDTO
            {
                Id = "Id",
                UniversityId = "SomeUniId"
            };
            List<UniversityDTO> list = new List<UniversityDTO>
            {
                new UniversityDTO
                {
                    Id = "id"
                }
            };
            _universityRepository.Setup(p => p.Find(It.IsAny<Expression<Func<University, bool>>>()))
                                                                                                .ReturnsAsync(list);
            _universityAdminRepository.Setup(p => p.GetByUniversityId("sdfs"))
                                                                          .ReturnsAsync(universityAdmin);
            _userManager.Setup(p => p.FindByEmailAsync("Email")).
                                                            ReturnsAsync(user);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => superAdminService.AddUniversityAdmin(model));
        }

        [Fact]
        public async Task DeleteAdmin_ReturnsSuccessDeleteMessage()
        {
            //Arrange
            _universityAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<UniversityAdminDTO>(new UniversityAdminDTO
                    { 
                        Id = uniAdmin.Id,
                        UserId = uniAdmin.UserId,
                        User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" }
                    }));
            _userRepository.Setup(p => p.Delete(uniAdmin.UserId)).Returns(Task.FromResult<bool>(true));

            //Act
            var a = await superAdminService.DeleteUniversityAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("User IsDeleted was updated", a.Object.Message);
        }
        [Fact]
        public async Task DeleteAdmin_NoAdminfound()
        {
            //Arrange
            _universityAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<UniversityAdminDTO>(null));

            //Act

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.DeleteUniversityAdmin(uniAdmin.Id));
        }
        [Fact]
        public async Task DisableAdmin_ReturnsSuccessDisableMessage()
        {
            //Arrange
            _universityAdminRepository
                .Setup(p => p.GetById(uniAdmin.Id))
                .Returns(Task.FromResult<UniversityAdminDTO>(new UniversityAdminDTO
                {
                    Id = uniAdmin.Id,
                    UserId = uniAdmin.UserId,
                    User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" }
                }));
            _universityAdminRepository.Setup(x => x.Disable(uniAdmin)).Returns(Task.FromResult("Admin IsBanned was set to true"));
            _mapperMock.Setup(x => x.Map<UniversityAdmin>(It.IsAny<UniversityAdminDTO>())).Returns(uniAdmin);

            //Act
            var a = await superAdminService.DisableUniversityAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("Admin IsBanned was set to true", a.Object.Message);
        }
        [Fact]
        public async Task DisableAdmin_ReturnsSuccessEnableMessage()
        {
            //Arrange
            _universityAdminRepository
                .Setup(p => p.GetById(uniAdmin.Id))
                .Returns(Task.FromResult<UniversityAdminDTO>(new UniversityAdminDTO
                {
                    Id = uniAdmin.Id,
                    UserId = uniAdmin.UserId,
                    User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" },
                    IsBanned = true
                }));
            _universityAdminRepository.Setup(x => x.Enable(uniAdmin)).Returns(Task.FromResult("Admin IsBanned was set to false"));
            _mapperMock.Setup(x => x.Map<UniversityAdmin>(It.IsAny<UniversityAdminDTO>())).Returns(uniAdmin);

            //Act
            var a = await superAdminService.DisableUniversityAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("Admin IsBanned was set to false", a.Object.Message);
        }

    }
}
