using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.School;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolRepository: ISchoolRepository<SchoolDTO>
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;
        public SchoolRepository(EFDbContext context,
                                    IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SchoolDTO> GetByName(string name)
        {
            var university = await _context.Schools.
                Where(p => p.Name == name).
                FirstOrDefaultAsync();
            if (university != null)
            {
                return _mapper.Map<SchoolDTO>(university);
            }
            return null;
        }
        [ExcludeFromCodeCoverage]
        public async void Dispose()
        {
            await _context.DisposeAsync();
        }

        public async Task<IEnumerable<SchoolDTO>> GetAll()
        {
            var schools = await _context.Schools.ToListAsync();
            return _mapper.Map<IEnumerable<SchoolDTO>>(schools);
        }
    }
}
