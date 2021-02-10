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
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO> _specialtyToUniversityRepository;
        private readonly IRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IMapper _mapper;
        public SpecialtyService(
            IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO> specialtyToUniversityRepository,
            IRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IMapper mapper)
        {
            _specialtyToUniversityRepository = specialtyToUniversityRepository;
            _specialtyRepository = specialtyRepository;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _specialtyRepository.Dispose();
            _specialtyToUniversityRepository.Dispose();
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

            if (filterModel.UniversityName != string.Empty && filterModel.UniversityName != null)
            {
                var specialtyToUniversity = await _specialtyToUniversityRepository.Find(su => su.University.Name == filterModel.UniversityName);
                var specialtyId = specialtyToUniversity.Select(su => su.SpecialtyId);
                specilaties = specilaties.Where(s => specialtyId.Contains(s.Id));
            }

            if (filterModel.UniversityAbbreviation != string.Empty && filterModel.UniversityAbbreviation != null)
            {
                var specialtyToUniversity = await _specialtyToUniversityRepository.Find(su => su.University.Abbreviation == filterModel.UniversityAbbreviation);
                var specialtyIds = specialtyToUniversity.Select(su => su.SpecialtyId);
                specilaties = specilaties.Where(s => specialtyIds.Contains(s.Id));
            }

            return _mapper.Map<IEnumerable<SpecialtyResponseApiModel>>(specilaties.Distinct().ToList());
        }

        public async Task<ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>> GetAllSpecialties()
        {
            var result = new ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>();
            var specialties = await _specialtyRepository.GetAll();
            if (specialties.Count() < 1)
            {
                throw new NotFoundException("Спеціальностей немає.");
            }
            result.Object = _mapper.Map<IEnumerable<SpecialtyResponseApiModel>>(specialties);
            return result.Set(true);
        }

        public async Task<IEnumerable<string>> GetSpecialtiesNamesByFilter(FilterApiModel filterModel)
        {
            var specialties = await GetAllSpecialtiesByFilter(filterModel);

            if (specialties == null || specialties.Count() == 0)
            {
                throw new NotFoundException("Спеціальностей немає.");
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
                throw new NotFoundException($"Спеціальність не знайдена із таким id:  {id}.");
            }
            result.Object = _mapper.Map<SpecialtyResponseApiModel>(specialty);
            return result.Set(true);
        }
    }
}
