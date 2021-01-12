using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ISpecialtyRepository<SpecialtyDTO> _specialtyRepository;
        private readonly IMapper _mapper;
        public SpecialtyService(ISpecialtyRepository<SpecialtyDTO> specialtyRepository, IMapper mapper)
        {
            _specialtyRepository = specialtyRepository;
            _mapper = mapper;
        }

        public void Dispose() => _specialtyRepository.Dispose();

        public async Task<ResponseApiModel<IEnumerable<SpecialtyApiModel>>> GetAllSpecialties()
        {
            var result = new ResponseApiModel<IEnumerable<SpecialtyApiModel>>();
            var specialties = (List<SpecialtyDTO>)await _specialtyRepository.GetAllSpecialties();
            if (specialties.Count < 1)
            {
                return result.Set(false, "Спеціальностей немає");
            }
            result.Object = _mapper.Map<IEnumerable<SpecialtyApiModel>>(specialties);
            return result.Set(true);
        }

        public async Task<ResponseApiModel<SpecialtyNamesResponseApiModel>> GetAllSpecialtiesNames()
        {
            var result = new ResponseApiModel<SpecialtyNamesResponseApiModel>();
            var specialties = (List<SpecialtyDTO>)await _specialtyRepository.GetAllSpecialties();
            if (specialties.Count < 1)
            {
                return result.Set(false, "Спеціальностей немає");
            }
            var names = specialties.Select(x => x.Name).ToList();
            result.Object = new SpecialtyNamesResponseApiModel(names);
            return result.Set(true);
        }

        public async Task<ResponseApiModel<SpecialtyApiModel>> GetSpecialtyById(string id)
        {
            var result = new ResponseApiModel<SpecialtyApiModel>();
            var specialtiy = await _specialtyRepository.GetById(id);
            if (specialtiy == null)
            {
                return result.Set(false, $"Спеціальність не знайдена із таким id:  {id}.");
            }
            result.Object = _mapper.Map<SpecialtyApiModel>(specialtiy);
            return result.Set(true);
        }
    }
}
