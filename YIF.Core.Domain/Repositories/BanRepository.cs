using System;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YIF.Core.Data.Interfaces;

namespace YIF.Core.Domain.Repositories
{
    public class BanRepository : IBanRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly ResourceManager _resourceManager;

        public BanRepository(IApplicationDbContext context, ResourceManager resourceManager)
        {
            _context = context;
            _resourceManager = resourceManager;
        }

        public async Task<string> Ban<TEntity>(TEntity entity) where TEntity : class
        {
            var entityType = typeof(TEntity);
            var property = entityType.GetProperty("IsBanned");

            if (property == null)
            {
                return _resourceManager.GetString("ObjectCannotBeDisabled");
            }

            property.SetValue(entity, Convert.ChangeType(true, property.PropertyType), null);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _resourceManager.GetString("ObjectWasDisabled");
        }

        public async Task<string> Unban<TEntity>(TEntity entity) where TEntity : class
        {
            var entityType = typeof(TEntity);
            var property = entityType.GetProperty("IsBanned");

            if (property == null)
            {
                return _resourceManager.GetString("ObjectCannotBeEnabled");
            }

            property.SetValue(entity, Convert.ChangeType(false, property.PropertyType), null);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _resourceManager.GetString("ObjectWasEnabled");
        }
    }
}
