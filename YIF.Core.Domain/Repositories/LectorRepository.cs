﻿using AutoMapper;
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
    public class LectorRepository : ILectorRepository<Lector, LectorDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LectorRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Lector lector)
        {
            await _context.Lectors.AddAsync(lector);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(string id)
        {
            (await _context.Lectors.FirstOrDefaultAsync(x => x.Id == id)).IsDeleted = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<LectorDTO>> Find(Expression<Func<Lector, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<LectorDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LectorDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LectorDTO>> GetLectorsByIoEId(string ioEId)
        {
            var result = await _context.Lectors
                .Include(x => x.User)
                .Where(x => x.InstitutionOfEducationId == ioEId && x.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<LectorDTO>>(result);
        }

        public async Task<LectorDTO> GetLectorByUserAndIoEIds(string userId, string ioEId)
        {
            var lector = await _context.Lectors.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId && a.InstitutionOfEducationId == ioEId);
            return _mapper.Map<LectorDTO>(lector);
        }

        public async Task<LectorDTO> GetLectorInIoE(string lectorId, string ioEId)
        {
            var lector = await _context.Lectors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == lectorId && a.InstitutionOfEducationId == ioEId);
            return _mapper.Map<LectorDTO>(lector);
        }

        public Task<bool> Update(Lector item)
        {
            throw new NotImplementedException();
        }
    }
}
