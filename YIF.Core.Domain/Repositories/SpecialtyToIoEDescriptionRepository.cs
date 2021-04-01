﻿using AutoMapper;
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
    public class SpecialtyToIoEDescriptionRepository : ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SpecialtyToIoEDescriptionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> Add(SpecialtyToIoEDescription specialtyToIoEDescription)
        {
            await _context.SpecialtyToIoEDescriptions.AddAsync(specialtyToIoEDescription);
            await _context.SaveChangesAsync();
            return specialtyToIoEDescription.Id;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<IEnumerable<SpecialtyToIoEDescriptionDTO>> Find(Expression<Func<SpecialtyToIoEDescription, bool>> predicate)
        {
            var list = await _context.SpecialtyToIoEDescriptions
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();

            if (list != null || list.Count > 0)
            {
                return await Task.FromResult(_mapper.Map<IEnumerable<SpecialtyToIoEDescriptionDTO>>(list));
            }

            return null;
        }

        public Task<SpecialtyToIoEDescriptionDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialtyToIoEDescriptionDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<SpecialtyToIoEDescriptionDTO> GetEducationForms()
        {
            return _mapper.Map<SpecialtyToIoEDescriptionDTO>(new SpecialtyToIoEDescription());
        }

        public async Task<SpecialtyToIoEDescriptionDTO> GetPaymentForms()
        {
            return _mapper.Map<SpecialtyToIoEDescriptionDTO>(new SpecialtyToIoEDescription());
        }

        public Task<bool> Update(SpecialtyToIoEDescription item)
        {
            throw new NotImplementedException();
        }
    }
}
