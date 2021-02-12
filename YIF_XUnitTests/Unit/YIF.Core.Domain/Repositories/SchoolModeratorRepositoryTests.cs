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
    public class SchoolModeratorRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly SchoolModeratorRepository _schoolModeratorRepository;

        private readonly List<SchoolModerator> _databaseSchoolModerators = new List<SchoolModerator>();
        private readonly SchoolModerator schoolModerator = new SchoolModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", SchoolId = "007a43f8-7553-4eec-9e91-898a9cba37c9", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };
        public SchoolModeratorRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();

            _schoolModeratorRepository = new SchoolModeratorRepository(_dbContextMock.Object);
            _dbContextMock.Setup(p => p.SchoolModerators).Returns(DbContextMock.GetQueryableMockDbSet<SchoolModerator>(_databaseSchoolModerators));
        }

        [Fact]
        public async Task Create_UniModerator_ReturnsEmptyString()
        {
            string a = await _schoolModeratorRepository.AddSchoolModerator(schoolModerator);
            Assert.Equal(string.Empty, a);
        }
    }
}
