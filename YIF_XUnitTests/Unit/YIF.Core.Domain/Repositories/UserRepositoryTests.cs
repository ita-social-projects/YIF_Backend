using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Others;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly Mock<EFDbContext> _dbEFContextMock;
        private readonly UserRepository _testRepo;

        // User for check Create method
        private readonly DbUser _newUserStub;
        private readonly UserDTO _newUserDTOStub;
        private readonly string _newUserPassword;

        // Users for check Update and Delete methods
        private readonly DbUser _userStub;
        private readonly UserDTO _userDTOStub;
        private readonly DbUser _userStub2;
        private readonly DbUser _userStub3;

        // Users for check Find methods from DbSet
        private readonly List<UserDTO> _listDTO;
        // Fake database
        private readonly List<DbUser> _dataStub;


        public UserRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new FakeUserManager<DbUser>();
            _dbEFContextMock = new Mock<EFDbContext>();
            _testRepo = new UserRepository(_dbContextMock.Object, _mapperMock.Object, _userManagerMock, _dbEFContextMock.Object);

            _newUserPassword = "QWerty-1";
            _listDTO = new List<UserDTO>();

            _userStub = new DbUser { Id = Guid.NewGuid().ToString("D"), UserName = "Safwan Wickens", Email = "cfarid.nadji2r@devist.com" };
            _newUserStub = new DbUser { Id = Guid.NewGuid().ToString("D"), UserName = "Gia Vang", Email = "dill.pazee@azel.xyz" };
            _userStub2 = new DbUser { Id = Guid.NewGuid().ToString("D"), UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
            _userStub3 = new DbUser { Id = Guid.NewGuid().ToString("D") };

            _userDTOStub = new UserDTO { Id = Guid.NewGuid().ToString("D"), UserName = "Safwan Wickens", Email = "cfarid.nadji2r@devist.com" };
            _newUserDTOStub = new UserDTO { Id = Guid.NewGuid().ToString("D"), UserName = "Gia Vang", Email = "dill.pazee@azel.xyz" };

            _listDTO.Add(new UserDTO { Id = Guid.NewGuid().ToString("D"), UserName = "Safwan Wickens", Email = "cfarid.nadji2r@devist.com" });
            _dataStub = new List<DbUser> { _userStub, _userStub2 };

            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_dataStub));
            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
        }

        [Fact]
        public async Task Create_ShouldAddUserToDatabase_WhenUserIsValid()
        {
            // Arrange
            _userManagerMock.ResIsSucces = IdentityResult.Success;
            _dbContextMock.Setup(s => s.AddAsync(It.IsAny<IdentityUser>())).Verifiable();
            // Act
            var errors = await _testRepo.Create(_newUserStub, It.IsAny<IdentityUser>(), _newUserPassword, ProjectRoles.Graduate);
            // Assert
            Assert.Empty(errors);
        }

        [Fact]
        public async Task Create_ShouldReturnErrors_WhenCreatingUserIsFailed()
        {
            // Act
            _userManagerMock.ResIsSucces = IdentityResult.Failed();
            var errors = await _testRepo.Create(null, null, null, null);
            // Assert
            Assert.True(errors.Length > 0);
        }

        [Fact]
        public async Task Update_ShouldChangeUserInDBAndReturnTrue_WhenUserIsValidAndFoundInDatabase()
        {
            // Arrange
            _dbContextMock.Setup(s => s.Users.Find(_userStub)).Returns(_userStub);
            _userStub.UserName = "New Name";
            // Act
            var result = await _testRepo.Update(_userStub);
            // Assert
            Assert.True(result);
            Assert.Equal(_userStub.UserName, _dataStub[0].UserName);
        }

        [Fact]
        public async Task Update_ShouldReturnFalse_WhenUserIsValidButNotFoundInDatabase()
        {
            // Arrange
            _dbContextMock.Setup(s => s.Users.Find(_userStub3)).Returns<DbUser>(null);
            // Act
            var result = await _testRepo.Update(_userStub3);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Update_ShouldReturnFalse_WhenIsNoUser()
        {
            // Act
            var result = await _testRepo.Update(null);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_ShouldDeleteUserFromDBAndReturnTrue_WhenUserIsValidAndFoundInDatabase()
        {
            // Arrange
            _dbContextMock.Setup(s => s.Users.Find(_userStub2.Id)).Returns(_userStub2);
            // Act
            var result = await _testRepo.Delete(_userStub2.Id);
            // Assert
            Assert.True(result);
            Assert.DoesNotContain(_userStub2, _dataStub);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenUserNotFoundInDatabase()
        {
            // Arrange
            _dbContextMock.Setup(s => s.Users.Find(_userStub3.Id)).Returns<DbUser>(null);
            // Act
            var result = await _testRepo.Delete(_userStub3.Id);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Get_ShouldReturnUserDTO_WhenUserIsFoundInDatabase()
        {
            // Arrange
            _dbContextMock.Setup(s => s.Users.FindAsync(_userStub.Id)).ReturnsAsync(_userStub);
            _mapperMock.Setup(s => s.Map<UserDTO>(_userStub)).Returns(_userDTOStub);
            // Act
            var result = await _testRepo.Get(_userStub.Id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(_listDTO[0].Id, result.Id);
        }

        [Fact]
        public async Task Get_ShouldReturnException_WhenUserNotFoundInDatabase()
        {
            // Arrange
            _dbContextMock.Setup(s => s.Users.FindAsync(_userStub3.Id)).ReturnsAsync((DbUser)null);
            // Assert
            var exeption = await Assert.ThrowsAsync<KeyNotFoundException>(() => _testRepo.Get(_userStub3.Id));
            Assert.StartsWith("User not found", exeption.Message);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllUsersFromDatabase()
        {
            // Arrange
            List<UserDTO> listDTO2 = new List<UserDTO> {
                new UserDTO { Id = _dataStub[0].Id, UserName = _dataStub[0].UserName, Email = _dataStub[0].Email },
                new UserDTO { Id = _dataStub[1].Id, UserName = _dataStub[1].UserName, Email = _dataStub[1].Email }
            };
            _mapperMock.Setup(s => s.Map<IEnumerable<UserDTO>>(_dataStub)).Returns(listDTO2);
            // Act
            List<UserDTO> list = (await _testRepo.GetAll()).ToList();
            //Assert
            Assert.Equal(list[0].Id, listDTO2[0].Id);
            Assert.Equal(list[1].Id, listDTO2[1].Id);
            Assert.Equal(list[1].UserName, listDTO2[1].UserName);
        }

        [Fact]
        public void Dispose_ShouldDisposeDatabase()
        {
            // Arrange
            var context = new Mock<IApplicationDbContext>();
            var result = false;
            context.Setup(x => x.Dispose()).Callback(() => result = true);
            // Act
            var repo = new UserRepository(context.Object, _mapperMock.Object, _userManagerMock, _dbEFContextMock.Object);
            repo.Dispose();
            // Assert
            Assert.True(result);
        }
    }
}
