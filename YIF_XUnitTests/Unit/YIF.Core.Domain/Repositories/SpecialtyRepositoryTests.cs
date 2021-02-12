using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class SpecialtyRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _context = new Mock<IApplicationDbContext>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly SpecialtyRepository _testRepo;

        private readonly Direction _direction;
        private readonly Specialty _specialty1 = new Specialty() { Id = "id" };
        private readonly Specialty _specialty2 = new Specialty();
        private readonly List<Direction> _dbDirections;
        private readonly List<Specialty> _dbSpecialties;

        public SpecialtyRepositoryTests()
        {
            _testRepo = new SpecialtyRepository(_context.Object, _mapper.Object);

            _direction = new Direction();
            _dbDirections = new List<Direction>() { _direction };
            _dbSpecialties = new List<Specialty>() { _specialty1, _specialty2 };

            _context.Setup(p => p.Directions).Returns(DbContextMock.GetQueryableMockDbSet<Direction>(_dbDirections));
            _context.Setup(p => p.Specialties).Returns(DbContextMock.GetQueryableMockDbSet<Specialty>(_dbSpecialties));
            _context.Setup(s => s.SaveChangesAsync()).Verifiable();
        }

        [Fact]
        public async Task Delete_NotImplements_Yet()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => _testRepo.Delete("id"));
        }

        [Fact]
        public async Task Update_ShouldReturnTrue_IfSpecialtyFound()
        {
            // Arrange
            _context.Setup(s => s.Specialties.Find(_specialty1)).Returns(_specialty1);
            // Act
            var result = await _testRepo.Update(_specialty1);
            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Update_ShouldReturnFalse_IfSpecialtyNotFound()
        {
            // Arrange
            _context.Setup(s => s.Specialties.Find(_specialty1)).Returns<Specialty>(null);
            // Act
            var result = await _testRepo.Update(_specialty1);
            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Dispose_ShouldDisposeContext()
        {
            // Arrange
            var context = new Mock<IApplicationDbContext>();
            var result = false;
            context.Setup(x => x.Dispose()).Callback(() => result = true);
            // Act
            var repo = new SpecialtyRepository(context.Object, _mapper.Object);
            repo.Dispose();
            // Assert
            Assert.True(result);
        }
    }
}
