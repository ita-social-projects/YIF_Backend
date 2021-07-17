using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class IoEBufferRepository : IIoEBufferRepository<IoEBuffer, IoEBufferDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public IoEBufferRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Add(IoEBuffer institutionOfEducationBuffer)
        {
            await _context.IoEBuffers.AddAsync(institutionOfEducationBuffer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Delete(string id)
        {
            var ioEBuffer = await _context.IoEBuffers.FirstOrDefaultAsync(x => x.Id == id);
            _context.IoEBuffers.Remove(ioEBuffer);
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<IoEBufferDTO>> Find(Expression<Func<IoEBuffer, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IoEBufferDTO> Get(string id)
        {
            var ioEBuffer = await _context.IoEBuffers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<IoEBufferDTO>(ioEBuffer);
        }

        public async Task<IEnumerable<IoEBufferDTO>> GetAll()
        {
            var ioEs = await _context.IoEBuffers.ToListAsync();

            if (ioEs != null && ioEs.Count > 0)
            {
                return _mapper.Map<IEnumerable<IoEBufferDTO>>(ioEs);
            }

            return null;
        }

        public async Task<bool> Update(IoEBuffer item)
        {
            _context.IoEBuffers.Update(item);
            return await _context.SaveChangesAsync() > 1;
        }
    }
}
