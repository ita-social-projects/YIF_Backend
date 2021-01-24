using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
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

        public Task<string> Create(SpecialityToUniversity dbUser, object entityUser, string userPassword, string role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<SpecialityToUniversityDTO>> Find(Expression<Func<SpecialityToUniversity, bool>> predicate)
        {
            var list = _context.SpecialityToUniversities.Where(predicate)
                .Include(x => x.University)
                .ToList();

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

        public async Task<IEnumerable<SpecialityToUniversityDTO>> GetAll()
        {
            var list = await _context.SpecialityToUniversities
                .Join(_context.Universities,
                      su => su.UniversityId,
                      u => u.Id,
                      (su, u) => new SpecialityToUniversity
                      {
                          Id = su.Id,
                          SpecialityId = su.SpecialityId,
                          UniversityId = su.UniversityId,
                          University = u
                      })
                .Join(_context.Specialities,
                      su => su.SpecialityId,
                      s => s.Id,
                      (su, s) => new SpecialityToUniversity
                      {
                          Id = su.Id,
                          SpecialityId = su.SpecialityId,
                          UniversityId = su.UniversityId,
                          University = su.University,
                          Speciality = s
                      })
                .ToListAsync();

            return _mapper.Map<IEnumerable<SpecialityToUniversityDTO>>(list);
        }

        public Task<SpecialityToUniversityDTO> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<DbUser> GetUserWithToken(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<DbUser> GetUserWithUserProfile(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDefaultUserProfileIfEmpty(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(SpecialityToUniversity item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserPhoto(DbUser user, string photo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserToken(DbUser user, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
