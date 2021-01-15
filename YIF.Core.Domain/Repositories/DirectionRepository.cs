﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
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
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<DirectionDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DirectionDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            throw new NotImplementedException();
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