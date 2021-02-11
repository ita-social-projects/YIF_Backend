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
