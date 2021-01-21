using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UniversityRepository : IUniversityRepository<University, UniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> Update(University item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UniversityDTO> Get(string id)
        {
            var university = await _context.Universities.FindAsync(id);
            if (university != null)
            {
                return _mapper.Map<UniversityDTO>(university);
            }
            throw new KeyNotFoundException("User not found:  " + id);
        }

        public async Task<IEnumerable<UniversityDTO>> GetAll()
        {
            var list = await _context.Universities.ToListAsync();
            return _mapper.Map<IEnumerable<UniversityDTO>>(list);
        }

        public async Task<IEnumerable<UniversityDTO>> GetFavoritesByUserId(string userId)
        {
            var universities = from universityToGraduate in _context.UniversitiesToGraduates
                    join university in _context.Universities on universityToGraduate.UniversityId equals university.Id
                    join graduate in _context.Graduates on universityToGraduate.GraduateId equals graduate.Id
                    where (graduate.UserId == userId)
                    select university;

            var list = await universities.ToListAsync();
            return _mapper.Map<IEnumerable<UniversityDTO>>(list);
        }

        [ExcludeFromCodeCoverage]
        public void Dispose() => _context.Dispose();

        public async Task<IEnumerable<UniversityDTO>> Find(Expression<Func<University, bool>> predicate)
        {
            var universities = await _context.Universities.Where(predicate).AsNoTracking().ToListAsync();

            if (universities != null || universities.Count > 0)
            {
                return _mapper.Map<IEnumerable<UniversityDTO>>(universities);
            }

            return null;
        }

        public async Task<string> AddUniversity(University university)
        {
            var result = await _context.Universities.AddAsync(university);
            await _context.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<UniversityDTO> GetByName(string name)
        {
            var uni = await _context.Universities.
                                    Where(p => p.Name == name).
                                    FirstOrDefaultAsync();
            if (uni != null)
            {
                return _mapper.Map<UniversityDTO>(uni);
            }
            return null;
        }

        public async Task<IEnumerable<string>> GetAbbreviations()
        {
            return await _context.Universities
                .Select(u => u.Abbreviation)
                .Where(a => a != null && a != string.Empty)
                .OrderBy(a => a)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
