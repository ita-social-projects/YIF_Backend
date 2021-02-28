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
    public class EducationFormToDescriptionRepository : IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EducationFormToDescriptionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(EducationFormToDescription item)
        {
            _context.EducationFormToDescriptions.Update(item);
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

        public Task<IEnumerable<EducationFormToDescriptionDTO>> Find(Expression<Func<EducationFormToDescription, bool>> predicate)
        {
            var list = _context.EducationFormToDescriptions
                .Include(x => x.SpecialtyInUniversityDescription)
                .Include(x => x.EducationForm)
                .Where(predicate)
                .ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<EducationFormToDescriptionDTO>>(list));
            }

            return null;
        }

        public async Task<EducationFormToDescriptionDTO> Get(string id)
        {
            var educationFormToDescription = await _context.EducationFormToDescriptions.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<EducationFormToDescriptionDTO>(educationFormToDescription);
        }

        // Not implemented, as the logic will be determined in the future
        public Task<IEnumerable<EducationFormToDescriptionDTO>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}