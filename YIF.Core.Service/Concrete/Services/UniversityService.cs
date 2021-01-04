using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> _specialityRepository;
        private readonly IRepository<DirectionToUniversity, DirectionToUniversityDTO> _directionRepository;
        private readonly IMapper _mapper;

        public UniversityService(
            IRepository<University, UniversityDTO> universityRepository,
            IRepository<SpecialityToUniversity, SpecialityToUniversityDTO> specialityRepository,
            IRepository<DirectionToUniversity, DirectionToUniversityDTO> directionRepository,
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
            // Filtered list of universities 
            IEnumerable<UniversityDTO> filteredUniversities = null;

            // Check if we must filter universities by direction name
            if(filterModel.DirectionName != string.Empty)
            {
                // Get all directions by name
                var directions = await _directionRepository.Find(x => x.Direction.Name == filterModel.DirectionName);
                // Get universities by these directions
                filteredUniversities = directions.Select(x => x.University);
            }

            if(filterModel.SpecialityName != string.Empty)
            {
                // Get all specialities by name
                var specialities = await _specialityRepository.Find(x => x.Speciality.Name == filterModel.SpecialityName);

                if (filteredUniversities.Count() > 1)
                {
                    // Inner join between directions and specialities 
                    filteredUniversities = filteredUniversities.Where(x => specialities.Any(y => y.UniversityId == x.Id));
                }
                else
                {
                    filteredUniversities = specialities.Select(x => x.University);
                }
            } 

            Expression<Func<University, bool>> linqFunc = null;
            if (filterModel.UniversityName != string.Empty)
            {
                if(filteredUniversities.Count() > 1)
                {
                    filteredUniversities = filteredUniversities.Where(x => x.Name == filterModel.UniversityName);
                } else
                {
                    linqFunc = (x => x.Name.ToLower().Contains(filterModel.UniversityName));
                    filteredUniversities = await _universityRepository.Find(linqFunc);
                }
            } 

            result.Object = _mapper.Map<IEnumerable<UniversityFilterResponseApiModel>>(filteredUniversities.ToList());
            return result.Set(true);
        }
    }
}
