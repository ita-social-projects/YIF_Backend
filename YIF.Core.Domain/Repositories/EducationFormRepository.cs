using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class EducationFormRepository : IEducationFormRepository<EducationForm, EducationFormDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EducationFormRepository(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<EducationFormDTO>> Find(Expression<Func<EducationForm, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<EducationFormDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EducationFormDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<EducationFormDTO>>(await _context.EducationForms.AsNoTracking().ToListAsync());
        }

        public Task<bool> Update(EducationForm item)
        {
            throw new NotImplementedException();
        }
    }
}
