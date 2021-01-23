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
    public class SpecialtyService : ISpecialityService
    {
        private readonly IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> _specialtyToUniversityRepository;
        private readonly IRepository<Speciality, SpecialityDTO> _specialtyRepository;
        private readonly IMapper _mapper;
        public SpecialtyService(
            IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> specialtyToUniversityRepository,
            IRepository<Speciality, SpecialityDTO> specialtyRepository,
            IMapper mapper)
        {
            _specialtyToUniversityRepository = specialtyToUniversityRepository;
            _specialtyRepository = specialtyRepository;
            _mapper = mapper;
        }

        public void Dispose() => _specialtyToUniversityRepository.Dispose();

        public async Task<IEnumerable<SpecialtyApiModel>> GetAllSpecialtiesByFilter(FilterApiModel filterModel)
        {
            var specilaties = await _specialtyRepository.GetAll();

            if (filterModel.SpecialityName != string.Empty && filterModel.SpecialityName != null)
            {
                specilaties = specilaties.Where(s => s.Name == filterModel.SpecialityName);
            }

            if (filterModel.DirectionName != string.Empty && filterModel.DirectionName != null)
            {
                specilaties = specilaties.Where(s => s.Direction.Name == filterModel.DirectionName);
            }

            if (filterModel.UniversityName != string.Empty && filterModel.UniversityName != null)
            {
                var specialtyToUniversity = await _specialtyToUniversityRepository.Find(su => su.University.Name == filterModel.UniversityName);
                var specialtyId = specialtyToUniversity.Select(su => su.SpecialityId);
                specilaties = specilaties.Where(s => specialtyId.Contains(s.Id));
            }

            if (filterModel.UniversityAbbreviation != string.Empty && filterModel.UniversityAbbreviation != null)
            {
                var specialtyToUniversity = await _specialtyToUniversityRepository.Find(su => su.University.Abbreviation == filterModel.UniversityAbbreviation);
                var specialtyIds = specialtyToUniversity.Select(su => su.SpecialityId);
                specilaties = specilaties.Where(s => specialtyIds.Contains(s.Id));
            }

            return _mapper.Map<IEnumerable<SpecialtyApiModel>>(specilaties.Distinct().ToList());
        }

        public async Task<ResponseApiModel<IEnumerable<SpecialtyApiModel>>> GetAllSpecialties()
        {
            var result = new ResponseApiModel<IEnumerable<SpecialtyApiModel>>();
            var specialties = await _specialtyRepository.GetAll();
            if (specialties.Count() < 1)
            {
                throw new NotFoundException("Спеціальностей немає.");
            }
            result.Object = _mapper.Map<IEnumerable<SpecialtyApiModel>>(specialties);
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

        public async Task<ResponseApiModel<SpecialtyApiModel>> GetSpecialtyById(string id)
        {
            var result = new ResponseApiModel<SpecialtyApiModel>();
            var specialty = await _specialtyToUniversityRepository.Get(id);
            if (specialty == null)
            {
                throw new NotFoundException($"Спеціальність не знайдена із таким id:  {id}.");
            }
            result.Object = _mapper.Map<SpecialtyApiModel>(specialty);
            return result.Set(true);
        }
    }
}
