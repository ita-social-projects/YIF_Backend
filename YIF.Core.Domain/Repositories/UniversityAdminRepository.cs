using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var universityAdmin = _dbContext.Users.Where(u => u.IsDeleted == false)
                .Join(_dbContext.UniversityModerators,
                      user => user.Id,
                      moderator => moderator.UserId,
                      (user, moderator) => new UniversityModerator
                      {
                          UserId = user.Id,
                          AdminId = moderator.AdminId
                      })
                .Join(_dbContext.UniversityAdmins.Where(a => a.Id == adminId),
                      moderator => moderator.AdminId,
                      admin => admin.Id,
                      (moderator, admin) => new UniversityAdmin
                      {
                          Id = moderator.UserId,
                          UniversityId = admin.UniversityId
                      });

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
            var universityAdmin = _dbContext.Users.Where(u => u.IsDeleted == false)
                   .Join(_dbContext.UniversityModerators,
                         user => user.Id,
                         moderator => moderator.UserId,
                         (user, moderator) => new UniversityModerator
                         {
                             UserId = user.Id,
                             AdminId = moderator.AdminId
                         })
                   .Join(_dbContext.UniversityAdmins.Where(a => a.UniversityId == universityId),
                         moderator => moderator.AdminId,
                         admin => admin.Id,
                         (moderator, admin) => new UniversityAdminDTO
                         {
                             Id = moderator.UserId,
                             UniversityId = admin.UniversityId
                         });

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
            var universityAdmin = _dbContext.Users
                .Join(_dbContext.UniversityModerators,
                      user => user.Id,
                      moderator => moderator.UserId,
                      (user, moderator) => new UniversityModerator
                      {
                          UserId = user.Id,
                          AdminId = moderator.AdminId
                      })
                .Join(_dbContext.UniversityAdmins,
                      moderator => moderator.AdminId,
                      admin => admin.Id,
                      (moderator, admin) => new UniversityAdmin
                      {
                          Id = moderator.UserId,
                          UniversityId = admin.UniversityId
                      })
                .Join(_dbContext.Universities,
                      admin => admin.UniversityId,
                      university => university.Id,
                      (admin, university) => new UniversityAdminDTO
                      {
                          Id = admin.Id,
                          UniversityId = admin.UniversityId,
                          UniversityName = university.Name
                      });

            if (universityAdmin.Count() != 0)
            {
                return await universityAdmin.ToListAsync();
            }
            return null;
        }

        public async Task<UniversityAdminDTO> GetById(string id)
        {
            var admin = await _dbContext.UniversityAdmins.FirstOrDefaultAsync(a => a.Id == id);
            return _mapper.Map<UniversityAdminDTO>(admin);
        }

        public  void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
