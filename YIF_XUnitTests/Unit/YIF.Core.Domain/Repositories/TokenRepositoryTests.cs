using AutoMapper;
using Moq;
using Superpower.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Repositories;

namespace YIF_XUnitTests.Unit.YIF.Core.Domain.Repositories
{
    public class TokenRepositoryTests
    {
        private static readonly Mock<IApplicationDbContext> _dbContextMock = new Mock<IApplicationDbContext>();
        private static readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private static readonly TokenRepository _testRepo = new TokenRepository(_dbContextMock.Object);

        //private static readonly List<Token> _dataStub = new List<Token>();

        public TokenRepositoryTests()
        {
            //_dbContextMock.Setup(p => p.Tokens).Returns(DbContextMock.GetQueryableMockDbSet<Token>(_dataStub));
            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
        }
    }
}
