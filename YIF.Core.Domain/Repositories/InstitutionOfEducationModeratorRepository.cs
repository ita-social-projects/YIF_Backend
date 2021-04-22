using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class InstitutionOfEducationModeratorRepository : IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public InstitutionOfEducationModeratorRepository(
            IMapper mapper,
            IApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<string> AddUniModerator(InstitutionOfEducationModerator institutionOfEducationModerator)
        {
             await _dbContext.InstitutionOfEducationModerators.AddAsync(institutionOfEducationModerator);
             await _dbContext.SaveChangesAsync();
             return string.Empty;
        }

        public async Task<bool> Delete(string id)
        {
            var moderator = _dbContext.InstitutionOfEducationModerators.FirstOrDefault(x => x.Id == id);
            moderator.IsDeleted = true;
            _dbContext.InstitutionOfEducationModerators.Update(moderator);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public  void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<IEnumerable<InstitutionOfEducationModeratorDTO>> GetAll()
        {
            var moderators = await _dbContext.InstitutionOfEducationModerators
                .Include(x => x.User)
                .Include(x => x.Admin)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InstitutionOfEducationModeratorDTO>>(moderators.AsEnumerable());
        }

        public async Task<IEnumerable<InstitutionOfEducationModeratorDTO>> GetByIoEId(string ioEId)
        {
            var result = await _dbContext.InstitutionOfEducationModerators
                .Include(x => x.User)
                .Where(x => x.Admin.InstitutionOfEducationId == ioEId)
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<InstitutionOfEducationModeratorDTO>>(result);
        }

        public async Task<InstitutionOfEducationModeratorDTO> GetByUserId(string id)
        {
            var moderator = await _dbContext.InstitutionOfEducationModerators.Include(x=>x.User).FirstOrDefaultAsync(x => x.UserId == id);
            return _mapper.Map<InstitutionOfEducationModeratorDTO>(moderator);
        }

        public Task<bool> Update(InstitutionOfEducationModerator item)
        {
            throw new NotImplementedException();
        }

        public Task<InstitutionOfEducationModeratorDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InstitutionOfEducationModeratorDTO>> Find(Expression<Func<InstitutionOfEducationModerator, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
