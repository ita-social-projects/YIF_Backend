using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class SpecialtyToGraduateRepository : ISpecialtyToGraduateRepository<SpecialtyToGraduate, SpecialtyToGraduateDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialtyToGraduateRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<bool> Update(SpecialtyToGraduate item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SpecialtyToGraduateDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialtyToGraduateDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialtyToGraduateDTO>> Find(Expression<Func<SpecialtyToGraduate, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task AddFavorite(SpecialtyToGraduate specialtyToGraduate)
        {
            await _context.SpecialtyToGraduates.AddAsync(specialtyToGraduate);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavorite(SpecialtyToGraduate specialtyToGraduate)
        {
            _context.SpecialtyToGraduates.Remove(specialtyToGraduate);
            await _context.SaveChangesAsync();
        }
    }
}
