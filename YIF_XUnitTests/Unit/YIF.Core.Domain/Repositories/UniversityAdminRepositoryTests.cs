using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class UniversityAdminRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly UniversityAdminRepository _universtityAdminRepository;

        private readonly DbUser _user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
        private readonly UniversityAdmin uniAdmin = new UniversityAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9" };
        private readonly University uni = new University { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub", ImagePath = "Image1Path" };
        private readonly UniversityModerator universityModerator = new UniversityModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        private readonly List<UniversityAdmin> _databaseUniAdmins = new List<UniversityAdmin>();
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<University> _databaseUniversities = new List<University>();
        private readonly List<UniversityModerator> _databaseUniversityModerators = new List<UniversityModerator>();

        public UniversityAdminRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new FakeUserManager<DbUser>();
            //_dbEFContextMock = new Mock<EFDbContext>();

            _universtityAdminRepository = new UniversityAdminRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock);

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
        public async Task Create_UniAdmin_ReturnsEmptyString()
        { 
            string a = await _universtityAdminRepository.AddUniAdmin(uniAdmin);
            Assert.Equal(string.Empty, a);
        }

        [Fact]
        public async Task DeleteAdmin_WrongId_ReturnsNull()
        {
            //Arrange
            //Act
            string a = await _universtityAdminRepository.Delete("sdfsdf");
            
            //Assert
            Assert.Equal(a, null);
        }

        [Fact]
        public async Task DeleteAdmin_ReturnsSuccessDeleteString()
        {
            //Arrange
            _userManagerMock.Obj = _user;
            //Act
            string a = await _universtityAdminRepository.Delete("3b16d794-7aaa-4ca5-943a-36d328f86ed3");
            //Assert
            Assert.Equal(a,"User IsDeleted was updated");
        }

        //[Fact]
        //public async Task GetByUniversityId_RetursNullForBadId()
        //{
        //    //Arrange
        //    //Act
        //    var a = await _universtityAdminRepository.GetByUniversityId("sdfs");
        //    //Assert
        //    Assert.Equal(a, null);
        //}

        //[Fact] // not finished no dea how to fx it
        //public async Task GetByUniversityId_ReturnsValidDto()
        //{
        //    //Arrange

        //    //Act
        //    var a = await _universtityAdminRepository.GetByUniversityId("007a43f8-7553-4eec-9e91-898a9cba37c9");
        //    //Assert
        //    Assert.Equal(a.Id, uniAdmin.Id);
        //}

        //[Fact] // same as prevous one
        //public async Task GetByUniversityIdWithoutIsDeletedCheck()
        //{
        //    //Arrange
        //    //Act
        //    var a = await _universtityAdminRepository.GetByUniversityIdWithoutIsDeletedCheck("007a43f8-7553-4eec-9e91-898a9cba37c9");
        //    //Assert
        //    Assert.Equal(a.Id, uniAdmin.Id);
        //}
    }
}
