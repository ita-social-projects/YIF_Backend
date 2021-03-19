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
    public class PaymentFormToDescriptionRepository : IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PaymentFormToDescriptionRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Update(PaymentFormToDescription item)
        {
            _context.PaymentFormToDescriptions.Update(item);
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

        public Task<IEnumerable<PaymentFormToDescriptionDTO>> Find(Expression<Func<PaymentFormToDescription, bool>> predicate)
        {
            var list = _context.PaymentFormToDescriptions
                .Include(x => x.SpecialtyToIoEDescription)
                .Include(x => x.PaymentForm)
                .Where(predicate)
                .ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult(_mapper.Map<IEnumerable<PaymentFormToDescriptionDTO>>(list));
            }

            return null;
        }

        public async Task<PaymentFormToDescriptionDTO> Get(string id)
        {
            var paymentFormToDescription = await _context.PaymentFormToDescriptions.FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<PaymentFormToDescriptionDTO>(paymentFormToDescription);
        }

        // Not implemented, as the logic will be determined in the future
        public Task<IEnumerable<PaymentFormToDescriptionDTO>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}