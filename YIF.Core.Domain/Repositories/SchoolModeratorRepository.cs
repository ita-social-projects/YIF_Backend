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
using YIF.Core.Domain.DtoModels.SchoolModerator;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolModeratorRepository : ISchoolModeratorRepository<SchoolModeratorDTO>
    {
        private readonly EFDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;
        public SchoolModeratorRepository(IMapper mapper,
                              UserManager<DbUser> userManager,
                              EFDbContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<string> AddSchoolModerator(SchoolModerator schoolModerator)
        {
            await _dbContext.SchoolModerators.AddAsync(schoolModerator);
            await _dbContext.SaveChangesAsync();
            return string.Empty;
        }

        public async void Dispose()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
