﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class InstitutionOfEducationRepository : IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resourceManager;

        public InstitutionOfEducationRepository(IApplicationDbContext context, IMapper mapper, ResourceManager resourceManager)
        {
            _context = context;
            _mapper = mapper;
            _resourceManager = resourceManager;
        }

        public async Task<bool> Update(InstitutionOfEducation item)
        {
            _context.InstitutionOfEducations.Update(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(string id)
        {
           (await _context.InstitutionOfEducations.FirstOrDefaultAsync(x => x.Id == id)).IsDeleted = true;
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task<InstitutionOfEducationDTO> Get(string id)
        {
            var institutionOfEducation = await _context.InstitutionOfEducations
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<InstitutionOfEducationDTO>(institutionOfEducation);
        }

        public async Task<IEnumerable<InstitutionOfEducationDTO>> GetAll()
        {
            var list = await _context.InstitutionOfEducations.ToListAsync();
            return _mapper.Map<IEnumerable<InstitutionOfEducationDTO>>(list);
        }

        public async Task<IEnumerable<InstitutionOfEducationDTO>> GetFavoritesByUserId(string userId)
        {
            var institutionOfEducations = from institutionOfEducationToGraduate in _context.InstitutionOfEducationsToGraduates
                    join institutionOfEducation in _context.InstitutionOfEducations on institutionOfEducationToGraduate.InstitutionOfEducationId equals institutionOfEducation.Id
                    join graduate in _context.Graduates on institutionOfEducationToGraduate.GraduateId equals graduate.Id
                    where (graduate.UserId == userId)
                    select institutionOfEducation;

            var list = await institutionOfEducations.ToListAsync();
            return _mapper.Map<IEnumerable<InstitutionOfEducationDTO>>(list);
        }

        [ExcludeFromCodeCoverage]
        public void Dispose() => _context.Dispose();

        public async Task<IEnumerable<InstitutionOfEducationDTO>> Find(Expression<Func<InstitutionOfEducation, bool>> predicate)
        {
            var institutionOfEducations = await _context.InstitutionOfEducations.Where(predicate).AsNoTracking().ToListAsync();

            if (institutionOfEducations != null || institutionOfEducations.Count > 0)
            {
                return _mapper.Map<IEnumerable<InstitutionOfEducationDTO>>(institutionOfEducations);
            }

            return null;
        }

        public async Task<InstitutionOfEducationDTO> AddInstitutionOfEducation(InstitutionOfEducation institutionOfEducation)
        {
            await _context.InstitutionOfEducations.AddAsync(institutionOfEducation);
            await _context.SaveChangesAsync();
            return _mapper.Map<InstitutionOfEducationDTO>(institutionOfEducation);
        }

        public async Task<InstitutionOfEducationDTO> GetByName(string name)
        {
            var uni = await _context.InstitutionOfEducations.
                                    Where(p => p.Name == name).
                                    FirstOrDefaultAsync();
            if (uni != null)
            {
                return _mapper.Map<InstitutionOfEducationDTO>(uni);
            }
            return null;
        }

        public async Task AddFavorite(InstitutionOfEducationToGraduate institutionOfEducationToGraduate)
        {
            await _context.InstitutionOfEducationsToGraduates.AddAsync(institutionOfEducationToGraduate);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavorite(InstitutionOfEducationToGraduate institutionOfEducationToGraduate)
        {
            _context.InstitutionOfEducationsToGraduates.Remove(institutionOfEducationToGraduate);
            await _context.SaveChangesAsync();
        }
        
        public async Task<bool> ContainsById(string id)
        {
            return await _context.InstitutionOfEducations.AnyAsync(x => x.Id == id);
        }

        public async Task<string> Disable(InstitutionOfEducation IoE)
        {
            IoE.IsBanned = true;
            _context.InstitutionOfEducations.Update(IoE);
            await _context.SaveChangesAsync();
            return _resourceManager.GetString("InstitutionOfEducationIsDisabled");
        }

        public async Task<string> Enable(InstitutionOfEducation IoE)
        {
            IoE.IsBanned = false;
            _context.InstitutionOfEducations.Update(IoE);
            await _context.SaveChangesAsync();
            return _resourceManager.GetString("InstitutionOfEducationIsEnabled");
        }
    }
}
