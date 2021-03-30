using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class ExamRepository : IExamRepository<Exam, ExamDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ExamRepository(
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

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<ExamDTO>> Find(Expression<Func<Exam, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ExamDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ExamDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<ExamDTO>>(await _context.Exams.AsNoTracking().ToListAsync());
        }

        public Task<bool> Update(Exam item)
        {
            throw new NotImplementedException();
        }
    }
}
