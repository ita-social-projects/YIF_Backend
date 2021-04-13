using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class InstitutionOfEducationModeratorRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FakeUserManager<DbUser> _userManagerMock;
        private readonly InstitutionOfEducationModeratorRepository _institutionOfEducationModeratorRepository;
        private readonly List<InstitutionOfEducationModerator> _databaseInstitutionOfEducationModerators = new List<InstitutionOfEducationModerator>();
        private readonly InstitutionOfEducationModerator institutionOfEducationModerator = new InstitutionOfEducationModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };

        public InstitutionOfEducationModeratorRepositoryTests()
        {
            _dbContextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = new FakeUserManager<DbUser>();

            _institutionOfEducationModeratorRepository = new InstitutionOfEducationModeratorRepository(_mapperMock.Object, _dbContextMock.Object);
            _dbContextMock.Setup(p => p.InstitutionOfEducationModerators).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationModerator>(_databaseInstitutionOfEducationModerators));
        }

        [Fact]
        public async Task Create_UniModerator_ReturnsEmptyString()
        {
            string a = await _institutionOfEducationModeratorRepository.AddUniModerator(institutionOfEducationModerator);
            Assert.Equal(string.Empty, a);
        }
    }
}
