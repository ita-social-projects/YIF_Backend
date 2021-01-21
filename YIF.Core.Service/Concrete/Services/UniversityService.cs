using AutoMapper;
using SendGrid.Helpers.Errors.Model;
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
    public class UniversityService : IUniversityService<University>
    {
        private readonly IUniversityRepository<University, UniversityDTO> _universityRepository;
        private readonly IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> _specialityRepository;
        private readonly IRepository<DirectionToUniversity, DirectionToUniversityDTO> _directionRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;

        public UniversityService(
            IUniversityRepository<University, UniversityDTO> universityRepository,
            IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> specialityRepository,
            IRepository<DirectionToUniversity, DirectionToUniversityDTO> directionRepository,
            IMapper mapper,
            IPaginationService paginationService)
        {
            _universityRepository = universityRepository;
            _specialityRepository = specialityRepository;
            _directionRepository = directionRepository;
            _mapper = mapper;
            _paginationService = paginationService;
        }

        public async Task<IEnumerable<UniversityResponseApiModel>> GetUniversityByFilter(FilterApiModel filterModel)
        {
            // Filtered list of universities 
            IEnumerable<UniversityDTO> filteredUniversities = await _universityRepository.GetAll();

            // Check if we must filter universities by direction name
            if (filterModel.DirectionName != string.Empty && filterModel.DirectionName != null)
            {
                // Get all directions by name
                var directions = await _directionRepository.Find(x => x.Direction.Name == filterModel.DirectionName);
                // Get universities by these directions
                filteredUniversities = directions.Select(x => x.University).ToList();
            }

            if (filterModel.SpecialityName != string.Empty && filterModel.SpecialityName != null)
            {
                // Get all specialities by name
                var specialities = await _specialityRepository.Find(x => x.Speciality.Name == filterModel.SpecialityName);

                if (filteredUniversities != null && filteredUniversities.Count() > 1)
                {
                    // Inner join between directions and specialities 
                    filteredUniversities = filteredUniversities.Where(x => specialities.Any(y => y.UniversityId == x.Id));
                }
                else
                {
                    filteredUniversities = specialities.Select(x => x.University).ToList();
                }
            }

            if (filterModel.UniversityName != string.Empty && filterModel.UniversityName != null)
            {
                if (filteredUniversities != null && filteredUniversities.Count() > 1)
                {
                    filteredUniversities = filteredUniversities.Where(x => x.Name == filterModel.UniversityName);
                }
                else
                {
                    var universities = await _universityRepository.Find(x => x.Name == filterModel.UniversityName);
                    filteredUniversities = universities.AsEnumerable();
                }
            }

            return _mapper.Map<IEnumerable<UniversityResponseApiModel>>(filteredUniversities.Distinct().ToList());
        }

        public async Task<UniversityResponseApiModel> GetUniversityById(string universityId, string userId = null)
        {
            var university = await _universityRepository.Get(universityId);

            if (university == null)
                throw new NotFoundException("Університету з таким id не існує.");

            var favoriteUniversities = await _universityRepository.GetFavoritesByUserId(userId);
            university.IsFavorite = favoriteUniversities.Where(fu => fu.Id == university.Id).Count() > 0;

            return _mapper.Map<UniversityResponseApiModel>(university);
        }

        public async Task<PageResponseApiModel<UniversityResponseApiModel>> GetUniversitiesPage(
            FilterApiModel filterModel,
            PageApiModel pageModel,
            string userId = null)
        {
            var universities = _mapper.Map<IEnumerable<UniversityResponseApiModel>>(await GetUniversityByFilter(filterModel));
            var result = new PageResponseApiModel<UniversityResponseApiModel>();

            if (universities == null || universities.Count() == 0)
                throw new NotFoundException("Університети не було знайдено.");

            try
            {
                result = _paginationService.GetPageFromCollection(universities, pageModel);
            }
            catch
            {
                throw;
            }

            // Set the value to favorites
            var favoriteUniversities = await _universityRepository.GetFavoritesByUserId(userId);
            foreach (var university in result.ResponseList)
            {
                university.IsFavorite = favoriteUniversities.Where(fu => fu.Id == university.Id).Count() > 0;
            }

            return result;
        }

        public async Task<IEnumerable<UniversityResponseApiModel>> GetFavoriteUniversities(string userId)
        {
            var favoriteUnivesistes = await _universityRepository.GetFavoritesByUserId(userId);
            if (favoriteUnivesistes == null || favoriteUnivesistes.Count() == 0)
            {
                throw new NotFoundException("Немає вибраних університетів.");
            }

            foreach (var university in favoriteUnivesistes)
            {
                university.IsFavorite = true;
            }

            return _mapper.Map<IEnumerable<UniversityResponseApiModel>>(favoriteUnivesistes);
        }

        public async Task<IEnumerable<string>> GetUniversityAbbreviations()
        {
            var abbreviations = await _universityRepository.GetAbbreviations();

            if (abbreviations == null || abbreviations.Count() == 0)
                throw new NotFoundException("Університети не було знайдено");

            return abbreviations;
        }
    }
}
