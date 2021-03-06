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

        public async Task<bool> Update(Graduate item)
        {
            _context.Graduates.Update(item);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        // Not implemented, as the logic will be determined in the future
        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<GraduateDTO>> Find(Expression<Func<Graduate, bool>> predicate)
        {
            var graduates = await _context.Graduates.Where(predicate).AsNoTracking().ToListAsync();
            return _mapper.Map<IEnumerable<GraduateDTO>>(graduates);
        }

        public async Task<GraduateDTO> Get(string id)
        {
            var graduate = await _context.Graduates.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<GraduateDTO>(graduate);
        }

        public async Task<IEnumerable<GraduateDTO>> GetAll()
        {
            var graduates = await _context.Graduates.ToListAsync();
            return _mapper.Map<IEnumerable<GraduateDTO>>(graduates);
        }

        public async Task<GraduateDTO> GetByUserId(string userId)
        {
            var graduate = await _context.Graduates
                .AsNoTracking()
                .Where(g => g.UserId.Equals(userId))
                .FirstOrDefaultAsync();
            return _mapper.Map<GraduateDTO>(graduate);
        }
    }
}
