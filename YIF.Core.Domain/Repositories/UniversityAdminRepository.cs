using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;
        public UniversityAdminRepository(
            IApplicationDbContext context,
            IMapper mapper,
            UserManager<DbUser> userManager)
        {
            _dbContext = context;
            _mapper = mapper;
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
                              from users in _dbContext.Users
                              join moderators in _dbContext.UniversityModerators on users.Id equals moderators.UserId
                              join admins in _dbContext.UniversityAdmins on moderators.AdminId equals admins.Id
                              where (users.IsDeleted == false && admins.Id == adminId)
                              select new UniversityAdmin()
                              {
                                  Id = users.Id,
                                  UniversityId = admins.UniversityId,

                              };
            if (universityAdmin.Count() == 0)
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
                                where (users.IsDeleted == false && admins.UniversityId == universityId)
                                select new UniversityAdminDTO()
                                {
                                    Id = admins.Id,
                                    UniversityId = admins.UniversityId,

                                };
            if (universityAdmin.Count() != 0)
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

        public async Task<IEnumerable<UniversityAdminDTO>> GetAllUniAdmins()
        {
            var universityAdmin =
                               from users in _dbContext.Users
                               join moderators in _dbContext.UniversityModerators on users.Id equals moderators.UserId
                               join admins in _dbContext.UniversityAdmins on moderators.AdminId equals admins.Id
                               join unis in _dbContext.Universities on admins.UniversityId equals unis.Id
                               where (users.IsDeleted == false)
                               select new UniversityAdminDTO()
                               {
                                   Id = admins.Id,
                                   UniversityId = admins.UniversityId,
                                   UniversityName = unis.Name

                               };
            if (universityAdmin.Count() != 0)
            {
                return await universityAdmin.ToListAsync();
            }
            return null;
        }

        public Task<UniversityAdminDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }
        [ExcludeFromCodeCoverage]
        public  void Dispose()
        {
            _dbContext.Dispose();
        }


    }
}
