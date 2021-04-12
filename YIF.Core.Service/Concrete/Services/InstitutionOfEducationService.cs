using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class InstitutionOfEducationService : IInstitutionOfEducationService<InstitutionOfEducation>
    {
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _institutionOfEducationRepository;
        private readonly IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> _directionToIoERepository;
        private readonly IDirectionRepository<Direction, DirectionDTO> _directionRepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToInstitutionOfEducationRepository;
        private readonly ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> _specialtyToIoEDescriptionRepository;
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;
        private readonly IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO> _institutionOfEducationModeratorRepository;
        private readonly IGraduateRepository<Graduate, GraduateDTO> _graduateRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly ResourceManager _resourceManager;
        private readonly IConfiguration _configuration;

        public InstitutionOfEducationService(
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> institutionOfEducationRepository,
            IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> directionToIoERepository,
            IDirectionRepository<Direction, DirectionDTO> directionRepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToInstitutionOfEducationRepository,
            ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> specialtyToIoEDescriptionRepository,
            IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO> institutionOfEducationAdminRepository,
            IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO> institutionOfEducationModeratorRepository,
            IGraduateRepository<Graduate, GraduateDTO> graduateRepository,
            IMapper mapper,
            IPaginationService paginationService,
            ResourceManager resourceManager,
            IConfiguration configuration)
        {
            _institutionOfEducationRepository = institutionOfEducationRepository;
            _directionToIoERepository = directionToIoERepository;
            _directionRepository = directionRepository;
            _graduateRepository = graduateRepository;
            _specialtyToInstitutionOfEducationRepository = specialtyToInstitutionOfEducationRepository;
            _specialtyToIoEDescriptionRepository = specialtyToIoEDescriptionRepository;
            _institutionOfEducationAdminRepository = institutionOfEducationAdminRepository;
            _institutionOfEducationModeratorRepository = institutionOfEducationModeratorRepository;
            _mapper = mapper;
            _paginationService = paginationService;
            _resourceManager = resourceManager;
            _configuration = configuration;
        }

        public void Dispose() => _institutionOfEducationRepository.Dispose();

        public async Task<IEnumerable<InstitutionsOfEducationResponseApiModel>> GetInstitutionOfEducationsByFilter(FilterApiModel filterModel)
        {
            // Filtered list of institutionOfEducations 
            var filteredInstitutionOfEducations = await _institutionOfEducationRepository.GetAll();

            if (filterModel.InstitutionOfEducationName != string.Empty && filterModel.InstitutionOfEducationName != null)
            {
                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(x => x.Name == filterModel.InstitutionOfEducationName);
            }

            if (filterModel.InstitutionOfEducationAbbreviation != string.Empty && filterModel.InstitutionOfEducationAbbreviation != null)
            {
                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(x => x.Abbreviation == filterModel.InstitutionOfEducationAbbreviation);
            }

            // Check if we must filter institutionOfEducations by direction name
            if (filterModel.DirectionName != string.Empty && filterModel.DirectionName != null)
            {
                // Get all directions by name
                var directions = await _directionToIoERepository.Find(x => x.Direction.Name == filterModel.DirectionName);
                var institutionOfEducationId = directions.Select(d => d.InstitutionOfEducationId);

                // Get institutionOfEducations by these directions
                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(fu => institutionOfEducationId.Contains(fu.Id));
            }

            if (filterModel.SpecialtyName != string.Empty && filterModel.SpecialtyName != null)
            {
                // Get all specialties by name
                var specialties = await _specialtyToInstitutionOfEducationRepository.Find(x => x.Specialty.Name == filterModel.SpecialtyName);

                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(x => specialties.Any(y => y.InstitutionOfEducationId == x.Id));
            }

            if (filterModel.EducationForm != string.Empty && filterModel.EducationForm != null)
            {
                //Filtering for educationFormToDescription that has such EducationForm
                var specialtyToIoEDescription = await _specialtyToIoEDescriptionRepository.Find(x => x.EducationForm == (EducationForm)Enum.Parse(typeof(EducationForm), filterModel.EducationForm));

                //From all specialtyToInstitutionOfEducation set which contains educationFormToDescription
                var specialtyToInstitutionOfEducationAll = await _specialtyToInstitutionOfEducationRepository.GetAll();
                var specialtyToInstitutionOfEducation = specialtyToInstitutionOfEducationAll
                    .Where(x => specialtyToIoEDescription.Any(y => y.SpecialtyToInstitutionOfEducationId == x.Id));

                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(x => specialtyToInstitutionOfEducation.Any(y => y.InstitutionOfEducationId == x.Id));
            }

            if (filterModel.PaymentForm != string.Empty && filterModel.PaymentForm != null)
            {
                var specialtyToIoEDescription = await _specialtyToIoEDescriptionRepository.Find(x => x.PaymentForm == (PaymentForm)Enum.Parse(typeof(PaymentForm), filterModel.PaymentForm));

                var specialtyToInstitutionOfEducationAll = await _specialtyToInstitutionOfEducationRepository.GetAll();
                var specialtyToInstitutionOfEducation = specialtyToInstitutionOfEducationAll
                    .Where(x => specialtyToIoEDescription.Any(y => y.SpecialtyToInstitutionOfEducationId == x.Id));

                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(x => specialtyToInstitutionOfEducation.Any(y => y.InstitutionOfEducationId == x.Id));
            }

            return _mapper.Map<IEnumerable<InstitutionsOfEducationResponseApiModel>>(filteredInstitutionOfEducations.Distinct().ToList());
        }

        public async Task<InstitutionOfEducationResponseApiModel> GetInstitutionOfEducationById(string institutionOfEducationId, HttpRequest request, string userId = null)
        {
            var institutionOfEducation = await _institutionOfEducationRepository.Get(institutionOfEducationId);

            if (institutionOfEducation == null)
                throw new NotFoundException(_resourceManager.GetString("InstitutionOfEducationWithSuchIdNotFound"));

            var favoriteInstitutionOfEducations = await _institutionOfEducationRepository.GetFavoritesByUserId(userId);
            institutionOfEducation.IsFavorite = favoriteInstitutionOfEducations.Where(fu => fu.Id == institutionOfEducation.Id).Count() > 0;

            var response  = _mapper.Map<InstitutionOfEducationResponseApiModel>(institutionOfEducation);
            var directions = _mapper.Map<IEnumerable<DirectionForIoEResponseApiModel>>(await _directionRepository.GetByIoEId(institutionOfEducationId));

            string pathPhoto = $"{request.Scheme}://{request.Host}/{_configuration.GetValue<string>("UrlImages")}/";
            response.ImagePath = response.ImagePath != null ? pathPhoto + response.ImagePath : null;

            response.Directions = directions;
            return response;
        }

        public async Task<PageResponseApiModel<InstitutionsOfEducationResponseApiModel>> GetInstitutionOfEducationsPage(
            FilterApiModel filterModel,
            PageApiModel pageModel)
        {
            var institutionOfEducations = await GetInstitutionOfEducationsByFilter(filterModel);
            var result = new PageResponseApiModel<InstitutionsOfEducationResponseApiModel>();

            if (institutionOfEducations == null || institutionOfEducations.Count() == 0)
                throw new NotFoundException(_resourceManager.GetString("InstitutionOfEducationsNotFound"));

            try
            {
                result = _paginationService.GetPageFromCollection(institutionOfEducations, pageModel);
            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<PageResponseApiModel<InstitutionsOfEducationResponseApiModel>> GetInstitutionOfEducationsPageForUser(
            FilterApiModel filterModel,
            PageApiModel pageModel, 
            string userId)
        {
            var result = await GetInstitutionOfEducationsPage(filterModel, pageModel);

            // Set the value to favorites
            var favoriteInstitutionOfEducations = await _institutionOfEducationRepository.GetFavoritesByUserId(userId);
            foreach (var institutionOfEducation in result.ResponseList)
            {
                institutionOfEducation.IsFavorite = favoriteInstitutionOfEducations.Where(fu => fu.Id == institutionOfEducation.Id).Count() > 0;
            }

            return result;
        }

        public async Task<IEnumerable<InstitutionOfEducationResponseApiModel>> GetFavoriteInstitutionOfEducations(string userId)
        {
            var favoriteInstitutionOfEducations = await _institutionOfEducationRepository.GetFavoritesByUserId(userId);
            if (favoriteInstitutionOfEducations == null || favoriteInstitutionOfEducations.Count() == 0)
            {
                throw new NotFoundException(_resourceManager.GetString("FavoriteInstitutionOfEducationsNotFound"));
            }

            foreach (var institutionOfEducation in favoriteInstitutionOfEducations)
            {
                institutionOfEducation.IsFavorite = true;
            }

            return _mapper.Map<IEnumerable<InstitutionOfEducationResponseApiModel>>(favoriteInstitutionOfEducations);
        }

        public async Task<IEnumerable<string>> GetInstitutionOfEducationAbbreviations(FilterApiModel filterModel)
        {
            var institutionOfEducation = await GetInstitutionOfEducationsByFilter(filterModel);

            if (institutionOfEducation == null || institutionOfEducation.Count() == 0)
                throw new NotFoundException(_resourceManager.GetString("InstitutionOfEducationsNotFound"));

            return institutionOfEducation
                .Select(u => u.Abbreviation)
                .Where(a => a != null)
                .OrderBy(a => a);
        }

        public async Task AddInstitutionOfEducationToFavorite(string institutionOfEducationId, string userId)
        {
            var favorites = await _institutionOfEducationRepository.GetFavoritesByUserId(userId);
            var institutionOfEducation = await _institutionOfEducationRepository.Get(institutionOfEducationId);
            var graduate = await _graduateRepository.GetByUserId(userId);

            if (favorites.Where(f => f.Id == institutionOfEducationId).Count() > 0)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationIsAlreadyFavorite"));

            if (institutionOfEducation == null)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (graduate == null)
                throw new BadRequestException(_resourceManager.GetString("GraduateNotFound"));

            await _institutionOfEducationRepository.AddFavorite(new InstitutionOfEducationToGraduate
            {
                InstitutionOfEducationId = institutionOfEducation.Id,
                GraduateId = graduate.Id
            });
        }

        public async Task DeleteInstitutionOfEducationFromFavorite(string institutionOfEducationId, string userId)
        {
            var favorites = await _institutionOfEducationRepository.GetFavoritesByUserId(userId);
            var institutionOfEducation = await _institutionOfEducationRepository.Get(institutionOfEducationId);
            var graduate = await _graduateRepository.GetByUserId(userId);

            if (favorites.Where(f => f.Id == institutionOfEducationId).Count() == 0)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationIsNotFavorite"));

            if (institutionOfEducation == null)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (graduate == null)
                throw new BadRequestException(_resourceManager.GetString("GraduateNotFound"));

            await _institutionOfEducationRepository.RemoveFavorite(new InstitutionOfEducationToGraduate
            {
                InstitutionOfEducationId = institutionOfEducation.Id,
                GraduateId = graduate.Id
            });
        }

        public async Task<IEnumerable<DirectionToIoEResponseApiModel>> GetAllDirectionsAndSpecialitiesInIoE(string userId)
        {
            var admin = await _institutionOfEducationAdminRepository.GetByUserId(userId);

            string ioeId;

            if (admin == null)
            {
                var moderator = await _institutionOfEducationModeratorRepository.GetByUserId(userId);

                if (moderator == null)
                {
                    throw new BadRequestException(_resourceManager.GetString("IoEAdminOrModeratorNotFound"));
                }

                ioeId = moderator.Admin.InstitutionOfEducationId;
            }
            else
            {
                ioeId = admin.InstitutionOfEducationId;
            }

            var institutionOfEducation = await _institutionOfEducationRepository.ContainsById(ioeId);

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            var ioEdirections = await _directionToIoERepository.Find(x => x.InstitutionOfEducationId == ioeId);
            var response = _mapper.Map<IEnumerable<DirectionToIoEResponseApiModel>>(ioEdirections);
            foreach (DirectionToIoEResponseApiModel responseApiModel in response)
            {
                responseApiModel.Specialties = _mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>>
                    (await _specialtyToInstitutionOfEducationRepository
                    .Find(s => s.Specialty.DirectionId == responseApiModel.Id && s.InstitutionOfEducationId == ioeId));
            }
            return response;
        }
    }
}
