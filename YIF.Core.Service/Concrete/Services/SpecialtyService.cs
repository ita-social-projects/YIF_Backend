using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class SpecialtyService : ISpecialityService
    {
        private readonly ISpecialtyRepository<Speciality, SpecialityDTO> _specialtyRepository;
        private readonly IMapper _mapper;
        public SpecialtyService(ISpecialtyRepository<Speciality, SpecialityDTO> specialtyRepository, IMapper mapper)
        {
            _specialtyRepository = specialtyRepository;
            _mapper = mapper;
        }

        public void Dispose() => _specialtyRepository.Dispose();

        public async Task<ResponseApiModel<IEnumerable<SpecialtyApiModel>>> GetAllSpecialties()
        {
            var result = new ResponseApiModel<IEnumerable<SpecialtyApiModel>>();
            var specialties = (List<SpecialityDTO>)await _specialtyRepository.GetAll();
            if (specialties.Count < 1)
            {
                throw new NotFoundException("Спеціальностей немає.");
            }
            result.Object = _mapper.Map<IEnumerable<SpecialtyApiModel>>(specialties);
            return result.Set(true);
        }

        public async Task<IEnumerable<string>> GetAllSpecialtiesNames()
        {
            var specialties = await _specialtyRepository.GetNames();
            if (specialties == null || specialties.Count() == 0)
            {
                throw new NotFoundException("Спеціальностей немає.");
            }

            return specialties;
        }

        public async Task<ResponseApiModel<SpecialtyApiModel>> GetSpecialtyById(string id)
        {
            var result = new ResponseApiModel<SpecialtyApiModel>();
            var specialtiy = await _specialtyRepository.Get(id);
            if (specialtiy == null)
            {
                throw new NotFoundException($"Спеціальність не знайдена із таким id:  {id}.");
            }
            result.Object = _mapper.Map<SpecialtyApiModel>(specialtiy);
            return result.Set(true);
        }
    }
}
