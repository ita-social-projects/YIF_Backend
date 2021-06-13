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
    public class SpecialtyRepository : ISpecialtyRepository<Specialty, SpecialtyDTO>
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
             _context.Specialties.Update(specialty);
             return await _context.SaveChangesAsync() > 0;
        }

        public async Task Delete(string id)
        {
            var specialty = _context.Specialties.FirstOrDefault(x => x.Id == id);
            specialty.IsDeleted = true;
            await _context.SaveChangesAsync();
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

        public async Task<IEnumerable<SpecialtyDTO>> GetFavoritesByUserId(string userId)
        {
            var specialties = from specialtyToGraduate in _context.SpecialtyToGraduates
                join specialty in _context.Specialties on specialtyToGraduate.SpecialtyId equals specialty.Id
                join graduate in _context.Graduates on specialtyToGraduate.GraduateId equals graduate.Id
                where (graduate.UserId == userId)
                select specialty;

            var list = await specialties.ToListAsync();
            return _mapper.Map<IEnumerable<SpecialtyDTO>>(list);
        }

        public async Task<bool> ContainsById(string id)
        {
            return await _context.Specialties.AnyAsync(x => x.Id == id);
        }

        public async Task Add(Specialty specialty)
        {
            await _context.Specialties.AddAsync(specialty);
            await _context.SaveChangesAsync();
        }
    }
}
