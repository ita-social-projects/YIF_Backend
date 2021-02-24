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
    public class SpecialityToUniversityRepository : IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialityToUniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(SpecialtyToUniversity item)
        {
            _context.SpecialtyToUniversities.Update(item);
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

        public Task<IEnumerable<SpecialtyToUniversityDTO>> Find(Expression<Func<SpecialtyToUniversity, bool>> predicate)
        {
            var list = _context.SpecialtyToUniversities
                .Include(x => x.Specialty)
                .Include(x => x.University)
                .Include(x => x.SpecialtyInUniversityDescription)
                .Where(predicate)
                .ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<SpecialtyToUniversityDTO>>(list));
            }

            return null;
        }

        public async Task<SpecialtyToUniversityDTO> Get(string id)
        {
            var specialtyToUniversity = await _context.SpecialtyToUniversities.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SpecialtyToUniversityDTO>(specialtyToUniversity);
        }

        public async Task<IEnumerable<SpecialtyToUniversityDTO>> GetAll()
        {
            var list = await _context.SpecialtyToUniversities
                .Join(_context.Universities,
                      su => su.UniversityId,
                      u => u.Id,
                      (su, u) => new SpecialtyToUniversity
                      {
                          Id = su.Id,
                          SpecialtyId = su.SpecialtyId,
                          UniversityId = su.UniversityId,
                          University = u
                      })
                .Join(_context.Specialties,
                      su => su.SpecialtyId,
                      s => s.Id,
                      (su, s) => new SpecialtyToUniversity
                      {
                          Id = su.Id,
                          SpecialtyId = su.SpecialtyId,
                          UniversityId = su.UniversityId,
                          University = su.University,
                          Specialty = s
                      })
                .Include(x => x.SpecialtyInUniversityDescription)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SpecialtyToUniversityDTO>>(list);
        }
    }
}
