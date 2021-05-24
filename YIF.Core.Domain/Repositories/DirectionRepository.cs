using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class DirectionRepository : Repository<Direction>
    {
        private readonly IApplicationDbContext _context;

        public DirectionRepository(EFDbContext context):base(context)
        {

        }

        public async Task<DirectionDTO> Get(string id)
        {
            var direction = await _context.Directions.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<DirectionDTO>(direction);
        }

        public async Task<IEnumerable<DirectionDTO>> GetAll()
        {
            var directions = await _context.Directions.Select(x => new Direction
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                
                Specialties = x.Specialties.Select(y => new Specialty
                {
                    Id = y.Id,
                    Name = y.Name,
                    Code = y.Code,
                    Description = y.Description
                }).ToList()
            })
                .ToListAsync();
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
        public async Task<IEnumerable<DirectionDTO>> GetByIoEId(string InstitutionOfEducationId)
        {
            var directions = await (from d in _context.Directions.AsNoTracking()
                                    join di in _context.DirectionsToInstitutionOfEducations.AsNoTracking() on d.Id equals di.DirectionId
                                    where di.InstitutionOfEducationId == InstitutionOfEducationId
                                    select new Direction
                                    {
                                        Id = d.Id,
                                        Name = d.Name,
                                        Code = d.Code,
                                        Specialties = _context.Specialties.AsNoTracking().Where(x => x.DirectionId == d.Id).ToList()
                                    }).ToListAsync();

            return _mapper.Map<IEnumerable<DirectionDTO>>(directions);
        }
    }
}
