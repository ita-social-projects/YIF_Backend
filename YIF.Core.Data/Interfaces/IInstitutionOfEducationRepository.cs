using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> AddInstitutionOfEducation(TEntity institutionOfEducation);
        Task<TEntity> GetByName(string name);
        Task AddFavorite(TEntity institutionOfEducationToGraduate);
        Task RemoveFavorite(TEntity institutionOfEducationToGraduate);
        Task<IEnumerable<TEntity>> GetFavoritesByUserId(string userId);
        Task<bool> ContainsById(string id);
        Task<string> Disable(TEntity IoE);
        Task<string> Enable(TEntity IoE);
    }
}
