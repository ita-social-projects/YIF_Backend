﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolAdminRepository : ISchoolAdminRepository<SchoolAdminDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;

        public SchoolAdminRepository(
            IApplicationDbContext context,
            IMapper mapper,
            UserManager<DbUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<string> AddSchoolAdmin(SchoolAdmin schoolAdmin)
        {
            await _context.AddAsync(schoolAdmin);
            await _context.SaveChangesAsync();
            return string.Empty;
        }

        public async Task<SchoolAdminDTO> GetBySchoolId(string schoolId)
        {
            var admin = await _context.Schools.Include(x => x.Admins).Where(x => x.Id == schoolId)
                        .Select(x=>x.Admins).FirstOrDefaultAsync();
            
            if (admin != null && admin.Count>0)
            {
                return  _mapper.Map<SchoolAdminDTO>(admin);
            }

            return null;
        }

        public async Task<string> Delete(string adminId)
        {
            var schoolAdmin = _context.Users.Where(u => u.IsDeleted == false)
                .Join(_context.SchoolModerators,
                      user => user.Id,
                      moderator => moderator.UserId,
                      (user, moderator) => new SchoolModerator
                      {
                          UserId = user.Id,
                          AdminId = moderator.AdminId
                      })
                .Join(_context.SchoolAdmins.Where(a => a.Id == adminId),
                      moderator => moderator.AdminId,
                      admin => admin.Id,
                      (moderator, admin) => new SchoolAdmin
                      {
                          Id = moderator.UserId,
                          SchoolId = admin.SchoolId,
                      });

            if (schoolAdmin.Count() == 0)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(schoolAdmin.First().Id);
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();
            return "User IsDeleted was updated";
        }
        public async Task<SchoolAdminDTO> GetBySchoolIdWithoutIsDeletedCheck(string schoolId)
        {
            var schoolAdmin = await _context.SchoolAdmins
                .Where(p => p.SchoolId == schoolId)
                .FirstOrDefaultAsync();
            if (schoolAdmin != null)
            {
                return _mapper.Map<SchoolAdminDTO>(schoolAdmin);
            }
            return null;
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<SchoolAdminDTO>> GetAllSchoolAdmins()
        {
            var schoolAdmin = _context.Users
                .Join(_context.SchoolModerators,
                      user => user.Id,
                      moderator => moderator.UserId,
                      (user, moderator) => new SchoolModerator
                      {
                          UserId = user.Id,
                          AdminId = moderator.AdminId
                      })
                .Join(_context.SchoolAdmins,
                      moderator => moderator.AdminId,
                      admin => admin.Id,
                      (moderator, admin) => new SchoolAdmin
                      {
                          Id = moderator.UserId,
                          SchoolId = admin.SchoolId,
                      })
                .Join(_context.Schools,
                      admin => admin.SchoolId,
                      school => school.Id,
                      (admin, school) => new SchoolAdminDTO
                      {
                          Id = admin.Id,
                          SchoolId = admin.SchoolId,
                          SchoolName = school.Name
                      });

            if (schoolAdmin.Count() != 0)
            {
                return await schoolAdmin.ToListAsync();
            }
            return null;
        }
    }
}
