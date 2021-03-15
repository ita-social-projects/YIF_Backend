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
using YIF.Core.Domain.DtoModels.IdentityDTO;

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

        public async Task<string> Delete(DbUser user)
        {
            //isDelete is User's field, we're using UserRepository.Delete
            return "";
        }

        public async Task<string> Disable(UniversityAdmin admin)
        {
            admin.IsBanned = true;
            _dbContext.UniversityAdmins.Update(admin);
            await _dbContext.SaveChangesAsync();
            return "Admin IsBanned was set to true";
        }

        public async Task<string> Enable(UniversityAdmin admin)
        {
            admin.IsBanned = false;
            _dbContext.UniversityAdmins.Update(admin);
            await _dbContext.SaveChangesAsync();
            return "Admin IsBanned was set to false";
        }
        public async Task<UniversityAdminDTO> GetByUniversityId(string universityId)
        {
            var universityAdmin = await _dbContext.UniversityAdmins
                .Where(admin => admin.UniversityId == universityId && admin.User.IsDeleted == false)
                .Include(x => x.University)
                .Include(y => y.User).FirstOrDefaultAsync();

            if (universityAdmin != null)
            {
                return _mapper.Map<UniversityAdminDTO>(universityAdmin);
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
            var universityAdmin = await _dbContext.UniversityAdmins
                .Where(admin => admin.User.IsDeleted == false)
                .Include(x => x.University)
                .Include(y => y.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UniversityAdminDTO>>(universityAdmin);
        }

        public async Task<UniversityAdminDTO> GetUserByAdminId(string id)
        {
            var universityAdmin = await _dbContext.UniversityAdmins.AsNoTracking()
                .Where(admin => admin.Id == id && admin.User.IsDeleted == false)
                .Include(y => y.User)
                .FirstOrDefaultAsync();
            return _mapper.Map<UniversityAdminDTO>(universityAdmin);
        }

        public async Task<UniversityAdminDTO> GetById(string id)
        {
            var admin = await _dbContext.UniversityAdmins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            return _mapper.Map<UniversityAdminDTO>(admin);
        }

        public  void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
