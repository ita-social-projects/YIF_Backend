using AutoMapper;
using System;
using System.Collections.Generic;
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
                return result.Set(false, $"There are no specialties");
            }
            result.Object = _mapper.Map<IEnumerable<UserApiModel>>(users);
            return result.Set(true);
        }

        public async Task<ResponseApiModel<SpecialtyNamesApiModel>> GetAllSpecialtiesNames()
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseApiModel<SpecialtyApiModel>> GetSpecialtyById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseApiModel<SpecialtyApiModel>> GetSpecialtyByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
