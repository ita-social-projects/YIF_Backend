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
    public class DirectionToInstitutionOfEducationRepository : IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectionToInstitutionOfEducationRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(DirectionToInstitutionOfEducation item)
        {
            _context.DirectionsToInstitutionOfEducations.Update(item);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        // Not implemented, as the logic will be determined in the future
        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<DirectionToInstitutionOfEducationDTO>> Find(Expression<Func<DirectionToInstitutionOfEducation, bool>> predicate)
        {
            var list = _context.DirectionsToInstitutionOfEducations.Where(predicate)
                .Include(x => x.InstitutionOfEducation)
                .Include(x => x.Direction)
                .AsNoTracking().ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<DirectionToInstitutionOfEducationDTO>>(list));
            }

            return null;
        }

        public async Task<DirectionToInstitutionOfEducationDTO> Get(string id)
        {
            var directionToInstitutionOfEducation = await _context.DirectionsToInstitutionOfEducations.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<DirectionToInstitutionOfEducationDTO>(directionToInstitutionOfEducation);
        }

        public async Task<IEnumerable<DirectionToInstitutionOfEducationDTO>> GetAll()
        {
            var directionsToInstitutionOfEducation = await _context.Directions.ToListAsync();
            return _mapper.Map<IEnumerable<DirectionToInstitutionOfEducationDTO>>(directionsToInstitutionOfEducation);
        }
    }
}
