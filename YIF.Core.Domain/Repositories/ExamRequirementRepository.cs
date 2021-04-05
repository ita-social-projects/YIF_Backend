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
    public class ExamRequirementRepository: IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExamRequirementRepository(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteRangeByDescriptionId(string id)
        {
            _context.ExamRequirements
                           .RemoveRange(await _context.ExamRequirements
                                            .Where(x => x.SpecialtyToIoEDescriptionId == id)
                                            .AsNoTracking()
                                            .ToListAsync());

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<ExamRequirementDTO>> Find(Expression<Func<ExamRequirement, bool>> predicate)
        {
            var list = await _context.ExamRequirements
               .Where(predicate)
               .ToListAsync();

            if (list != null || list.Count > 0)
            {
                return await Task.FromResult(_mapper.Map<IEnumerable<ExamRequirementDTO>>(list));
            }

            return null;
        }

        public Task<ExamRequirementDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExamRequirementDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(ExamRequirement item)
        {
            throw new NotImplementedException();
        }
    }
}
