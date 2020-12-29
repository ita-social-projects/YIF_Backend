using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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
