﻿using AutoMapper;
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
    public class SpecialityRepositoryTests
    {
        private readonly Mock<IApplicationDbContext> _context = new Mock<IApplicationDbContext>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly SpecialityRepository _testRepo;

        private readonly Direction _directon;
        private readonly Speciality _specialty1 = new Speciality() { Id = "id" };
        private readonly Speciality _specialty2 = new Speciality();
        private readonly List<Direction> _dbDirections;
        private readonly List<Speciality> _dbSpecialties;

        public SpecialityRepositoryTests()
        {
            _testRepo = new SpecialityRepository(_context.Object, _mapper.Object);

            _directon = new Direction();
            //_specialty = new Speciality();
            _dbDirections = new List<Direction>() { _directon };
            _dbSpecialties = new List<Speciality>() { _specialty1, _specialty2 };

            _context.Setup(p => p.Directions).Returns(DbContextMock.GetQueryableMockDbSet<Direction>(_dbDirections));
            _context.Setup(p => p.Specialities).Returns(DbContextMock.GetQueryableMockDbSet<Speciality>(_dbSpecialties));
            _context.Setup(s => s.SaveChangesAsync()).Verifiable();
        }

        [Fact]
        public async Task Delete_NotImplements_Yet()
        {
            await Assert.ThrowsAsync<NotImplementedException>(() => _testRepo.Delete("id"));
        }
    }
}
