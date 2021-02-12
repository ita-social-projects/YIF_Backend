using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolGraduateRepository : ISchoolGraduateRepository<SchoolDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SchoolGraduateRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [ExcludeFromCodeCoverage]
        public void Dispose() => _context.Dispose();


        public async Task<SchoolDTO> GetSchoolByUserId(string userId)
        {
            var graduate = await _context.Graduates.Include(s => s.School).FirstOrDefaultAsync(x => x.UserId == userId);
            var school = graduate?.School;
            return _mapper.Map<SchoolDTO>(school);
        }
    }
}
