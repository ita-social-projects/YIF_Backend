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
    public class DirectionRepository : IDirectionRepository<Direction, DirectionDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> Update(Direction item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DirectionDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DirectionDTO>> GetAll()
        {
            var directions = await _context.Directions.ToListAsync();
            return _mapper.Map<IEnumerable<DirectionDTO>>(directions);
        }

        [ExcludeFromCodeCoverage]
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

        public async Task<IEnumerable<string>> GetNames()
        {
            return await _context.Directions
                .Select(d => d.Name)
                .Where(n => n != null && n != string.Empty)
                .OrderBy(n => n)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
