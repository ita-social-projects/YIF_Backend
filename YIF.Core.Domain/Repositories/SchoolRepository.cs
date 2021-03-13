using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolRepository: ISchoolRepository<SchoolDTO>
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;
        public SchoolRepository(
            EFDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolDTO> GetByName(string name)
        {
            var institutionOfEducation = await _context.Schools.
                Where(p => p.Name == name).
                FirstOrDefaultAsync();
            if (institutionOfEducation != null)
            {
                return _mapper.Map<SchoolDTO>(institutionOfEducation);
            }
            return null;
        }
        public async void Dispose()
        {
            await _context.DisposeAsync();
        }

        public async Task<IEnumerable<SchoolDTO>> GetAll()
        {
            var schools = await _context.Schools.ToListAsync();
            return _mapper.Map<IEnumerable<SchoolDTO>>(schools);
        }

        public async Task<IEnumerable<string>> GetAllAsStrings()
        {
            return await _context.Schools
                .Select(d => d.Name)
                .Where(n => n != null && n != string.Empty)
                .OrderBy(n => n)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> Exist(string schoolName)
        {
            var school = await _context.Schools.FirstOrDefaultAsync(x => x.Name == schoolName);
            return school != null;
        }
    }
}
