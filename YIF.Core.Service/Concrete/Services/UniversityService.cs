using AutoMapper;
using SendGrid.Helpers.Errors.Model;
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
    public class UniversityService : IUniversityService<University>
    {
        private readonly IUniversityRepository<University, UniversityDTO> _universityRepository;
        private readonly IRepository<DirectionToUniversity, DirectionToUniversityDTO> _directionRepository;
        private readonly IRepository<EducationFormToDescription, EducationFormToDescription> _educationFormToDescriptionRepository;
        private readonly IRepository<PaymentFormToDescription, PaymentFormToDescription> _paymentFormToDescriptionRepository;
        private readonly IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO> _specialtyToUniversityRepository;
        private readonly IGraduateRepository<Graduate, GraduateDTO> _graduateRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly ResourceManager _resourceManager;

        public UniversityService(
            IUniversityRepository<University, UniversityDTO> universityRepository,
            IRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IRepository<DirectionToUniversity, DirectionToUniversityDTO> directionRepository,
            IRepository<EducationFormToDescription, EducationFormToDescription> educationFormToDescriptionRepository,
            IRepository<PaymentFormToDescription, PaymentFormToDescription> paymentFormToDescriptionRepository,
            IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO> specialtyToUniversityRepository,
            IGraduateRepository<Graduate, GraduateDTO> graduateRepository,
            IMapper mapper,
            IPaginationService paginationService,
            ResourceManager resourceManager)
        {
            _universityRepository = universityRepository;
            _directionRepository = directionRepository;
            _graduateRepository = graduateRepository;
            _educationFormToDescriptionRepository = educationFormToDescriptionRepository;
            _paymentFormToDescriptionRepository = paymentFormToDescriptionRepository;
            _specialtyToUniversityRepository = specialtyToUniversityRepository;
            _mapper = mapper;
            _paginationService = paginationService;
            _resourceManager = resourceManager;
        }

        public void Dispose() => _universityRepository.Dispose();

        public async Task<IEnumerable<UniversityResponseApiModel>> GetUniversitiesByFilter(FilterApiModel filterModel)
        {
            // Filtered list of universities 
            var filteredUniversities = await _universityRepository.GetAll();

            if (filterModel.UniversityName != string.Empty && filterModel.UniversityName != null)
            {
                filteredUniversities = filteredUniversities.Where(x => x.Name == filterModel.UniversityName);
            }

            if (filterModel.UniversityAbbreviation != string.Empty && filterModel.UniversityAbbreviation != null)
            {
                filteredUniversities = filteredUniversities.Where(x => x.Abbreviation == filterModel.UniversityAbbreviation);
            }

            // Check if we must filter universities by direction name
            if (filterModel.DirectionName != string.Empty && filterModel.DirectionName != null)
            {
                // Get all directions by name
                var directions = await _directionRepository.Find(x => x.Direction.Name == filterModel.DirectionName);
                var universityId = directions.Select(d => d.UniversityId);

                // Get universities by these directions
                filteredUniversities = filteredUniversities.Where(fu => universityId.Contains(fu.Id));
            }

            if (filterModel.SpecialtyName != string.Empty && filterModel.SpecialtyName != null)
            {
                // Get all specialties by name
                var specialties = await _specialtyToUniversityRepository.Find(x => x.Specialty.Name == filterModel.SpecialtyName);

                filteredUniversities = filteredUniversities.Where(x => specialties.Any(y => y.UniversityId == x.Id));
            }

            if (filterModel.EducationForm != string.Empty && filterModel.EducationForm != null)
            {
                //Filtering for educationFormToDescription that has such EducationForm
                var educationFormToDescription = await _educationFormToDescriptionRepository.Find(x => x.EducationForm.Name == filterModel.EducationForm);

                //From all specialtyToUniversity set which contains educationFormToDescription
                var specialtyToUniversityAll = await _specialtyToUniversityRepository.GetAll();
                var specialtyToUniversity = specialtyToUniversityAll.Where(x => educationFormToDescription.Any(y => y.SpecialtyInUniversityDescriptionId == x.SpecialtyInUniversityDescriptionId));

                filteredUniversities = filteredUniversities.Where(x => specialtyToUniversity.Any(y => y.UniversityId == x.Id));
            }

            if (filterModel.PaymentForm != string.Empty && filterModel.PaymentForm != null)
            {
                //Filtering for paymentFormToDescription that has such PaymentForm
                var paymentFormToDescription = await _paymentFormToDescriptionRepository.Find(x => x.PaymentForm.Name == filterModel.PaymentForm);

                //From all specialtyToUniversity set which contains paymentFormToDescription
                var specialtyToUniversityAll = await _specialtyToUniversityRepository.GetAll();
                var specialtyToUniversity = specialtyToUniversityAll.Where(x => paymentFormToDescription.Any(y => y.SpecialtyInUniversityDescriptionId == x.SpecialtyInUniversityDescriptionId));

                filteredUniversities = filteredUniversities.Where(x => specialtyToUniversity.Any(y => y.UniversityId == x.Id));
            }

            return _mapper.Map<IEnumerable<UniversityResponseApiModel>>(filteredUniversities.Distinct().ToList());
        }

        public async Task<UniversityResponseApiModel> GetUniversityById(string universityId, string userId = null)
        {
            var university = await _universityRepository.Get(universityId);

            if (university == null)
                throw new NotFoundException(_resourceManager.GetString("UniversityWithSuchIdNotFound"));

            var favoriteUniversities = await _universityRepository.GetFavoritesByUserId(userId);
            university.IsFavorite = favoriteUniversities.Where(fu => fu.Id == university.Id).Count() > 0;

            return _mapper.Map<UniversityResponseApiModel>(university);
        }

        public async Task<PageResponseApiModel<UniversityResponseApiModel>> GetUniversitiesPage(
            FilterApiModel filterModel,
            PageApiModel pageModel,
            string userId = null)
        {
            var universities = await GetUniversitiesByFilter(filterModel);
            var result = new PageResponseApiModel<UniversityResponseApiModel>();

            if (universities == null || universities.Count() == 0)
                throw new NotFoundException(_resourceManager.GetString("UniversitiesNotFound"));

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
            var favoriteUniversities = await _universityRepository.GetFavoritesByUserId(userId);
            if (favoriteUniversities == null || favoriteUniversities.Count() == 0)
            {
                throw new NotFoundException(_resourceManager.GetString("FavoriteUniversitiesNotFound"));
            }

            foreach (var university in favoriteUniversities)
            {
                university.IsFavorite = true;
            }

            return _mapper.Map<IEnumerable<UniversityResponseApiModel>>(favoriteUniversities);
        }

        public async Task<IEnumerable<string>> GetUniversityAbbreviations(FilterApiModel filterModel)
        {
            var university = await GetUniversitiesByFilter(filterModel);

            if (university == null || university.Count() == 0)
                throw new NotFoundException(_resourceManager.GetString("UniversitiesNotFound"));

            return university
                .Select(u => u.Abbreviation)
                .Where(a => a != null)
                .OrderBy(a => a);
        }

        public async Task AddUniversityToFavorite(string universityId, string userId)
        {
            var favorites = await _universityRepository.GetFavoritesByUserId(userId);
            var university = await _universityRepository.Get(universityId);
            var graduate = await _graduateRepository.GetByUserId(userId);

            if (favorites.Where(f => f.Id == universityId).Count() > 0)
                throw new BadRequestException(_resourceManager.GetString("UniversityIsAlreadyFavorite"));

            if (university == null)
                throw new BadRequestException(_resourceManager.GetString("UniversityNotFound"));

            if (graduate == null)
                throw new BadRequestException(_resourceManager.GetString("GraduateNotFound"));

            await _universityRepository.AddFavorite(new UniversityToGraduate
            {
                UniversityId = university.Id,
                GraduateId = graduate.Id
            });
        }

        public async Task DeleteUniversityFromFavorite(string universityId, string userId)
        {
            var favorites = await _universityRepository.GetFavoritesByUserId(userId);
            var university = await _universityRepository.Get(universityId);
            var graduate = await _graduateRepository.GetByUserId(userId);

            if (favorites.Where(f => f.Id == universityId).Count() == 0)
                throw new BadRequestException(_resourceManager.GetString("UniversityIsNotFavorite"));

            if (university == null)
                throw new BadRequestException(_resourceManager.GetString("UniversityNotFound"));

            if (graduate == null)
                throw new BadRequestException(_resourceManager.GetString("GraduateNotFound"));

            await _universityRepository.RemoveFavorite(new UniversityToGraduate
            {
                UniversityId = university.Id,
                GraduateId = graduate.Id
            });
        }
    }
}
