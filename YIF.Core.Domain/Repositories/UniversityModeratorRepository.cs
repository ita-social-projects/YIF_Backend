using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.UniversityModerator;

namespace YIF.Core.Domain.Repositories
{
    public class UniversityModeratorRepository : IUniversityModeratorRepository<UniversityModeratorDTO>
    {
        private readonly EFDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;
        public UniversityModeratorRepository(IMapper mapper,
                              UserManager<DbUser> userManager,
                              EFDbContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<string> AddUniModerator(UniversityModerator universityModerator)
        {
            UniversityModerator q = new UniversityModerator();
            q.UniversityId = universityModerator.UniversityId;
            q.UserId = universityModerator.UserId;
             await _dbContext.UniversityModerators.AddAsync(q);
             await _dbContext.SaveChangesAsync();
             return string.Empty;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UniversityModeratorDTO>> GetAllUniModerators()
        {
            throw new NotImplementedException();
        }

        public Task<UniversityModeratorDTO> GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
