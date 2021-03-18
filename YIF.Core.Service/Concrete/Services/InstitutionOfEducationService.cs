﻿using AutoMapper;
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
    public class InstitutionOfEducationService : IInstitutionOfEducationService<InstitutionOfEducation>
    {
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _institutionOfEducationRepository;
        private readonly IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> _directionRepository;
        private readonly IRepository<EducationFormToDescription, EducationFormToDescriptionDTO> _educationFormToDescriptionRepository;
        private readonly IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO> _paymentFormToDescriptionRepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToInstitutionOfEducationRepository;
        private readonly IGraduateRepository<Graduate, GraduateDTO> _graduateRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService _paginationService;
        private readonly ResourceManager _resourceManager;

        public InstitutionOfEducationService(
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> institutionOfEducationRepository,
            IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> directionRepository,
            IRepository<EducationFormToDescription, EducationFormToDescriptionDTO> educationFormToDescriptionRepository,
            IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO> paymentFormToDescriptionRepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToInstitutionOfEducationRepository,
            IGraduateRepository<Graduate, GraduateDTO> graduateRepository,
            IMapper mapper,
            IPaginationService paginationService,
            ResourceManager resourceManager)
        {
            _institutionOfEducationRepository = institutionOfEducationRepository;
            _directionRepository = directionRepository;
            _graduateRepository = graduateRepository;
            _educationFormToDescriptionRepository = educationFormToDescriptionRepository;
            _paymentFormToDescriptionRepository = paymentFormToDescriptionRepository;
            _specialtyToInstitutionOfEducationRepository = specialtyToInstitutionOfEducationRepository;
            _mapper = mapper;
            _paginationService = paginationService;
            _resourceManager = resourceManager;
        }

        public void Dispose() => _institutionOfEducationRepository.Dispose();

        public async Task<IEnumerable<InstitutionOfEducationResponseApiModel>> GetInstitutionOfEducationsByFilter(FilterApiModel filterModel)
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
                var directions = await _directionRepository.Find(x => x.Direction.Name == filterModel.DirectionName);
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
                var educationFormToDescription = await _educationFormToDescriptionRepository.Find(x => x.EducationForm.Name == filterModel.EducationForm);

                //From all specialtyToInstitutionOfEducation set which contains educationFormToDescription
                var specialtyToInstitutionOfEducationAll = await _specialtyToInstitutionOfEducationRepository.GetAll();
                var specialtyToInstitutionOfEducation = specialtyToInstitutionOfEducationAll
                    .Where(x => educationFormToDescription.Any(y => y.SpecialtyInInstitutionOfEducationDescriptionId == x.SpecialtyInInstitutionOfEducationDescriptionId));

                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(x => specialtyToInstitutionOfEducation.Any(y => y.InstitutionOfEducationId == x.Id));
            }

            if (filterModel.PaymentForm != string.Empty && filterModel.PaymentForm != null)
            {
                var paymentFormToDescription = await _paymentFormToDescriptionRepository.Find(x => x.PaymentForm.Name == filterModel.PaymentForm);

                var specialtyToInstitutionOfEducationAll = await _specialtyToInstitutionOfEducationRepository.GetAll();
                var specialtyToInstitutionOfEducation = specialtyToInstitutionOfEducationAll
                    .Where(x => paymentFormToDescription.Any(y => y.SpecialtyInInstitutionOfEducationDescriptionId == x.SpecialtyInInstitutionOfEducationDescriptionId));

                filteredInstitutionOfEducations = filteredInstitutionOfEducations.Where(x => specialtyToInstitutionOfEducation.Any(y => y.InstitutionOfEducationId == x.Id));
            }

            return _mapper.Map<IEnumerable<InstitutionOfEducationResponseApiModel>>(filteredInstitutionOfEducations.Distinct().ToList());
        }

        public async Task<InstitutionOfEducationResponseApiModel> GetInstitutionOfEducationById(string institutionOfEducationId, string userId = null)
        {
            var institutionOfEducation = await _institutionOfEducationRepository.Get(institutionOfEducationId);

            if (institutionOfEducation == null)
                throw new NotFoundException(_resourceManager.GetString("InstitutionOfEducationWithSuchIdNotFound"));

            var favoriteInstitutionOfEducations = await _institutionOfEducationRepository.GetFavoritesByUserId(userId);
            institutionOfEducation.IsFavorite = favoriteInstitutionOfEducations.Where(fu => fu.Id == institutionOfEducation.Id).Count() > 0;

            return _mapper.Map<InstitutionOfEducationResponseApiModel>(institutionOfEducation);
        }

        public async Task<PageResponseApiModel<InstitutionOfEducationResponseApiModel>> GetInstitutionOfEducationsPage(
            FilterApiModel filterModel,
            PageApiModel pageModel,
            string userId = null)
        {
            var institutionOfEducations = await GetInstitutionOfEducationsByFilter(filterModel);
            var result = new PageResponseApiModel<InstitutionOfEducationResponseApiModel>();

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
    }
}
