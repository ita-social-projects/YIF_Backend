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
    public class LectorRepository : ILectorRepository<Lecture, LectureDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LectorRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Lecture lector)
        {
            await _context.Lectures.AddAsync(lector);
            await _context.SaveChangesAsync();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<LectureDTO>> Find(Expression<Func<Lecture, bool>> predicate)
        {
            var lectors = await _context.Lectures.Where(predicate)
                .AsNoTracking().ToListAsync();

            if (lectors != null || lectors.Count > 0)
            {
                return _mapper.Map<IEnumerable<LectureDTO>>(lectors);
            }

            return null;
        }

        public Task<LectureDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LectureDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<LectureDTO> GetByUserId(string userId, string ioEId)
        {
            var lector = await _context.Lectures.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId && a.InstitutionOfEducationId == ioEId);
            return _mapper.Map<LectureDTO>(lector);
        }

        public Task<bool> Update(Lecture item)
        {
            throw new NotImplementedException();
        }
    }
}
