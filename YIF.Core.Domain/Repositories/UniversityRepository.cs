using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.University;

namespace YIF.Core.Domain.Repositories
{
    public class UniversityRepository : IUniversityRepository<UniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UniversityRepository(IApplicationDbContext context, 
                                    IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public  Task<string> AddUniversity(University university)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<UniversityDTO>> GetAllUniversities()
        {
            var list = await _context.Universities.ToListAsync();
            return _mapper.Map<IEnumerable<UniversityDTO>>(list);
        }

        public async Task<UniversityDTO> GetByName(string name)
        {
            var university = await _context.Universities.
                Where(p=>p.Name == name).
                FirstOrDefaultAsync();
            if (university != null)
            {
                return _mapper.Map<UniversityDTO>(university);
            }
            throw new KeyNotFoundException("There is no university with name = " + name);
        }


        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
