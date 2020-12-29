using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class SpecialityToUniversityRepository : IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialityToUniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<string> Create(SpecialityToUniversity dbUser, object entityUser, string userPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialityToUniversityDTO>> Find(Expression<Func<SpecialityToUniversity, bool>> predicate)
        {
            var list = _context.SpecialityToUniversities.Where(predicate).AsNoTracking().ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<SpecialityToUniversityDTO>>(list));
            }

            return null;
        }

        public Task<SpecialityToUniversityDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialityToUniversityDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(SpecialityToUniversity item)
        {
            throw new NotImplementedException();
        }
    }
}
