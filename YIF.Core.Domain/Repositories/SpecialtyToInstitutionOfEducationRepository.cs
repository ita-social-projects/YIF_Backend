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

        public Task<IEnumerable<SpecialtyToInstitutionOfEducationDTO>> Find(Expression<Func<SpecialtyToInstitutionOfEducation, bool>> predicate)
        {
            var list = _context.SpecialtyToInstitutionOfEducations
                .Include(x => x.Specialty)
                .Include(x => x.InstitutionOfEducation)
                //.Include(x => x.SpecialtyToIoEDescription)
                .Where(predicate)
                .ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationDTO>>(list));
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
                //.Include(x => x.SpecialtyToIoEDescription)
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
              //.Include(sd => sd.SpecialtyToIoEDescription)
              //.ThenInclude(e => e.ExamRequirements)
                  //.ThenInclude(e => e.Exam)
              .AsNoTracking()
              .ToListAsync();
            //var entity = from si in _context.SpecialtyToInstitutionOfEducations
                         //join desc in _context.SpecialtyToIoEDescriptions on si.SpecialtyToIoEDescriptionId equals desc.Id
                         //join er in _context.ExamRequirements on desc.Id equals er.SpecialtyToIoEDescriptionId
                         //join e in _context.Exams on er.ExamId equals e.Id
                         //select si;


            //foreach (var item in specialtyToInstitutionOfEducation)
            //{
            //    if(item.SpecialtyToIoEDescription.Description == null)
            //    {
            //        item.SpecialtyToIoEDescription.Description = item.Specialty.Description;
            //    }
            //}

            return _mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationDTO>>(specialtyToInstitutionOfEducation);
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

        public async Task<SpecialtyToInstitutionOfEducationDTO> GetFullSpecialtyDescriptionById(string specialtyId, string IoEId)
        {
            var specialtyToInstitutionOfEducation = await _context.SpecialtyToInstitutionOfEducations
              .Where(i => i.InstitutionOfEducationId == IoEId)
              .Where(su => su.SpecialtyId == specialtyId)
              .Include(u => u.InstitutionOfEducation)
              .Include(s => s.Specialty)
              //.Include(sd => sd.SpecialtyToIoEDescription)
              //.ThenInclude(e => e.ExamRequirements)
                  //.ThenInclude(e => e.Exam)
              .AsNoTracking()
              .FirstOrDefaultAsync();

            return _mapper.Map<SpecialtyToInstitutionOfEducationDTO>(specialtyToInstitutionOfEducation);
        }
    }
}
