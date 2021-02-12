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
    public class DirectionRepository : IRepository<Direction, DirectionDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(Direction item)
        {
            _context.Directions.Update(item);
            return await _context.SaveChangesAsync() > 0;
        }

        // Not implemented, as the logic will be determined in the future
        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<DirectionDTO> Get(string id)
        {
            var direction = await _context.Directions.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<DirectionDTO>(direction);
        }

        public async Task<IEnumerable<DirectionDTO>> GetAll()
        {
            var directions = await _context.Directions.ToListAsync();
            return _mapper.Map<IEnumerable<DirectionDTO>>(directions);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<DirectionDTO>> Find(Expression<Func<Direction, bool>> predicate)
        {
            var directions = await _context.Directions.Where(predicate).AsNoTracking().ToListAsync();

            if (directions != null || directions.Count > 0)
            {
                return _mapper.Map<IEnumerable<DirectionDTO>>(directions);
            }

            return null;
        }
    }
}
