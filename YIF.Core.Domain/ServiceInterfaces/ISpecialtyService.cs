using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ISpecialtyService : IDisposable
    {
        Task<ResponseApiModel<SpecialtyApiModel>> GetSpecialtyByName(string name);
        Task<ResponseApiModel<SpecialtyApiModel>> GetSpecialtyById(string id);
        Task<ResponseApiModel<IEnumerable<SpecialtyApiModel>>> GetAllSpecialties();
        Task<ResponseApiModel<SpecialtyNamesApiModel>> GetAllSpecialtiesNames();
    }
}
