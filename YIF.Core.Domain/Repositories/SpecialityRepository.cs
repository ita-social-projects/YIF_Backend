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
    public class SpecialityRepository : IRepository<Speciality, SpecialityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<string> Create(Speciality dbUser, object entityUser, string userPassword)
        {
            throw new NotImplementedException();
        }

        public Task<string> Create(Speciality dbUser, object entityUser, string userPassword, string role)
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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialityDTO>> Find(Expression<Func<Speciality, bool>> predicate)
        {
            var specialities = _context.Specialities.Where(predicate)
                .AsNoTracking().ToList();

            if (specialities != null || specialities.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<SpecialityDTO>>(specialities));
            }

            return null;
        }

        public Task<SpecialityDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialityDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SpecialityDTO> GetByEmail(string email)
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

        public Task<bool> Update(Speciality item)
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
