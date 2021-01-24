﻿using AutoMapper;
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
    public class SpecialityRepository : IRepository<Speciality, SpecialityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public Task<bool> Update(Speciality item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<SpecialityDTO> Get(string id)
        {
            var specialty = await _context.Specialities.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SpecialityDTO>(specialty);
        }

        public async Task<IEnumerable<SpecialityDTO>> GetAll()
        {
            var list = await _context.Specialities
                .Join(_context.Directions,
                s => s.DirectionId,
                d => d.Id,
                (s, d) => new Speciality
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    DirectionId = s.DirectionId,
                    Direction = d
                })
                .ToListAsync();
            return _mapper.Map<IEnumerable<SpecialityDTO>>(list);
        }

        [ExcludeFromCodeCoverage]
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
