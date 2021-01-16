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
    public class UniversityModeratorRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly UniversityModeratorRepository _universtityModeratorRepository;

        private readonly List<UniversityModerator> _databaseUniversityModerators = new List<UniversityModerator>();
        private readonly UniversityModerator universityModerator = new UniversityModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", UniversityId = "007a43f8-7553-4eec-9e91-898a9cba37c9", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        public UniversityModeratorRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new FakeUserManager<DbUser>();

            _universtityModeratorRepository = new UniversityModeratorRepository(_mapperMock.Object, _userManagerMock, _dbContextMock.Object);
            _dbContextMock.Setup(p => p.UniversityModerators).Returns(DbContextMock.GetQueryableMockDbSet<UniversityModerator>(_databaseUniversityModerators));

            //_databaseUniversityModerators.Add(universityModerator);
        }

        [Fact]
        public async Task Create_UniModerator_ReturnsEmptyString()
        {
            string a = await _universtityModeratorRepository.AddUniModerator(universityModerator);
            Assert.Equal(string.Empty, a);
        }
    }
}
