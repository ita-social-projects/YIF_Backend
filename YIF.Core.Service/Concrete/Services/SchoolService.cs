using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.School;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository<SchoolDTO> _schoolRepository;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resourceManager;

        public SchoolService(
            ISchoolRepository<SchoolDTO> schoolRepository,
            IMapper mapper,
            ResourceManager resourceManager)
        {
            _schoolRepository = schoolRepository;
            _mapper = mapper;
            _resourceManager = resourceManager;
        }

        public async Task<ResponseApiModel<IEnumerable<SchoolOnlyNameResponseApiModel>>> GetAllSchoolNames()
        {
            var result = new ResponseApiModel<IEnumerable<SchoolOnlyNameResponseApiModel>>();
            var schools = (List<SchoolDTO>)await _schoolRepository.GetAll();
            if (schools.Count() == 0)
            {
                throw new NotFoundException(_resourceManager.GetString("SchoolsNotFound"));
            }
            result.Object = _mapper.Map<IEnumerable<SchoolOnlyNameResponseApiModel>>(schools);
            return result.Set(true);
        }

        public async Task<ResponseApiModel<IEnumerable<string>>> GetAllSchoolNamesAsStrings()
        {
            var result = new ResponseApiModel<IEnumerable<string>>();
            var schools = await _schoolRepository.GetAllAsStrings();
            if (schools.Count() == 0)
            {                
                throw new NotFoundException(_resourceManager.GetString("SchoolsNotFound"));
            }
            result.Object = schools;
            return result.Set(true);
        }
    }
}
