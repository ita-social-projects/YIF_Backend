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
    public class InstitutionOfEducationAdminRepository : IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;

        public InstitutionOfEducationAdminRepository(
            IApplicationDbContext context,
            IMapper mapper,
            UserManager<DbUser> userManager)
        {
            _dbContext = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<string> AddUniAdmin(InstitutionOfEducationAdmin institutionOfEducationAdmin)
        {
            await _dbContext.AddAsync(institutionOfEducationAdmin);
            await _dbContext.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<string> Delete(DbUser user)
        {
            //isDelete is User's field, we're using UserRepository.Delete
            return "";
        }

        public async Task<string> Disable(InstitutionOfEducationAdmin admin)
        {
            admin.IsBanned = true;
            _dbContext.InstitutionOfEducationAdmins.Update(admin);
            await _dbContext.SaveChangesAsync();
            return "Admin IsBanned was updated";
        }
        public async Task<InstitutionOfEducationAdminDTO> GetByInstitutionOfEducationId(string institutionOfEducationId)
        {
            var institutionOfEducationAdmin = await _dbContext.InstitutionOfEducationAdmins
                .Where(admin => admin.InstitutionOfEducationId == institutionOfEducationId && admin.User.IsDeleted == false)
                .Include(x => x.InstitutionOfEducation)
                .Include(y => y.User).FirstOrDefaultAsync();

            if (institutionOfEducationAdmin != null)
            {
                return _mapper.Map<InstitutionOfEducationAdminDTO>(institutionOfEducationAdmin);
            }
            return null;
        }
        public async Task<InstitutionOfEducationAdminDTO> GetByInstitutionOfEducationIdWithoutIsDeletedCheck(string institutionOfEducationId)
        {
            var institutionOfEducationAdmin = await _dbContext.InstitutionOfEducationAdmins
                                  .Where(p => p.InstitutionOfEducationId == institutionOfEducationId)
                                  .FirstOrDefaultAsync();
            if (institutionOfEducationAdmin != null)
            {
                return _mapper.Map<InstitutionOfEducationAdminDTO>(institutionOfEducationAdmin);
            }
            return null;
        }

        public async Task<IEnumerable<InstitutionOfEducationAdminDTO>> GetAllUniAdmins()
        {
            var institutionOfEducationAdmin = _dbContext.InstitutionOfEducationAdmins
                .Where(admin => admin.User.IsDeleted == false)
                .Include(x => x.InstitutionOfEducation)
                .Include(y => y.User)
                .ToList();

            return _mapper.Map<IEnumerable<InstitutionOfEducationAdminDTO>>(institutionOfEducationAdmin);
        }

        public async Task<InstitutionOfEducationAdminDTO> GetUserByAdminId(string id)
        {
            var institutionOfEducationAdmin = await _dbContext.InstitutionOfEducationAdmins.AsNoTracking()
                .Where(admin => admin.Id == id && admin.User.IsDeleted == false)
                .Include(y => y.User)
                .FirstOrDefaultAsync();
            return _mapper.Map<InstitutionOfEducationAdminDTO>(institutionOfEducationAdmin);
        }

        public async Task<InstitutionOfEducationAdminDTO> GetById(string id)
        {
            var admin = await _dbContext.InstitutionOfEducationAdmins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            return _mapper.Map<InstitutionOfEducationAdminDTO>(admin);
        }

        public  void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
