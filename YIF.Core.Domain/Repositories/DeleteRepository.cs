using Microsoft.EntityFrameworkCore;
using System;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Interfaces;

namespace YIF.Core.Domain.Repositories
{
    public class DeleteRepository: IDeleteRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly ResourceManager _resourceManager;

        public DeleteRepository(IApplicationDbContext context, ResourceManager resourceManager)
        {
            _context = context;
            _resourceManager = resourceManager;
        }

        public async Task<string> Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var entityType = typeof(TEntity);
            var property = entityType.GetProperty("IsDeleted");

            if (property == null)
            {
                return _resourceManager.GetString("ObjectCannotBeDeleted");
            }

            property.SetValue(entity, Convert.ChangeType(true, property.PropertyType), null);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _resourceManager.GetString("ObjectWasDeleted");
        }

        public async Task<string> Restore<TEntity>(TEntity entity) where TEntity : class
        {
            var entityType = typeof(TEntity);
            var property = entityType.GetProperty("IsDeleted");

            if (property == null)
            {
                return _resourceManager.GetString("ObjectCannotBeRestored");
            }

            property.SetValue(entity, Convert.ChangeType(false, property.PropertyType), null);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _resourceManager.GetString("ObjectWasRestored");
        }
    }
}
