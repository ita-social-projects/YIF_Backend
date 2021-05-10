using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
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
        private readonly ResourceManager _resourceManager;
        public InstitutionOfEducationModeratorRepository(
            IMapper mapper,
            IApplicationDbContext dbContext,
            ResourceManager resourceManager)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _resourceManager = resourceManager;
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
            var moderator = await _dbContext.InstitutionOfEducationModerators
                .Include(x=>x.User)
                .Include(m => m.Admin)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == id);
            
            return _mapper.Map<InstitutionOfEducationModeratorDTO>(moderator);
        }

        public async Task<bool> Update(InstitutionOfEducationModerator item)
        {
            _dbContext.InstitutionOfEducationModerators.Update(item);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<InstitutionOfEducationModeratorDTO> GetModeratorForAdmin(string id, string adminId)
        {
            var moderator = await _dbContext.InstitutionOfEducationModerators
                .Include(x => x.User)
                .Include(m => m.Admin)
                .Where(x => x.AdminId == adminId)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<InstitutionOfEducationModeratorDTO>(moderator);
        }

        public Task<IEnumerable<InstitutionOfEducationModeratorDTO>> Find(Expression<Func<InstitutionOfEducationModerator, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Disable(InstitutionOfEducationModerator ioEModerator)
        {
            ioEModerator.IsBanned = true;
            _dbContext.InstitutionOfEducationModerators.Update(ioEModerator);
            await _dbContext.SaveChangesAsync();
            return _resourceManager.GetString("IoEModeratorIsDisabled");
        }

        public async Task<string> Enable(InstitutionOfEducationModerator ioEModerator)
        {
            ioEModerator.IsBanned = false;
            _dbContext.InstitutionOfEducationModerators.Update(ioEModerator);
            await _dbContext.SaveChangesAsync();
            return _resourceManager.GetString("IoEModeratorIsEnabled");
        }

        public async Task<InstitutionOfEducationModeratorDTO> Get(string id)
        {
            var moderator = await _dbContext.InstitutionOfEducationModerators
                .Include(x => x.User)
                .Include(m => m.Admin)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<InstitutionOfEducationModeratorDTO>(moderator);
        }
    }
}
