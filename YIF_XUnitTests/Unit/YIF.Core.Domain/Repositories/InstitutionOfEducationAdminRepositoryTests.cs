using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class InstitutionOfEducationAdminRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly InstitutionOfEducationAdminRepository _institutionOfEducationAdminRepository;

        private readonly DbUser _user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
        private readonly InstitutionOfEducationAdmin uniAdmin = new InstitutionOfEducationAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", InstitutionOfEducationId = "007a43f8-7553-4eec-9e91-898a9cba37c9", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };
        private readonly InstitutionOfEducation uni = new InstitutionOfEducation { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub", ImagePath = "Image1Path" };
        private readonly InstitutionOfEducationModerator institutionOfEducationModerator = new InstitutionOfEducationModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        private readonly List<InstitutionOfEducationAdmin> _databaseUniAdmins = new List<InstitutionOfEducationAdmin>();
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<InstitutionOfEducation> _databaseInstitutionOfEducations = new List<InstitutionOfEducation>();
        private readonly List<InstitutionOfEducationModerator> _databaseInstitutionOfEducationModerators = new List<InstitutionOfEducationModerator>();

        public InstitutionOfEducationAdminRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new FakeUserManager<DbUser>();

            _institutionOfEducationAdminRepository = new InstitutionOfEducationAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);

            _dbContextMock.Setup(p => p.InstitutionOfEducationAdmins).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationAdmin>(_databaseUniAdmins));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_databaseDbUsers));
            _dbContextMock.Setup(p => p.InstitutionOfEducations).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducation>(_databaseInstitutionOfEducations));
            _dbContextMock.Setup(p => p.InstitutionOfEducationModerators).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationModerator>(_databaseInstitutionOfEducationModerators));


            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            _databaseDbUsers.Add(_user);
            _databaseInstitutionOfEducations.Add(uni);
            _databaseUniAdmins.Add(uniAdmin);
            _databaseInstitutionOfEducationModerators.Add(institutionOfEducationModerator);
        }

        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnAllAdminsFromDatabase()
        {
            // Arrange
            var _dbContextMock = new Mock<IApplicationDbContext>();
            var _mapperMock = new Mock<IMapper>();
            var _userManagerMock = new FakeUserManager<DbUser>();

            var _institutionOfEducationAdminRepository = new InstitutionOfEducationAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);
            var _userStub = new DbUser { Id = "UserId", UserName = "Name", Email = "example@mail.com" };
            var _admin = new InstitutionOfEducationAdmin { Id = "adminId", UserId = "UserId", User = _userStub };

            var _listDTO = new List<InstitutionOfEducationAdminDTO>();
            _listDTO.Add(new InstitutionOfEducationAdminDTO { Id = "adminId", UserId = "UserId" });
            var _listView = new List<InstitutionOfEducationAdmin>();
            _listView.Add(new InstitutionOfEducationAdmin { Id = "adminId", UserId = "UserId" });

            _dbContextMock.Setup(p => p.InstitutionOfEducationAdmins).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationAdmin>(new List<InstitutionOfEducationAdmin> { _admin }));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(new List<DbUser> { _userStub }));
            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();

            var _institutionOfEducationAdminsDTO = new List<InstitutionOfEducationAdmin>()
            {
                _admin
            };

            var _listViewModel = new List<InstitutionOfEducationAdminDTO>()
            {
                new InstitutionOfEducationAdminDTO { Id = _admin.Id, InstitutionOfEducationId = _admin.InstitutionOfEducationId }
            };
            _mapperMock.Setup(s => s.Map<IEnumerable<InstitutionOfEducationAdminDTO>>(_institutionOfEducationAdminsDTO)).Returns(_listViewModel);
            // Act
            var list = (await _institutionOfEducationAdminRepository.GetAllUniAdmins()).ToList();

            //Assert
            Assert.Equal(_admin.Id, list[0].Id);
        }
        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnEmpty_IfThereAreNoAdmins()
        {
            // Arrange
            var _dbContextMock = new Mock<IApplicationDbContext>();
            _dbContextMock.Setup(p => p.InstitutionOfEducationAdmins).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationAdmin>(new List<InstitutionOfEducationAdmin>()));
            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            var _institutionOfEducationAdminRepository = new InstitutionOfEducationAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);

            //Act
            var list = await _institutionOfEducationAdminRepository.GetAllUniAdmins();

            //Assert
            Assert.Empty(list);
        }
        [Fact]
        public async Task Create_UniAdmin_ReturnsEmptyString()
        {
            //Act
            string a = await _institutionOfEducationAdminRepository.AddUniAdmin(uniAdmin);
            //Assert
            Assert.Equal(string.Empty, a);
        }

        [Fact]
        public async Task GetAllUniversities()
        {
            // Arrange
            var request = "/api/SuperAdmin/GetAllUniversities";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task DeleteUniversityAdmin()
        {
            // Arrange
            var admin = _context.UniversityAdmins.First();

            // Act
            var response = await _client.DeleteAsync(string.Format("/api/SuperAdmin/DeleteUniversityAdmin/{0}", admin.Id));

            // Assert
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task DisableUniversityAdmin()
        {
            // Arrange
            var admin = _context.UniversityAdmins.First();

            // Act
            var response = await _client.PostAsync(string.Format("/api/SuperAdmin/DisableUniversityAdmin/{0}", admin.Id), ContentHelper.GetStringContent(admin));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
