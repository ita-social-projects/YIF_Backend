using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class InstitutionOfEducationModeratorRepository : IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;
        public InstitutionOfEducationModeratorRepository(
            IMapper mapper,
            UserManager<DbUser> userManager,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<string> AddUniModerator(InstitutionOfEducationModerator institutionOfEducationModerator)
        {
             await _dbContext.InstitutionOfEducationModerators.AddAsync(institutionOfEducationModerator);
             await _dbContext.SaveChangesAsync();
             return string.Empty;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public  void Dispose()
        {
             _dbContext.Dispose();
        }

        public async Task<IEnumerable<InstitutionOfEducationModeratorDTO>> GetAllUniModerators()
        {
            var moderators = await _dbContext.InstitutionOfEducationModerators
                .Include(x => x.User)
                .Include(x => x.Admin)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InstitutionOfEducationModeratorDTO>>(moderators.AsEnumerable());
        }

        public async Task<InstitutionOfEducationModeratorDTO> GetById(string id)
        {
            var moderator = await _dbContext.InstitutionOfEducationModerators.Include(m => m.Admin).AsNoTracking().FirstAsync(a => a.UserId == id);
            return _mapper.Map<InstitutionOfEducationModeratorDTO>(moderator);
        }
    }
}
