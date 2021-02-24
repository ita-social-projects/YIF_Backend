using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;

namespace YIF.Core.Domain.Repositories
{
    public class EducationFormToDescriptionRepository : IRepository<EducationFormToDescription, EducationFormToDescription>
    {
        private readonly IApplicationDbContext _context;

        public EducationFormToDescriptionRepository(IApplicationDbContext context)
        {
            _context = context;
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

        public Task<IEnumerable<EducationFormToDescription>> Find(Expression<Func<EducationFormToDescription, bool>> predicate)
        {
            var list = _context.EducationFormToDescriptions
                .Include(x => x.SpecialtyInUniversityDescription)
                .Include(x => x.EducationForm)
                .Where(predicate)
                .ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult<IEnumerable<EducationFormToDescription>>(list);
            }

            return null;
        }

        public async Task<EducationFormToDescription> Get(string id)
        {
            var specialtyToUniversity = await _context.EducationFormToDescriptions.FirstOrDefaultAsync(x => x.Id == id);
            return specialtyToUniversity;
        }

        // Not implemented, as the logic will be determined in the future
        public Task<IEnumerable<EducationFormToDescription>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}