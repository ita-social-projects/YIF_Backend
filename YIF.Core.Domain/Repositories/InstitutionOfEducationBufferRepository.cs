using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class InstitutionOfEducationBufferRepository : IInstitutionOfEducationBufferRepository<InstitutionOfEducationBuffer, InstitutionOfEducationBufferDTO>
    {
        private readonly IApplicationDbContext _context;

        public InstitutionOfEducationBufferRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(InstitutionOfEducationBuffer IoEBuffer)
        {
            await _context.InstitutionOfEducationBuffers.AddAsync(IoEBuffer);
            await _context.SaveChangesAsync();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<InstitutionOfEducationBufferDTO>> Find(Expression<Func<InstitutionOfEducationBuffer, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<InstitutionOfEducationBufferDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InstitutionOfEducationBufferDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(InstitutionOfEducationBuffer item)
        {
            throw new NotImplementedException();
        }
    }
}
