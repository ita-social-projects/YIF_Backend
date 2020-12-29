using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class UniversityService : IUniversityService<University>
    {
        private readonly IRepository<University, UniversityDTO> _universityRepository;
        private readonly IRepository<Speciality, SpecialityDTO> _specialityRepository;
        private readonly IRepository<Direction, DirectionDTO> _directionRepository;
        private readonly IMapper _mapper;

        public UniversityService(
            IRepository<University, UniversityDTO> universityRepository,
            IRepository<Speciality, SpecialityDTO> specialityRepository,
            IRepository<Direction, DirectionDTO> directionRepository,
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

            if(filterModel.DirectionName != string.Empty)
            {
                var directions = _directionRepository.Find(x => x.Name == filterModel.DirectionName);
            }

            if(filterModel.SpecialityName != string.Empty)
            {
                var specialities = _specialityRepository.Find(x => x.Name == filterModel.SpecialityName);
            }

            IEnumerable<UniversityDTO> universities = null;
            Expression<Func<University, bool>> linqFunc = null;
            if (filterModel.UniversityName != string.Empty)
            {
                linqFunc = (x => x.Name.ToLower().Contains(filterModel.UniversityName));
                universities = await _universityRepository.Find(linqFunc);
            }




            return result.Set(true);
        }
    }
}
