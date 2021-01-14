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
using YIF.Core.Domain.DtoModels.UniversityAdmin;
using Microsoft.AspNetCore.Identity;

namespace YIF.Core.Domain.Repositories
{
    public class UniversityAdminRepository : IUniversityAdminRepository<UniversityAdminDTO>
    {
        private readonly EFDbContext _dbContext;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;
        public UniversityAdminRepository(IApplicationDbContext context, 
                                         IMapper mapper,
                                         EFDbContext dbContext,
                                         UserManager<DbUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<string> AddUniAdmin(UniversityAdmin universityAdmin)
        {
            await _dbContext.AddAsync(universityAdmin);
            await _dbContext.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<string> Delete(string adminId)
        {
            var universityAdmin =
                              from  users in _dbContext.Users 
                              join moderators in _dbContext.UniversityModerators on users.Id equals moderators.UserId 
                              join admins in _dbContext.UniversityAdmins on moderators.AdminId equals admins.Id 
                              where (users.IsDeleted == false && admins.Id == adminId) || (users.IsDeleted == true && admins.Id == adminId)// costil
                                select new UniversityAdmin()
                              {
                                  Id = users.Id,
                                  UniversityId = admins.Id,

                              };
            if (universityAdmin.Count() ==0)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(universityAdmin.First().Id);
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();
            return "User IsDeleted was updated";
        }
        public async Task<UniversityAdminDTO> GetByUniversityId(string universityId)
        {
            var universityAdmin =
                                from users in _dbContext.Users
                                join moderators in _dbContext.UniversityModerators on users.Id equals moderators.UserId
                                join admins in _dbContext.UniversityAdmins on moderators.AdminId equals admins.Id
                                where (users.IsDeleted == false && admins.UniversityId == universityId) ||(users.IsDeleted == true && admins.UniversityId == universityId)// costil
                                select new UniversityAdminDTO()
                                {
                                    Id = admins.Id,
                                    UniversityId = admins.Id,

                                };

            if (universityAdmin != null)
            {
                return await universityAdmin.FirstOrDefaultAsync();
            }
            return null;
        }
        public async Task<UniversityAdminDTO> GetByUniversityIdWithoutIsDeletedCheck(string universityId)
        {
            var universityAdmin = await _dbContext.UniversityAdmins
                                  .Where(p => p.UniversityId == universityId)
                                  .FirstOrDefaultAsync();
            if (universityAdmin != null)
            {
                return _mapper.Map<UniversityAdminDTO>(universityAdmin);
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
