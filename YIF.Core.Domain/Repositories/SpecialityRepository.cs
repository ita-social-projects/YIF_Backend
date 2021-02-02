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
    public class SpecialityRepository : IRepository<Speciality, SpecialityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Update(Speciality speciality)
        {
            if (speciality != null)
            {
                if (_context.Specialities.Find(speciality) != null)
                {
                    _context.Specialities.Update(speciality);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SpecialityDTO> Get(string id)
        {
            var specialty = await _context.Specialities.Include(s => s.Direction).FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SpecialityDTO>(specialty);
        }

        public async Task<IEnumerable<SpecialityDTO>> GetAll()
        {
            var list = await _context.Specialities.Include(s => s.Direction).ToListAsync();
            return _mapper.Map<IEnumerable<SpecialityDTO>>(list);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<SpecialityDTO>> Find(Expression<Func<Speciality, bool>> predicate)
        {
            var specialities = await _context.Specialities.Where(predicate)
                .AsNoTracking().ToListAsync();

            if (specialities != null || specialities.Count > 0)
            {
                return _mapper.Map<IEnumerable<SpecialityDTO>>(specialities);
            }

            return null;
        }
    }
}
