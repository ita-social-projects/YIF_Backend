using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class SchoolAdminRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly SchoolAdminRepository _schoolAdminRepository;

        private readonly DbUser _user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
        private readonly SchoolAdmin schoolAdmin = new SchoolAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", SchoolId = "007a43f8-7553-4eec-9e91-898a9cba37c9" };
        private readonly School school = new School { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub" };
        private readonly SchoolModerator schoolModerator = new SchoolModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", SchoolId = "007a43f8-7553-4eec-9e91-898a9cba37c9", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        private readonly List<SchoolAdmin> _databaseSchoolAdmins = new List<SchoolAdmin>();
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<School> _databaseSchools = new List<School>();
        private readonly List<SchoolModerator> _databaseSchoolModerators = new List<SchoolModerator>();

        public SchoolAdminRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new FakeUserManager<DbUser>();

            _schoolAdminRepository = new SchoolAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);

            _dbContextMock.Setup(p => p.SchoolAdmins).Returns(DbContextMock.GetQueryableMockDbSet<SchoolAdmin>(_databaseSchoolAdmins));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_databaseDbUsers));
            _dbContextMock.Setup(p => p.Schools).Returns(DbContextMock.GetQueryableMockDbSet<School>(_databaseSchools));
            _dbContextMock.Setup(p => p.SchoolModerators).Returns(DbContextMock.GetQueryableMockDbSet<SchoolModerator>(_databaseSchoolModerators));


            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            _databaseDbUsers.Add(_user);
            _databaseSchools.Add(school);
            _databaseSchoolAdmins.Add(schoolAdmin);
            _databaseSchoolModerators.Add(schoolModerator);
        }
        [Fact]
        public async Task Create_SchoolAdmin_ReturnsEmptyString()
        {
            string a = await _schoolAdminRepository.AddSchoolAdmin(schoolAdmin);
            Assert.Equal(string.Empty, a);
        }

        [Fact]
        public async Task DeleteAdmin_WrongId_ReturnsNull()
        {
            //Arrange
            //Act
            string a = await _schoolAdminRepository.Delete("sdfsdf");

            //Assert
            Assert.Null(a);
        }

        [Fact]
        public async Task DeleteAdmin_ReturnsSuccessDeleteString()
        {
            //Arrange
            _userManagerMock.ResponseObject = _user;
            //Act
            string a = await _schoolAdminRepository.Delete("3b16d794-7aaa-4ca5-943a-36d328f86ed3");
            //Assert
            Assert.Equal("User IsDeleted was updated", a);
        }

        [Fact]
        public async Task GetByUniversityId_ReturnsNullForBadId()
        {
            //Arrange
            //Act
            var a = await _schoolAdminRepository.GetBySchoolId("sdfs");
            //Assert
            Assert.Null(a);
        }
    }
}
