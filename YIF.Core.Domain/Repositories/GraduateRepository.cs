using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class GraduateRepository : IGraduateRepository<Graduate, GraduateDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GraduateRepository(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<GraduateDTO>> Find(Expression<Func<Graduate, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<GraduateDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GraduateDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<GraduateDTO> GetByUserId(string userId)
        {
            var graduate = await _context.Graduates
                .Where(g => g.UserId.Equals(userId))
                .FirstOrDefaultAsync();
            return _mapper.Map<GraduateDTO>(graduate);
        }

        public Task<bool> Update(Graduate item)
        {
            throw new NotImplementedException();
        }
    }
}
