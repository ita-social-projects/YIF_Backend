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
    public class SpecialtyToInstitutionOfEducationRepository : ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialtyToInstitutionOfEducationRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(SpecialtyToInstitutionOfEducation item)
        {
            _context.SpecialtyToInstitutionOfEducations.Update(item);
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

        public async Task<IEnumerable<SpecialtyToInstitutionOfEducationDTO>> Find(Expression<Func<SpecialtyToInstitutionOfEducation, bool>> predicate)
        {
            var list = await _context.SpecialtyToInstitutionOfEducations
                .Include(x => x.Specialty)
                .Include(x => x.InstitutionOfEducation)
                .Include(x => x.SpecialtyToIoEDescriptions)
                .Where(predicate)
                .ToListAsync();

            if (list != null || list.Count > 0)
            {
                return await Task.FromResult(_mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationDTO>>(list));
            }

            return null;
        }

        public async Task<SpecialtyToInstitutionOfEducationDTO> Get(string id)
        {
            throw new NotImplementedException();
            //var specialtyToInstitutionOfEducation = await _context.SpecialtyToInstitutionOfEducations.FirstOrDefaultAsync(x => x.Id == id);
            //return _mapper.Map<SpecialtyToInstitutionOfEducationDTO>(specialtyToInstitutionOfEducation);
        }

        public async Task<IEnumerable<SpecialtyToInstitutionOfEducationDTO>> GetAll()
        {
            var list = await _context.SpecialtyToInstitutionOfEducations
                .Include(x => x.Specialty)
                .Include(x => x.InstitutionOfEducation)
                .Include(x => x.SpecialtyToIoEDescriptions)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationDTO>>(list);
        }

        public async Task AddSpecialty(SpecialtyToInstitutionOfEducation specialtyToInstitutionOfEducation)
        {
            await _context.SpecialtyToInstitutionOfEducations.AddAsync(specialtyToInstitutionOfEducation);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SpecialtyToInstitutionOfEducationDTO>> GetSpecialtyToIoEDescriptionsById(string id)
        {
            var specialtyToInstitutionOfEducation = await _context.SpecialtyToInstitutionOfEducations
              .Where(su => su.SpecialtyId == id)
              .Include(u => u.InstitutionOfEducation)
              .Include(s => s.Specialty)
              .Include(sd => sd.SpecialtyToIoEDescriptions)
              .ThenInclude(e => e.ExamRequirements)
              .ThenInclude(e => e.Exam)
              .AsNoTracking()
              .ToListAsync();

            //good, but big count of operations
            foreach (var item in specialtyToInstitutionOfEducation)
            {
                foreach (var item1 in item.SpecialtyToIoEDescriptions)
                {
                    if (item1.Description == null)
                    {
                        item1.Description = item.Specialty.Description;
                    }
                }
              
            }

            return _mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationDTO>>(specialtyToInstitutionOfEducation);
        }

        //Move to specialtyToIoEToGraduate
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

        //Move to specialtyToIoEToGraduate
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

        public async Task<bool> Contains(string Id)
        {
            var result = await _context.SpecialtyToInstitutionOfEducations
                .AsNoTracking()
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (result != null)
                return true;
            return false;
        }
    }
}
