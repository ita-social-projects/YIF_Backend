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
    public class UniversityModeratorRepository : IUniversityModeratorRepository<UniversityModeratorDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;
        public UniversityModeratorRepository(
            IMapper mapper,
            UserManager<DbUser> userManager,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<string> AddUniModerator(UniversityModerator universityModerator)
        {
             await _dbContext.UniversityModerators.AddAsync(universityModerator);
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

        public async Task<IEnumerable<UniversityModeratorDTO>> GetAllUniModerators()
        {
            var moderators = await _dbContext.UniversityModerators
                .Include(x => x.User)
                .Include(x => x.Admin)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UniversityModeratorDTO>>(moderators.AsEnumerable());
        }

        public Task<UniversityModeratorDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
