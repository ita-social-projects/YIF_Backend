using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        private readonly Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO>> _institutionOfEducationAdminRepository;
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> _institutionOfEducationRepository;
        private readonly Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO>> _institutionOfEducationModeratorRepository;
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
        private readonly InstitutionOfEducationAdmin uniAdmin = new InstitutionOfEducationAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", InstitutionOfEducationId = "007a43f8-7553-4eec-9e91-898a9cba37c9", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };
        private readonly InstitutionOfEducation uni = new InstitutionOfEducation { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub", ImagePath = "Image1Path" };
        private readonly InstitutionOfEducationModerator institutionOfEducationModerator = new InstitutionOfEducationModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        private readonly List<InstitutionOfEducationAdmin> _databaseUniAdmins = new List<InstitutionOfEducationAdmin>();
        private readonly List<InstitutionOfEducationAdminDTO> _institutionOfEducationAdminsDTO;
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<InstitutionOfEducation> _databaseInstitutionOfEducations = new List<InstitutionOfEducation>();
        private readonly List<InstitutionOfEducationModerator> _databaseInstitutionOfEducationModerators = new List<InstitutionOfEducationModerator>();
        private readonly List<InstitutionOfEducationAdminResponseApiModel> _listViewModel;

        private readonly Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

        public readonly InstitutionOfEducationAdminApiModel model = new InstitutionOfEducationAdminApiModel
        {
            InstitutionOfEducationId = "Name",
            AdminEmail = "AdminEmail",
        };
        public SuperAdminServiceTests()
        {
            _userService = new Mock<IUserService<DbUser>>();
            _userRepository = new Mock<IUserRepository<DbUser, UserDTO>>();
            _userManager = new Mock<FakeUserManager<DbUser>>();
            _signInManager = new FakeSignInManager<DbUser>(_userManager);
            _jwtService = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();
            _institutionOfEducationAdminRepository = new Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO>>();
            _institutionOfEducationRepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
            _institutionOfEducationModeratorRepository = new Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO>>();
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
                                                    _institutionOfEducationRepository.Object,
                                                    _institutionOfEducationAdminRepository.Object,
                                                    _institutionOfEducationModeratorRepository.Object,
                                                    _schoolRepository.Object,
                                                    _schoolAdminRepository.Object,
                                                    _schoolModeratorRepository.Object,
                                                    _tokenRepository.Object,
                                                    _resourceManager.Object,
                                                    _env.Object,
                                                    _configuration.Object);

            _dbContextMock.Setup(p => p.InstitutionOfEducationAdmins).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationAdmin>(_databaseUniAdmins));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_databaseDbUsers));
            _dbContextMock.Setup(p => p.InstitutionOfEducations).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducation>(_databaseInstitutionOfEducations));
            _dbContextMock.Setup(p => p.InstitutionOfEducationModerators).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationModerator>(_databaseInstitutionOfEducationModerators));


            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            _databaseDbUsers.Add(_user);
            _databaseInstitutionOfEducations.Add(uni);
            _databaseUniAdmins.Add(uniAdmin);
            _databaseInstitutionOfEducationModerators.Add(institutionOfEducationModerator);

            _institutionOfEducationAdminsDTO = new List<InstitutionOfEducationAdminDTO>()
            {
                new InstitutionOfEducationAdminDTO {Id="1", InstitutionOfEducationId = "007a43f8-7553-4eec-9e91-898a9cba37c9" },
                new InstitutionOfEducationAdminDTO {Id="2", InstitutionOfEducationId = "107a43f8-7553-4eec-9e91-898a9cba37c9" },
            };
            _listViewModel = new List<InstitutionOfEducationAdminResponseApiModel>()
            {
                new InstitutionOfEducationAdminResponseApiModel { Id = _institutionOfEducationAdminsDTO[0].Id,  },
                new InstitutionOfEducationAdminResponseApiModel { Id = _institutionOfEducationAdminsDTO[1].Id,  }
            };

        }
        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnAllAdmins_IfThereAreMoreThanOneAdmin()
        {
            //Arrange
            _institutionOfEducationAdminRepository.Setup(x => x.GetAllUniAdmins()).Returns(Task.FromResult(_institutionOfEducationAdminsDTO.AsEnumerable()));
            _mapperMock.Setup(s => s.Map<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(_institutionOfEducationAdminsDTO)).Returns(_listViewModel);

            // Act
            var result = await superAdminService.GetAllInstitutionOfEducationAdmins();
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
            _institutionOfEducationAdminRepository.Setup(s => s.GetAllUniAdmins()).Returns(Task.FromResult(new List<InstitutionOfEducationAdminDTO>().AsEnumerable()));

            // Act
            var result = await superAdminService.GetAllInstitutionOfEducationAdmins();
            var users = result.Object.ToList();
            //Assert
            Assert.True(result.Success);
            Assert.Empty(users);
        }

        [Theory]
        [InlineData("institutionId", "adminEmail")]
        public async Task AddInstitutionOfEducationAdmin_InstituionAlreadyExist(string institutionId, string adminEmail)
        {
            // Act
            _institutionOfEducationRepository.Setup(p => p.ContainsById(It.IsAny<string>())).ReturnsAsync(false);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.AddInstitutionOfEducationAdmin(institutionId, adminEmail, httpRequest.Object));
        }

        [Theory]
        [InlineData("institutionId", "adminEmail")]
        public async Task AddInstitutionOfEducationAdmin_ThisInstitutionAlreadyHaveAdmin(string institutionId, string adminEmail)
        {
            //Arrange
            var admin = new InstitutionOfEducationAdminDTO();

            _institutionOfEducationRepository.Setup(p => p.ContainsById(It.IsAny<string>())).ReturnsAsync(true);
            _institutionOfEducationAdminRepository.Setup(p => p.GetByInstitutionOfEducationIdWithoutIsDeletedCheck(It.IsAny<string>())).ReturnsAsync(admin);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => superAdminService.AddInstitutionOfEducationAdmin(institutionId, adminEmail, httpRequest.Object));
        }

        [Theory]
        [InlineData("institutionId", "adminEmail")]
        public async Task AddInstitutionOfEducationAdmin_UserExistAndHaveAdminPermision(string institutionId, string adminEmail)
        {            
            //Arrange
            InstitutionOfEducationAdminDTO admin = new InstitutionOfEducationAdminDTO();
            DbUser dbUser = new DbUser();
            IEnumerable<InstitutionOfEducationAdminDTO> institutionOfEducationAdmins = new List<InstitutionOfEducationAdminDTO>()
            {
                new InstitutionOfEducationAdminDTO(){ Id = institutionId}
            };

            _institutionOfEducationRepository.Setup(p => p.ContainsById(It.IsAny<string>())).ReturnsAsync(true);
            _institutionOfEducationAdminRepository.Setup(p => p.GetByInstitutionOfEducationIdWithoutIsDeletedCheck(It.IsAny<string>())).ReturnsAsync(admin);
            _userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dbUser);
            _institutionOfEducationAdminRepository.Setup(p => p.GetAllUniAdmins()).ReturnsAsync(institutionOfEducationAdmins);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => superAdminService.AddInstitutionOfEducationAdmin(institutionId, adminEmail, httpRequest.Object));
        }

        [Fact]
        public async Task DeleteAdmin_ReturnsSuccessDeleteMessage()
        {
            //Arrange
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                    { 
                        Id = uniAdmin.Id,
                        UserId = uniAdmin.UserId,
                        User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" }
                    }));
            _userRepository.Setup(p => p.Delete(uniAdmin.UserId)).Returns(Task.FromResult<bool>(true));

            //Act
            var a = await superAdminService.DeleteInstitutionOfEducationAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("User IsDeleted was updated", a.Object.Message);
        }
        [Fact]
        public async Task DeleteAdmin_NoAdminfound()
        {
            //Arrange
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(null));

            //Act

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.DeleteInstitutionOfEducationAdmin(uniAdmin.Id));
        }
        [Fact]
        public async Task DisableAdmin_ReturnsSuccessDisableMessage()
        {
            //Arrange
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetById(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                {
                    Id = uniAdmin.Id,
                    UserId = uniAdmin.UserId,
                    User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" }
                }));
            _institutionOfEducationAdminRepository.Setup(x => x.Disable(uniAdmin)).Returns(Task.FromResult("Admin IsBanned was set to true"));
            _mapperMock.Setup(x => x.Map<InstitutionOfEducationAdmin>(It.IsAny<InstitutionOfEducationAdminDTO>())).Returns(uniAdmin);

            //Act
            var a = await superAdminService.DisableInstitutionOfEducationAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("Admin IsBanned was set to true", a.Object.Message);
        }
        [Fact]
        public async Task DisableAdmin_ReturnsSuccessEnableMessage()
        {
            //Arrange
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetById(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                {
                    Id = uniAdmin.Id,
                    UserId = uniAdmin.UserId,
                    User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" },
                    IsBanned = true
                }));
            _institutionOfEducationAdminRepository.Setup(x => x.Enable(uniAdmin)).Returns(Task.FromResult("Admin IsBanned was set to false"));
            _mapperMock.Setup(x => x.Map<InstitutionOfEducationAdmin>(It.IsAny<InstitutionOfEducationAdminDTO>())).Returns(uniAdmin);

            //Act
            var a = await superAdminService.DisableInstitutionOfEducationAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("Admin IsBanned was set to false", a.Object.Message);
        }

    }
}
