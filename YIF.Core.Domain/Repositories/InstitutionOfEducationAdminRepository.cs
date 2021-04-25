using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class InstitutionOfEducationAdminRepository : IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>
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
            return "Admin IsBanned was set to true";
        }

        public async Task<string> Enable(InstitutionOfEducationAdmin admin)
        {
            admin.IsBanned = false;
            _dbContext.InstitutionOfEducationAdmins.Update(admin);
            await _dbContext.SaveChangesAsync();
            return "Admin IsBanned was set to false";
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
            var institutionOfEducationAdmin = await _dbContext.InstitutionOfEducationAdmins
                .Where(admin => admin.User.IsDeleted == false)
                .Include(x => x.InstitutionOfEducation)
                .Include(y => y.User)
                .ToListAsync();

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

        public async Task<InstitutionOfEducationAdminDTO> GetByUserId(string userId)
        {
            var admin = await _dbContext.InstitutionOfEducationAdmins.AsNoTracking().FirstOrDefaultAsync(a => a.UserId == userId);
            return _mapper.Map<InstitutionOfEducationAdminDTO>(admin);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public Task<bool> Update(InstitutionOfEducationAdmin item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            var admin = _dbContext.InstitutionOfEducationAdmins.FirstOrDefault(x => x.Id == id);
            admin.IsDeleted = true;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public Task<InstitutionOfEducationAdminDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InstitutionOfEducationAdminDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstitutionOfEducationAdminDTO>> Find(Expression<Func<InstitutionOfEducationAdmin, bool>> predicate)
        {
            var ioEAdmins = await _dbContext.InstitutionOfEducationAdmins
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<InstitutionOfEducationAdminDTO>>(ioEAdmins);
        }
    }
}
