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
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToInstitutionOfEducationRepository;
        private readonly IRepository<EducationFormToDescription, EducationFormToDescriptionDTO> _educationFormToDescriptionRepository;
        private readonly IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO> _paymentFormToDescriptionRepository;
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _institutionOfEducationRepository;
        private readonly IGraduateRepository<Graduate, GraduateDTO> _graduateRepository;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resourceManager;

        public SpecialtyService(
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToInstitutionOfEducationRepository,
            IRepository<EducationFormToDescription, EducationFormToDescriptionDTO> educationFormToDescriptionRepository,
            IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO> paymentFormToDescriptionRepository,
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> institutionOfEducationRepository,
            IGraduateRepository<Graduate, GraduateDTO> graduateRepository,
            IMapper mapper,
            ResourceManager resourceManager)
        {
            _specialtyToInstitutionOfEducationRepository = specialtyToInstitutionOfEducationRepository;
            _educationFormToDescriptionRepository = educationFormToDescriptionRepository;
            _paymentFormToDescriptionRepository = paymentFormToDescriptionRepository;
            _specialtyRepository = specialtyRepository;
            _institutionOfEducationRepository = institutionOfEducationRepository;
            _graduateRepository = graduateRepository;
            _mapper = mapper;
            _resourceManager = resourceManager;
        }

        public void Dispose()
        {
            _specialtyRepository.Dispose();
            _specialtyToInstitutionOfEducationRepository.Dispose();
        }

        public async Task<IEnumerable<SpecialtyResponseApiModel>> GetAllSpecialtiesByFilter(FilterApiModel filterModel)
        {
            var specilaties = await _specialtyRepository.GetAll();

            if (filterModel.SpecialtyName != string.Empty && filterModel.SpecialtyName != null)
            {
                specilaties = specilaties.Where(s => s.Name == filterModel.SpecialtyName);
            }

            if (filterModel.DirectionName != string.Empty && filterModel.DirectionName != null)
            {
                specilaties = specilaties.Where(s => s.Direction.Name == filterModel.DirectionName);
            }

            if (filterModel.InstitutionOfEducationName != string.Empty && filterModel.InstitutionOfEducationName != null)
            {
                var specialtyToInstitutionOfEducation = await _specialtyToInstitutionOfEducationRepository.Find(su => su.InstitutionOfEducation.Name == filterModel.InstitutionOfEducationName);
                var specialtyId = specialtyToInstitutionOfEducation.Select(su => su.SpecialtyId);
                specilaties = specilaties.Where(s => specialtyId.Contains(s.Id));
            }

            if (filterModel.InstitutionOfEducationAbbreviation != string.Empty && filterModel.InstitutionOfEducationAbbreviation != null)
            {
                var specialtyToInstitutionOfEducation = await _specialtyToInstitutionOfEducationRepository.Find(su => su.InstitutionOfEducation.Abbreviation == filterModel.InstitutionOfEducationAbbreviation);
                var specialtyIds = specialtyToInstitutionOfEducation.Select(su => su.SpecialtyId);
                specilaties = specilaties.Where(s => specialtyIds.Contains(s.Id));
            }

            if (filterModel.EducationForm != string.Empty && filterModel.EducationForm != null)
            {
                //Filtering for educationFormToDescription that has such EducationForm
                var educationFormToDescription = await _educationFormToDescriptionRepository.Find(x => x.EducationForm.Name == filterModel.EducationForm);

                //From all specialtyToInstitutionOfEducation set which contains educationFormToDescription
                var specialtyToInstitutionOfEducationAll = await _specialtyToInstitutionOfEducationRepository.GetAll();
                var specialtyToInstitutionOfEducation = specialtyToInstitutionOfEducationAll
                    .Where(x => educationFormToDescription.Any(y => y.SpecialtyInInstitutionOfEducationDescriptionId == x.SpecialtyInInstitutionOfEducationDescriptionId));

                specilaties = specilaties.Where(x => specialtyToInstitutionOfEducation.Any(y => y.SpecialtyId == x.Id));
            }

            if (filterModel.PaymentForm != string.Empty && filterModel.PaymentForm != null)
            {
                var paymentFormToDescription = await _paymentFormToDescriptionRepository.Find(x => x.PaymentForm.Name == filterModel.PaymentForm);

                var specialtyToInstitutionOfEducationAll = await _specialtyToInstitutionOfEducationRepository.GetAll();
                var specialtyToInstitutionOfEducation = specialtyToInstitutionOfEducationAll
                    .Where(x => paymentFormToDescription.Any(y => y.SpecialtyInInstitutionOfEducationDescriptionId == x.SpecialtyInInstitutionOfEducationDescriptionId));

                specilaties = specilaties.Where(x => specialtyToInstitutionOfEducation.Any(y => y.SpecialtyId == x.Id));
            }
            return _mapper.Map<IEnumerable<SpecialtyResponseApiModel>>(specilaties.Distinct().ToList());
        }

        public async Task<ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>> GetAllSpecialties()
        {
            var result = new ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>();
            var specialties = await _specialtyRepository.GetAll();
            if (specialties.Count() < 1)
            {
                throw new NotFoundException(_resourceManager.GetString("SpecialtiesNotFound"));
            }
            result.Object = _mapper.Map<IEnumerable<SpecialtyResponseApiModel>>(specialties);
            return result.Set(true);
        }

        public async Task<IEnumerable<string>> GetSpecialtiesNamesByFilter(FilterApiModel filterModel)
        {
            var specialties = await GetAllSpecialtiesByFilter(filterModel);

            if (specialties == null || specialties.Count() == 0)
            {
                throw new NotFoundException(_resourceManager.GetString("SpecialtiesNotFound"));
            }

            return specialties
                .Select(s => s.Name)
                .Where(n => n != null)
                .OrderBy(n => n);
        }

        public async Task<ResponseApiModel<SpecialtyResponseApiModel>> GetSpecialtyById(string id)
        {
            var result = new ResponseApiModel<SpecialtyResponseApiModel>();
            var specialty = await _specialtyRepository.Get(id);
            if (specialty == null)
            {                
                throw new NotFoundException($"{_resourceManager.GetString("SpecialtyWithSuchIdNotFound")}: {id}");
            }
            result.Object = _mapper.Map<SpecialtyResponseApiModel>(specialty);
            return result.Set(true);
        }
        public async Task<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>> GetAllSpecialtyDescriptionsById(string id)
        {
            var specialtyDescriptions = await _specialtyToInstitutionOfEducationRepository.GetSpecialtyInInstitutionOfEducationDescriptionsById(id);
            if (specialtyDescriptions.Count() < 1)
            {
                throw new NotFoundException(_resourceManager.GetString("SpecialtyDescriptionsNotFound"));
            }
            var result = _mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>>(specialtyDescriptions);
            return result;
        }
        public async Task AddSpecialtyAndInstitutionOfEducationToFavorite(string specialtyId,string institutionOfEducationId, string userId)
        {
            var graduate = await _graduateRepository.GetByUserId(userId);

            var entity = new SpecialtyToInstitutionOfEducationToGraduate()
            {
                SpecialtyId = specialtyId,
                InstitutionOfEducationId = institutionOfEducationId,
                GraduateId = graduate.Id
            };

            var favorites = await _specialtyToInstitutionOfEducationRepository.FavoriteContains(entity);
            var institutionOfEducation = await _institutionOfEducationRepository.ContainsById(institutionOfEducationId);
            var specialty = await _specialtyRepository.ContainsById(specialtyId);

            if (favorites == true)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyAndInstitutionOfEducationIsAlreadyFavorite"));

            if (graduate == null)
                throw new BadRequestException(_resourceManager.GetString("GraduateNotFound"));

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));

            await _specialtyToInstitutionOfEducationRepository.AddFavorite(entity);
        }
        public async Task DeleteSpecialtyAndInstitutionOfEducationFromFavorite(string specialtyId ,string institutionOfEducationId, string userId)
        {
            var graduate = await _graduateRepository.GetByUserId(userId);

            var entity = new SpecialtyToInstitutionOfEducationToGraduate()
            {
                SpecialtyId = specialtyId,
                InstitutionOfEducationId = institutionOfEducationId,
                GraduateId = graduate.Id
            };

            var favorites = await _specialtyToInstitutionOfEducationRepository.FavoriteContains(entity);
            var institutionOfEducation = await _institutionOfEducationRepository.ContainsById(institutionOfEducationId);
            var specialty = await _specialtyRepository.ContainsById(specialtyId);

            if (favorites == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyAndInstitutionOfEducationIsNotFavorite"));

            if (graduate == null)
                throw new BadRequestException(_resourceManager.GetString("GraduateNotFound"));

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));

            await _specialtyToInstitutionOfEducationRepository.RemoveFavorite(entity);
        }
    }
}
