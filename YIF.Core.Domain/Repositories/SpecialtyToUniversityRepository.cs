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
    public class SpecialtyToUniversityRepository : ISpecialtyToUniversityRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialtyToUniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(SpecialtyToUniversity item)
        {
            _context.SpecialtyToUniversities.Update(item);
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

        public Task<IEnumerable<SpecialtyToUniversityDTO>> Find(Expression<Func<SpecialtyToUniversity, bool>> predicate)
        {
            var list = _context.SpecialtyToUniversities
                .Include(x => x.Specialty)
                .Include(x => x.University)
                .Include(x => x.SpecialtyInUniversityDescription)
                .Where(predicate)
                .ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<SpecialtyToUniversityDTO>>(list));
            }

            return null;
        }

        public async Task<SpecialtyToUniversityDTO> Get(string id)
        {
            var specialtyToUniversity = await _context.SpecialtyToUniversities.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SpecialtyToUniversityDTO>(specialtyToUniversity);
        }

        public async Task<IEnumerable<SpecialtyToUniversityDTO>> GetAll()
        {
            var list = await _context.SpecialtyToUniversities
                .Include(x => x.Specialty)
                .Include(x => x.University)
                .Include(x => x.SpecialtyInUniversityDescription)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SpecialtyToUniversityDTO>>(list);
        }
        public async Task<IEnumerable<SpecialtyToUniversityDTO>> GetSpecialtyInUniversityDescriptionsById(string id)
        {
            var specialtyToUniversity = await _context.SpecialtyToUniversities
              .Where(su => su.SpecialtyId == id)
              .Where(sdi => sdi.SpecialtyInUniversityDescriptionId != null)
              .Include(u => u.University)
              .Include(s => s.Specialty)
              .Include(sd => sd.SpecialtyInUniversityDescription)
              .ThenInclude(e => e.ExamRequirements)
                  .ThenInclude(e => e.Exam)
              .Include(sd => sd.SpecialtyInUniversityDescription)
                  .ThenInclude(e => e.PaymentFormToDescriptions)
                      .ThenInclude(e => e.PaymentForm)
              .Include(sd => sd.SpecialtyInUniversityDescription)
                  .ThenInclude(e => e.EducationFormToDescriptions)
                      .ThenInclude(e => e.EducationForm)
              .ToListAsync();

            foreach (var item in specialtyToUniversity)
            {
                if(item.SpecialtyInUniversityDescription.Description == null)
                {
                    item.SpecialtyInUniversityDescription.Description = item.Specialty.Description;
                }
            }

            return _mapper.Map<IEnumerable<SpecialtyToUniversityDTO>>(specialtyToUniversity);
        }
        public async Task AddFavorite(SpecialtyToUniversityToGraduate specialtyToUniversityToGraduate)
        {
            await _context.SpecialtyToUniversityToGraduates.AddAsync(specialtyToUniversityToGraduate);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveFavorite(SpecialtyToUniversityToGraduate specialtyToUniversityToGraduate)
        {
            _context.SpecialtyToUniversityToGraduates.Remove(specialtyToUniversityToGraduate);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> FavoriteContains(SpecialtyToUniversityToGraduate specialtyToUniversityToGraduate)
        {
            var result = await _context.SpecialtyToUniversityToGraduates
                .AsNoTracking()
                .Where(x => x.SpecialtyId == specialtyToUniversityToGraduate.SpecialtyId)
                .Where(x => x.UniversityId == specialtyToUniversityToGraduate.UniversityId)
                .Where(x => x.GraduateId == specialtyToUniversityToGraduate.GraduateId)
                .FirstOrDefaultAsync();

            if (result != null)
                return true;
            return false;
        }
    }
}