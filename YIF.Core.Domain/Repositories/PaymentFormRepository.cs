using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class PaymentFormRepository : IPaymentFormRepository<PaymentForm, PaymentFormDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PaymentFormRepository(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<PaymentFormDTO>> Find(Expression<Func<PaymentForm, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentFormDTO> Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PaymentFormDTO>> GetAll()
        {
            return _mapper.Map<IEnumerable<PaymentFormDTO>>(await _context.PaymentForms.AsNoTracking().ToListAsync());
        }

        public Task<bool> Update(PaymentForm item)
        {
            throw new NotImplementedException();
        }
    }
}
