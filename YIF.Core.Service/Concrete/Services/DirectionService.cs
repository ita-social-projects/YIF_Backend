using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class DirectionService : IDirectionService
    {
        private readonly IRepository<Direction, DirectionDTO> _repositoryDirection;
        private readonly IMapper _mapper;

        public DirectionService(IRepository<Direction, DirectionDTO> repositoryDirection, IMapper mapper)
        {
            _repositoryDirection = repositoryDirection;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DirectionResponseApiModel>> GetAllDirections()
        {
            var directions = await _repositoryDirection.GetAll();
            return _mapper.Map<IEnumerable<DirectionResponseApiModel>>(directions);
        }
    }
}
