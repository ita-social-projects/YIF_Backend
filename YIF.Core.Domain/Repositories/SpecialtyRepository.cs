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
    public class SpecialtyRepository : IRepository<Specialty, SpecialtyDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialtyRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Update(Specialty specialty)
        {
            if (specialty != null)
            {
                if (_context.Specialties.Find(specialty) != null)
                {
                    _context.Specialties.Update(specialty);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        // Not implemented, as the logic will be determined in the future
        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SpecialtyDTO> Get(string id)
        {
            var specialty = await _context.Specialties.Include(s => s.Direction).FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SpecialtyDTO>(specialty);
        }

        public async Task<IEnumerable<SpecialtyDTO>> GetAll()
        {
            var list = await _context.Specialties.Include(s => s.Direction).ToListAsync();
            return _mapper.Map<IEnumerable<SpecialtyDTO>>(list);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<SpecialtyDTO>> Find(Expression<Func<Specialty, bool>> predicate)
        {
            var specialties = await _context.Specialties.Where(predicate)
                .AsNoTracking().ToListAsync();

            if (specialties != null || specialties.Count > 0)
            {
                return _mapper.Map<IEnumerable<SpecialtyDTO>>(specialties);
            }

            return null;
        }
    }
}
