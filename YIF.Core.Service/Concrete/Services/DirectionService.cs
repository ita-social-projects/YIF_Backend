using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class DirectionService : IDirectionService
    {
        private readonly IRepository<Direction, DirectionDTO> _repositoryDirection;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public DirectionService(IRepository<Direction, DirectionDTO> repositoryDirection, IMapper mapper,
            IPaginationService paginationService)
        {
            _repositoryDirection = repositoryDirection;
            _mapper = mapper;
            _paginationService = paginationService;
        }
        public async Task<PageResponseApiModel<DirectionResponseApiModel>> GetAllDirections(int page = 1, int pageSize = 10, string url = null)
        {
            var directions = _mapper.Map<IEnumerable<DirectionResponseApiModel>>(await _repositoryDirection.GetAll());
            if (directions == null || directions.Count() == 0)
                throw new NotFoundException("Напрями не знайдено.");

            var pageModel = new PageApiModel
            {
                Page = page,
                PageSize = pageSize,
                Url = url
            };

            try
            {
                return _paginationService.GetPageFromCollection(directions, pageModel);
            }
            catch
            {
                throw new Exception("Проблема з пагінацією.");
            }
        }
    }
}
