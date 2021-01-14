using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UniversityAdminRepository : IUniversityAdminRepository<UniversityAdminDTO>
    {
        private readonly EFDbContext _dbContext;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UniversityAdminRepository(IApplicationDbContext context, 
                                         IMapper mapper,
                                         EFDbContext dbContext)
        {
            _context = context;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<string> AddUniAdmin(UniversityAdmin universityAdmin)
        {
            await _dbContext.AddAsync(universityAdmin);
            await _dbContext.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<bool> Delete(string id)
        {
            DbUser user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<UniversityAdminDTO> GetByUniversityId(string universityId)
        {
            var universityAdmin = await _dbContext.UniversityAdmins.
                Where(p => p.UniversityId == universityId).
                FirstOrDefaultAsync();
            if (universityAdmin != null)
            {
                return  _mapper.Map<UniversityAdminDTO>(universityAdmin);
            }
            return null;
        }

        public Task<IEnumerable<UniversityAdminDTO>> GetAllUniAdmins()
        {
            throw new NotImplementedException();
        }

        public Task<UniversityAdminDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }
        [ExcludeFromCodeCoverage]
        public async void Dispose()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
