using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolModeratorRepository : ISchoolModeratorRepository<SchoolModeratorDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        public SchoolModeratorRepository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> AddSchoolModerator(SchoolModerator schoolModerator)
        {
            await _dbContext.SchoolModerators.AddAsync(schoolModerator);
            await _dbContext.SaveChangesAsync();
            return string.Empty;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
