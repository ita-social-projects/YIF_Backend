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
    public class UniversityRepository : IRepository<University, UniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(University item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UniversityDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UniversityDTO>> GetAll()
        {
            var list = await _context.Universities.ToListAsync();

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
    }
}
