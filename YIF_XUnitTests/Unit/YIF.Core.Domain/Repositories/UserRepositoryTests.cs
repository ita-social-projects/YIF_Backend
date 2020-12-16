using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class UserRepositoryTests
    {
        private static readonly Mock<IApplicationDbContext> _dbContextMock = new Mock<IApplicationDbContext>();
        private static readonly UserRepository _testRepo = new UserRepository(_dbContextMock.Object);

        // User for check Create method
        private static readonly DbUser _newUserStub = new DbUser
        {
            Id = Guid.NewGuid().ToString("D"),
            UserName = "Gia Vang",
            Email = "dill.pazee@azel.xyz"
        };
        // Users for check Update and Delete methods
        private static readonly DbUser _userStub = new DbUser();
        private static readonly DbUser _userStub2 = new DbUser
        {
            Id = Guid.NewGuid().ToString("D"),
            UserName = "Jeremiah Gibson",
            Email = "shadj_hadjf@maliberty.com"
        };
        private static readonly DbUser _userStub3 = new DbUser { Id = Guid.NewGuid().ToString("D"), };
        // Users for check Find methods from DbSet
        private static readonly List<UserDTO> _listDTO = new List<UserDTO> { new UserDTO() };
        // Fake database
        private static readonly List<DbUser> _dataStub = new List<DbUser> { _userStub, _userStub2 };


        public UserRepositoryTests()
        {
            _listDTO[0].Id = _userStub.Id = Guid.NewGuid().ToString("D");
            _listDTO[0].UserName = _userStub.UserName = "Safwan Wickens";
            _listDTO[0].Email = _userStub.Email = "cfarid.nadji2r@devist.com";

            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_dataStub));
            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
        }



        [Fact]
        public async Task Create_ShouldAddUserToDatabase_WhenUserIsValid()
        {
            // Arrange
            _dbContextMock.Setup(s => s.Users.FindAsync(_newUserStub)).ReturnsAsync(_newUserStub);
            // Act
            var result = await _testRepo.Create(_newUserStub);
            // Assert
            Assert.Contains(_newUserStub, _dataStub);
            Assert.Equal(_newUserStub.Id, result.Id);
            Assert.Equal(_newUserStub.UserName, result.UserName);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenIsNoUser()
        {
            // Act
            var result = await _testRepo.Create(null);
            // Assert
            Assert.Null(result);
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
        public async Task GetAll_ShouldReturnFalse_WhenUserNotFoundInDatabase()
        {
            // Arrange
            List<UserDTO> listDTO2 = new List<UserDTO> {
                new UserDTO { Id = _dataStub[0].Id, UserName = _dataStub[0].UserName, Email = _dataStub[0].Email },
                new UserDTO { Id = _dataStub[1].Id, UserName = _dataStub[1].UserName, Email = _dataStub[1].Email }
            };
            // Act
            List<UserDTO> list = (await _testRepo.GetAll()).ToList();
            //Assert
            Assert.Equal(list[0].Id, listDTO2[0].Id);
            Assert.Equal(list[1].UserName, listDTO2[1].UserName);
            Assert.Equal(list[1].Id, listDTO2[1].Id);
        }

        [Fact]
        public void Dispose_ShouldDisposeDatabase()
        {
            // Arrange
            var context = new Mock<IApplicationDbContext>();
            var result = false;
            context.Setup(x => x.Dispose()).Callback(() => result = true);
            // Act
            var repo = new UserRepository(context.Object);
            repo.Dispose();
            // Assert
            Assert.True(result);
        }
    }
}
