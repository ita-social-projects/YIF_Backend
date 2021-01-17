using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IDirectionService
    {
        Task<IEnumerable<DirectionResponseApiModel>> GetAllDirections();
    }
}
