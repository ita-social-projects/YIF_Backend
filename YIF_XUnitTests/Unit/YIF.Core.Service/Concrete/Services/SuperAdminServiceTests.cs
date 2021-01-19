using AutoMapper;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;
using YIF.Core.Domain.DtoModels.SchoolAdmin;
using YIF.Core.Domain.DtoModels.SchoolModerator;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class SuperAdminServiceTests
    {
        private readonly Mock<IUserRepository<DbUser, UserDTO>> _userRepository;
        private readonly Mock<FakeUserManager<DbUser>> _userManager;
        private readonly FakeSignInManager<DbUser> _signInManager;
        private readonly Mock<IJwtService> _jwtService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUniversityAdminRepository<UniversityAdminDTO>> _universityAdminRepository;
        private readonly Mock<IRepository<University, UniversityDTO>> _universityRepository;
        private readonly Mock<IUniversityModeratorRepository<UniversityModeratorDTO>> _universityModeratorRepository;
        private readonly Mock<ISchoolRepository<SchoolDTO>> _schoolRepository;
        private readonly Mock<ISchoolAdminRepository<SchoolAdminDTO>> _schoolAdminRepository;
        private readonly Mock<ISchoolModeratorRepository<SchoolModeratorDTO>> _schoolModeratorRepository;
        private readonly Mock<ITokenRepository> _tokenRepository;
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<ITokenRepository> _tokenRepostory;
        private readonly Mock<IUniversityRepository<University, UniversityDTO>> _universityRepositoryAdditonal;
        private readonly SuperAdminService superAdminService;

        private readonly DbUser _user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
        private readonly UniversityAdmin uniAdmin = new UniversityAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9" };
        private readonly University uni = new University { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub", ImagePath = "Image1Path" };
        private readonly UniversityModerator universityModerator = new UniversityModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        private readonly List<UniversityAdmin> _databaseUniAdmins = new List<UniversityAdmin>();
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<University> _databaseUniversities = new List<University>();
        private readonly List<UniversityModerator> _databaseUniversityModerators = new List<UniversityModerator>();

        public readonly UniversityAdminApiModel model = new UniversityAdminApiModel
        {
            UniversityName = "Name",
            Email = "Email",
            Password = "Password"
        };
        public SuperAdminServiceTests()
        {
            _userRepository = new Mock<IUserRepository<DbUser, UserDTO>>();
            _userManager = new Mock<FakeUserManager<DbUser>>();
            _signInManager = new FakeSignInManager<DbUser>(_userManager);
            _jwtService = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();
            _universityAdminRepository = new Mock<IUniversityAdminRepository<UniversityAdminDTO>>();
            _universityRepository = new Mock<IRepository<University, UniversityDTO>>();
            _universityModeratorRepository = new Mock<IUniversityModeratorRepository<UniversityModeratorDTO>>();
            _schoolRepository = new Mock<ISchoolRepository<SchoolDTO>>();
            _schoolAdminRepository = new Mock<ISchoolAdminRepository<SchoolAdminDTO>>();
            _schoolModeratorRepository = new Mock<ISchoolModeratorRepository<SchoolModeratorDTO>>();
            _tokenRepository = new Mock<ITokenRepository>();
            _dbContextMock = new Mock<IApplicationDbContext>();
            _tokenRepostory = new Mock<ITokenRepository>();
            _universityRepositoryAdditonal = new Mock<IUniversityRepository<University, UniversityDTO>>();
            superAdminService = new SuperAdminService(
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
                                                    _tokenRepostory.Object,
                                                    _universityRepositoryAdditonal.Object);

            _dbContextMock.Setup(p => p.UniversityAdmins).Returns(DbContextMock.GetQueryableMockDbSet<UniversityAdmin>(_databaseUniAdmins));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_databaseDbUsers));
            _dbContextMock.Setup(p => p.Universities).Returns(DbContextMock.GetQueryableMockDbSet<University>(_databaseUniversities));
            _dbContextMock.Setup(p => p.UniversityModerators).Returns(DbContextMock.GetQueryableMockDbSet<UniversityModerator>(_databaseUniversityModerators));


            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            _databaseDbUsers.Add(_user);
            _databaseUniversities.Add(uni);
            _databaseUniAdmins.Add(uniAdmin);
            _databaseUniversityModerators.Add(universityModerator);
        }
        [Fact]
        public async Task AddUniAdmin_NoUniFound_returnsResult()
        {
            List<UniversityDTO> listNull = new List<UniversityDTO>
            {
                null
            }; 
            _universityRepository.Setup(p => p.Find(It.IsAny<Expression<Func<University, bool>>>()))
                                                                                                .ReturnsAsync(listNull);

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
            SchoolUniAdminDeleteApiModel model = new SchoolUniAdminDeleteApiModel { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3" };
            //Arrange
            _userManager.Setup(p => p.FindByIdAsync(_user.Id)).Returns(Task.FromResult<DbUser>(_user));
            _universityAdminRepository.Setup(p => p.Delete(model.Id)).Returns(Task.FromResult<string>("User IsDeleted was updated"));
            //Act
            var a = await superAdminService.DeleteUniversityAdmin(model);
            //Assert
            Assert.Equal("User IsDeleted was updated", a.Object.Message);
        }

    }
}
