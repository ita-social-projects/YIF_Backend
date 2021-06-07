using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class DepartmentRepository : IDepartmentRepository<Department, DepartmentDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsDepartmentByNameAndDescriptionExist(string name, string description)
        {
            var department = await _context.Departments.AsNoTracking()
                .FirstOrDefaultAsync(d => d.Name.Equals(name) && d.Description.Equals(description));

            return department == null ? false : true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<bool> Update(Department item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DepartmentDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DepartmentDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DepartmentDTO>> Find(Expression<Func<Department, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
