using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class DirectionToUniversityRepository : IRepository<DirectionToUniversity, DirectionToUniversityDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DirectionToUniversityRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<string> Create(DirectionToUniversity dbUser, object entityUser, string userPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<DirectionToUniversityDTO>> Find(Expression<Func<DirectionToUniversity, bool>> predicate)
        {
            var list = _context.DirectionsToUniversities.Where(predicate)
                .Include(x => x.University)
                .Include(x => x.Direction)
                .AsNoTracking().ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<DirectionToUniversityDTO>>(list));
            }

            return null;
        }

        public Task<DirectionToUniversityDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DirectionToUniversityDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(DirectionToUniversity item)
        {
            throw new NotImplementedException();
        }
    }
}
