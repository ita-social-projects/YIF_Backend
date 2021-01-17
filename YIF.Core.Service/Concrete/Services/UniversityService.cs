using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        public UniversityService(
            IUniversityRepository<University, UniversityDTO> universityRepository,
            IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> specialityRepository,
            IRepository<DirectionToUniversity, DirectionToUniversityDTO> directionRepository,
            IMapper mapper)
        {
            _universityRepository = universityRepository;
            _specialityRepository = specialityRepository;
            _directionRepository = directionRepository;
            _mapper = mapper;
        }

        public async Task<ResponseApiModel<IEnumerable<UniversityFilterResponseApiModel>>> GetUniversityByFilter(FilterApiModel filterModel)
        {
            var result = new ResponseApiModel<IEnumerable<UniversityFilterResponseApiModel>>();
            // Filtered list of universities 
            IEnumerable<UniversityDTO> filteredUniversities = null;

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

            result.Object = _mapper.Map<IEnumerable<UniversityFilterResponseApiModel>>(filteredUniversities.Distinct().ToList());
            return result.Set(true);
        }
        
        public async Task<UniversityResponseApiModel> GetUniversityById(string universityId, string userId = null)
        {
            
            var university = await _universityRepository.Get(universityId);

            if (university == null)
                throw new KeyNotFoundException();

            var favoriteUniversities = await _universityRepository.GetFavoritesByUserId(userId);
            university.IsFavorite = favoriteUniversities.Where(fu => fu.Id == university.Id).Count() > 0;

            return _mapper.Map<UniversityResponseApiModel>(university);
        }

        public async Task<PageResponseApiModel<UniversityResponseApiModel>> GetUniversitiesPage(int page = 1, int pageSize = 10, string url = null, string userId = null)
        {
            var universities =  _mapper.Map<IEnumerable<UniversityResponseApiModel>>(await _universityRepository.GetAll());
            var result = new PageResponseApiModel<UniversityResponseApiModel>();

            if (universities == null || universities.Count() == 0)
                throw new NotFoundException();

            try
            {
                result = GetPageFromCollection(universities, page, pageSize, url);
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
                throw new NotFoundException();
            }

            foreach (var university in favoriteUnivesistes)
            {
                university.IsFavorite = true;
            }

            return _mapper.Map<IEnumerable<UniversityResponseApiModel>>(favoriteUnivesistes);
        }

        private PageResponseApiModel<T> GetPageFromCollection<T>(IEnumerable<T> list, int pageNumber, int pageSize, string url = null)
        {
            if (pageSize <= 0)
                throw new BadRequestException("Введено неправильний розмір сторінки");

            var count = list.Count();
            var totalPages = (int)Math.Ceiling((double)count / pageSize);

            if (pageNumber <= 0 || pageNumber > totalPages)
                throw new BadRequestException("Введено неправильний номер сторінки");

            var itemsOnPage = list
                .OrderBy(x => {
                    var test = x.GetType().GetProperty("Id").GetValue(x).ToString();
                    if (test != null)
                        return test;
                    return x.GetHashCode().ToString();
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            // Add url for next and prev pages
            var currentUrl = new UriBuilder(url);
            UriBuilder nextPage = null;
            UriBuilder prevPage = null;

            var query = HttpUtility.ParseQueryString(currentUrl.Query);
            query["pageSize"] = pageSize.ToString();

            if (pageNumber + 1 <= totalPages)
            {
                nextPage = new UriBuilder(url);
                query["page"] = (pageNumber + 1).ToString();
                nextPage.Query = query.ToString();
            }

            if (pageNumber - 1 > 0)
            {
                prevPage = new UriBuilder(url);
                query["page"] = (pageNumber - 1).ToString();
                prevPage.Query = query.ToString();
            }

            var result = new PageResponseApiModel<T>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                ResponseList = itemsOnPage,
                NextPage = nextPage?.ToString(),
                PrevPage = prevPage?.ToString()
            };

            return result;
        }
    }
}
