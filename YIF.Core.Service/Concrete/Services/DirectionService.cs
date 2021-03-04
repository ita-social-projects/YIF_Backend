using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
        private readonly IRepository<Direction, DirectionDTO> _directionRepository;
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IRepository<DirectionToUniversity, DirectionToUniversityDTO> _directionToUniversityRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly ResourceManager _resourceManager;

        public DirectionService(
            IRepository<Direction, DirectionDTO> directionRepository,
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IRepository<DirectionToUniversity, DirectionToUniversityDTO> directionToUniversityRepository,
            IMapper mapper,
            IPaginationService paginationService,
            ResourceManager resourceManager)
        {
            _directionRepository = directionRepository;
            _mapper = mapper;
            _paginationService = paginationService;
            _specialtyRepository = specialtyRepository;
            _directionToUniversityRepository = directionToUniversityRepository;
            _resourceManager = resourceManager;
        }

        public async Task<IEnumerable<DirectionResponseApiModel>> GetAllDirectionsByFilter(PageApiModel pageModel, FilterApiModel filterModel)
        {
            var directions = await _directionRepository.GetAll();

            if (filterModel.DirectionName != string.Empty && filterModel.DirectionName != null)
            {
                directions = directions.Where(d => d.Name == filterModel.DirectionName);
            }

            if (filterModel.SpecialtyName != string.Empty && filterModel.SpecialtyName != null)
            {
                var specialties = await _specialtyRepository.Find(s => s.Name == filterModel.SpecialtyName);
                var filteredDirections = specialties.Select(s => s.DirectionId);
                directions = directions.Where(d => filteredDirections.Contains(d.Id));
            }

            if (filterModel.UniversityName != string.Empty && filterModel.UniversityName != null)
            {
                var directionToUniversity = await _directionToUniversityRepository.Find(du => du.University.Name == filterModel.UniversityName);
                var filteredDirections = directionToUniversity.Select(du => du.DirectionId);
                directions = directions.Where(d => filteredDirections.Contains(d.Id));
            }

            if (filterModel.UniversityAbbreviation != string.Empty && filterModel.UniversityAbbreviation != null)
            {
                var directionToUniversity = await _directionToUniversityRepository.Find(du => du.University.Abbreviation == filterModel.UniversityAbbreviation);
                var filteredDirections = directionToUniversity.Select(du => du.DirectionId);
                directions = directions.Where(d => filteredDirections.Contains(d.Id));
            }
            
            return _mapper.Map<IEnumerable<DirectionResponseApiModel>>(directions.Distinct().ToList());
        }

        public async Task<PageResponseApiModel<DirectionResponseApiModel>> GetAllDirections(PageApiModel pageModel)
        {
            var directions = _mapper.Map<IEnumerable<DirectionResponseApiModel>>(await _directionRepository.GetAll());
            try
            {
                return _paginationService.GetPageFromCollection(directions, pageModel);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetDirectionsNamesByFilter(FilterApiModel filterModel)
        {
            var pageModel = new PageApiModel
            {
                Page = 1,
                PageSize = 10
            };
            var directions = await GetAllDirectionsByFilter(pageModel, filterModel);
            
            return directions
                .Select(s => s.Name)
                .Where(n => n != null)
                .OrderBy(n => n);
        }
    }
}
