using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Data.Entities;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace YIF.Core.Domain.Repositories
{
    public class DisciplineRepository: IDisciplineRepository<Discipline, DisciplineDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DisciplineRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<bool> Update(Discipline item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DisciplineDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisciplineDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task Add(Discipline discipline)
        {
            await _context.Disciplines.AddAsync(discipline);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DisciplineDTO>> Find(Expression<Func<Discipline, bool>> predicate)
        {
            var discipline = await _context.Disciplines.Where(predicate).AsNoTracking().ToListAsync();

            if (discipline != null && discipline.Count > 0)
            {
                return _mapper.Map<IEnumerable<DisciplineDTO>>(discipline);
            }

            return null;
        }
    }
}
