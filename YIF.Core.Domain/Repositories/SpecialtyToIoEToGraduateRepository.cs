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
    public class SpecialtyToIoEToGraduateRepository : ISpecialtyToIoEToGraduateRepository<SpecialtyToInstitutionOfEducationToGraduate, SpecialtyToInstitutionOfEducationToGraduateDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialtyToIoEToGraduateRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<bool> Update(SpecialtyToInstitutionOfEducationToGraduate item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SpecialtyToInstitutionOfEducationToGraduateDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialtyToInstitutionOfEducationToGraduateDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialtyToInstitutionOfEducationToGraduateDTO>> Find(Expression<Func<SpecialtyToInstitutionOfEducationToGraduate, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task AddFavorite(SpecialtyToInstitutionOfEducationToGraduate specialtyToInstitutionOfEducationToGraduate)
        {
            await _context.SpecialtyToInstitutionOfEducationToGraduates.AddAsync(specialtyToInstitutionOfEducationToGraduate);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveFavorite(SpecialtyToInstitutionOfEducationToGraduate specialtyToInstitutionOfEducationToGraduate)
        {
            _context.SpecialtyToInstitutionOfEducationToGraduates.Remove(specialtyToInstitutionOfEducationToGraduate);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> FavoriteContains(SpecialtyToInstitutionOfEducationToGraduate specialtyToInstitutionOfEducationToGraduate)
        {
            var result = await _context.SpecialtyToInstitutionOfEducationToGraduates
                .AsNoTracking()
                .Where(x => x.SpecialtyId == specialtyToInstitutionOfEducationToGraduate.SpecialtyId)
                .Where(x => x.InstitutionOfEducationId == specialtyToInstitutionOfEducationToGraduate.InstitutionOfEducationId)
                .Where(x => x.GraduateId == specialtyToInstitutionOfEducationToGraduate.GraduateId)
                .FirstOrDefaultAsync();

            if (result != null)
                return true;
            return false;
        }
    }
}
