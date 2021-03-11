using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class UniversityAdminRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly UniversityAdminRepository _universityAdminRepository;

        private readonly DbUser _user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
        private readonly UniversityAdmin uniAdmin = new UniversityAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };
        private readonly University uni = new University { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub", ImagePath = "Image1Path" };
        private readonly UniversityModerator universityModerator = new UniversityModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        private readonly List<UniversityAdmin> _databaseUniAdmins = new List<UniversityAdmin>();
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<University> _databaseUniversities = new List<University>();
        private readonly List<UniversityModerator> _databaseUniversityModerators = new List<UniversityModerator>();

        public UniversityAdminRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new FakeUserManager<DbUser>();

            _universityAdminRepository = new UniversityAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);

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
        public async Task GetAllUniAdmins_ShouldReturnAllAdminsFromDatabase()
        {
            // Arrange
            var _dbContextMock = new Mock<IApplicationDbContext>();
            var _mapperMock = new Mock<IMapper>();
            var _userManagerMock = new FakeUserManager<DbUser>();

            var _universityAdminRepository = new UniversityAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);
            var _userStub = new DbUser { Id = "UserId", UserName = "Name", Email = "example@mail.com" };
            var _admin = new UniversityAdmin { Id = "adminId", UserId = "UserId", User = _userStub };

            var _listDTO = new List<UniversityAdminDTO>();
            _listDTO.Add(new UniversityAdminDTO { Id = "adminId", UserId = "UserId" });
            var _listView = new List<UniversityAdmin>();
            _listView.Add(new UniversityAdmin { Id = "adminId", UserId = "UserId" });

            _dbContextMock.Setup(p => p.UniversityAdmins).Returns(DbContextMock.GetQueryableMockDbSet<UniversityAdmin>(new List<UniversityAdmin> { _admin }));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(new List<DbUser> { _userStub }));
            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();

            var _universityAdminsDTO = new List<UniversityAdmin>()
            {
                _admin
            };

            var _listViewModel = new List<UniversityAdminDTO>()
            {
                new UniversityAdminDTO { Id = _admin.Id, UniversityId = _admin.UniversityId }
            };
            _mapperMock.Setup(s => s.Map<IEnumerable<UniversityAdminDTO>>(_universityAdminsDTO)).Returns(_listViewModel);
            // Act
            var list = (await _universityAdminRepository.GetAllUniAdmins()).ToList();

            //Assert
            Assert.Equal(_admin.Id, list[0].Id);
        }
        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnEmpty_IfThereAreNoAdmins()
        {
            // Arrange
            var _dbContextMock = new Mock<IApplicationDbContext>();
            _dbContextMock.Setup(p => p.UniversityAdmins).Returns(DbContextMock.GetQueryableMockDbSet<UniversityAdmin>(new List<UniversityAdmin>()));
            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            var _universityAdminRepository = new UniversityAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);

            //Act
            var list = await _universityAdminRepository.GetAllUniAdmins();

            //Assert
            Assert.Empty(list);
        }
        [Fact]
        public async Task Create_UniAdmin_ReturnsEmptyString()
        {
            //Act
            string a = await _universityAdminRepository.AddUniAdmin(uniAdmin);
            //Assert
            Assert.Equal(string.Empty, a);
        }
    }
}
