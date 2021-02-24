using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;

namespace YIF.Core.Domain.Repositories
{
    public class PaymentFormToDescriptionRepository : IRepository<PaymentFormToDescription, PaymentFormToDescription>
    {
        private readonly IApplicationDbContext _context;

        public PaymentFormToDescriptionRepository(IApplicationDbContext context)
        {
            _context = context;
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

        public Task<IEnumerable<PaymentFormToDescription>> Find(Expression<Func<PaymentFormToDescription, bool>> predicate)
        {
            var list = _context.PaymentFormToDescriptions
                .Include(x => x.SpecialtyInUniversityDescription)
                .Include(x => x.PaymentForm)
                .Where(predicate)
                .ToList();

            if (list != null || list.Count > 0)
            {
                return Task.FromResult<IEnumerable<PaymentFormToDescription>>(list);
            }

            return null;
        }

        public async Task<PaymentFormToDescription> Get(string id)
        {
            var paymentFormToDescription = await _context.PaymentFormToDescriptions.FirstOrDefaultAsync(x => x.Id == id);
            return paymentFormToDescription;
        }

        // Not implemented, as the logic will be determined in the future
        public Task<IEnumerable<PaymentFormToDescription>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}