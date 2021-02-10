using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.SchoolAdmin;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolAdminRepository : ISchoolAdminRepository<SchoolAdminDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;

        public SchoolAdminRepository(IApplicationDbContext context,
                                         IMapper mapper,
                                         UserManager<DbUser> userManager)
        {
            _dbContext = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<string> AddSchoolAdmin(SchoolAdmin schoolAdmin)
        {
            await _dbContext.AddAsync(schoolAdmin);
            await _dbContext.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<SchoolAdminDTO> GetBySchoolId(string schoolId)
        {
            var schoolAdmin =
                                from users in _dbContext.Users
                                join moderators in _dbContext.SchoolModerators on users.Id equals moderators.UserId
                                join admins in _dbContext.SchoolAdmins on moderators.AdminId equals admins.Id
                                where (users.IsDeleted == false && admins.SchoolId == schoolId)
                                select new SchoolAdminDTO()
                                {
                                    Id = admins.Id,
                                    SchoolId = admins.Id,

                                };

            if (schoolAdmin.Count() != 0)
            {
                return await schoolAdmin.FirstOrDefaultAsync();
            }
            return null;
        }
        public async Task<string> Delete(string adminId)
        {
            var schoolAdmin =
                              from users in _dbContext.Users
                              join moderators in _dbContext.SchoolModerators on users.Id equals moderators.UserId
                              join admins in _dbContext.SchoolAdmins on moderators.AdminId equals admins.Id
                              where (users.IsDeleted == false && admins.Id == adminId)
                              select new SchoolAdmin()
                              {
                                  Id = users.Id,
                                  SchoolId = admins.SchoolId,

                              };
            if (schoolAdmin.Count() == 0)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(schoolAdmin.First().Id);
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            await _dbContext.SaveChangesAsync();
            return "User IsDeleted was updated";
        }
        public async Task<SchoolAdminDTO> GetBySchoolIdWithoutIsDeletedCheck(string schoolId)
        {
            var schoolAdmin = await _dbContext.SchoolAdmins.
                Where(p => p.SchoolId == schoolId).
                FirstOrDefaultAsync();
            if (schoolAdmin != null)
            {
                return _mapper.Map<SchoolAdminDTO>(schoolAdmin);
            }
            return null;
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<IEnumerable<SchoolAdminDTO>> GetAllSchoolAdmins()
        {
            var schoolAdmin =
                               from users in _dbContext.Users
                               join moderators in _dbContext.SchoolModerators on users.Id equals moderators.UserId
                               join admins in _dbContext.SchoolAdmins on moderators.AdminId equals admins.Id
                               join schools in _dbContext.Schools on admins.SchoolId equals schools.Id
                               where (users.IsDeleted == false)
                               select new SchoolAdminDTO()
                               {
                                   Id = admins.Id,
                                   SchoolId = admins.SchoolId,
                                   SchoolName = schools.Name

                               };
            if (schoolAdmin.Count() != 0)
            {
                return await schoolAdmin.ToListAsync();
            }
            return null;
        }
    }
}
