using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels;

namespace YIF.Core.Domain.Repositories
{
    public class SpecialtyRepository : ISpecialtyRepository<SpecialtyDTO>
    {
        private readonly EFDbContext _context;
        private readonly IMapper _mapper;
        public SpecialtyRepository(EFDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void Dispose() => _context.Dispose();

        public async Task<IEnumerable<SpecialtyDTO>> GetAllSpecialties()
        {
            var list = await _context.Specialities.ToListAsync();
            return _mapper.Map<IEnumerable<SpecialtyDTO>>(list);
        }

        public async Task<SpecialtyDTO> GetById(string id)
        {
            var specialty = await _context.Specialities.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<SpecialtyDTO>(specialty);
        }
    }
}
