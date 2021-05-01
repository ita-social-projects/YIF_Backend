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
                .ThenInclude(sd => sd.ExamRequirements)
                .ThenInclude(er => er.Exam)
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
            var specialtyToInstitutionOfEducation = await _context.SpecialtyToInstitutionOfEducations.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SpecialtyToInstitutionOfEducationDTO>(specialtyToInstitutionOfEducation);
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

        public async Task<string> AddSpecialty(SpecialtyToInstitutionOfEducation specialtyToInstitutionOfEducation)
        {
            await _context.SpecialtyToInstitutionOfEducations.AddAsync(specialtyToInstitutionOfEducation);
            await _context.SaveChangesAsync();
            return specialtyToInstitutionOfEducation.Id;
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

        public async Task<SpecialtyToInstitutionOfEducationDTO> GetBySpecialtyId(string id, string ioeId)
        {
            var specialty = await _context.SpecialtyToInstitutionOfEducations.Where(x => x.SpecialtyId == id && x.InstitutionOfEducationId == ioeId).FirstOrDefaultAsync();
                return _mapper.Map<SpecialtyToInstitutionOfEducationDTO>(specialty);
        }
    }
}
