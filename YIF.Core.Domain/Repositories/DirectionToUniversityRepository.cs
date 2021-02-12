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
    public class DirectionToUniversityRepository : IRepository<DirectionToUniversity, DirectionToUniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectionToUniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(DirectionToUniversity item)
        {
            _context.DirectionsToUniversities.Update(item);
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

        public Task<IEnumerable<DirectionToUniversityDTO>> Find(Expression<Func<DirectionToUniversity, bool>> predicate)
        {
            var list = _context.DirectionsToUniversities.Where(predicate)
                .Include(x => x.University)
                .Include(x => x.Direction)
                .AsNoTracking().ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<DirectionToUniversityDTO>>(list));
            }

            return null;
        }

        public async Task<DirectionToUniversityDTO> Get(string id)
        {
            var directionToUniversity = await _context.DirectionsToUniversities.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<DirectionToUniversityDTO>(directionToUniversity);
        }

        public async Task<IEnumerable<DirectionToUniversityDTO>> GetAll()
        {
            var directionsToUniversity = await _context.Directions.ToListAsync();
            return _mapper.Map<IEnumerable<DirectionToUniversityDTO>>(directionsToUniversity);
        }
    }
}
