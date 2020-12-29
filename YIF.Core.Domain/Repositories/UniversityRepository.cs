using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels;

namespace YIF.Core.Domain.Repositories
{
    public class UniversityRepository : IRepository<University, UniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<string> Create(University dbUser, object entityUser, string userPassword)
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

        public Task<IEnumerable<UniversityDTO>> Find(Expression<Func<University, bool>> predicate)
        {
            var universities = _context.Universities.Where(predicate).AsNoTracking().ToList();

            if (universities != null || universities.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<UniversityDTO>>(universities));
            }

            return null;
        }

        public Task<UniversityDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UniversityDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(University item)
        {
            throw new NotImplementedException();
        }
    }
}
