using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Others;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class SpecialtyServiceTests
    {
        private readonly SpecialtyService _testService;
        private readonly Mock<IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>> _specialtyToUniversityRepository;
        private readonly Mock<IRepository<Speciality, SpecialityDTO>> _specialtyRepository;
        private readonly Mock<IMapper> _mapper;

        public SpecialtyServiceTests()
        {
            _specialtyToUniversityRepository = new Mock<IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>>();
            _specialtyRepository = new Mock<IRepository<Speciality, SpecialityDTO>>();
            _mapper = new Mock<IMapper>();
            _testService = new SpecialtyService(
                _specialtyToUniversityRepository.Object,
                _specialtyRepository.Object,
                _mapper.Object
                );
        }

        [Theory]
        [InlineData("Specialty", "Direction", "University", "Abbreviation")]
        [InlineData("Specialty", null, "University", null)]
        [InlineData(null, "Direction", null, "Abbreviation")]
        [InlineData(null, null, null, null)]
        public async Task GetAllSpecialtiesByFilter_EndpointReturnsOk(string specialty, string direction, string uniName, string uniAbbr)
        {
            // Arrange
            var request = new FilterApiModel
            {
                SpecialityName = specialty,
                DirectionName = direction,
                UniversityName = uniName,
                UniversityAbbreviation = uniAbbr
            };
            var specialtyDTO = new SpecialityDTO { Id = "id", Name = specialty, Direction = new DirectionDTO { Name = direction } };
            var specialtyToUniversityDTO = new SpecialityToUniversityDTO
            {
                SpecialityId = "id",
                Speciality = specialtyDTO,
                University = new UniversityDTO { Name = uniName, Abbreviation = uniAbbr }
            };

            var listSpiciality = new List<SpecialityDTO>() { specialtyDTO }.AsEnumerable();
            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(listSpiciality);
            var listSpicialityToUniversity = new List<SpecialityToUniversityDTO>() { specialtyToUniversityDTO }.AsEnumerable();
            _specialtyToUniversityRepository.Setup(s => s.Find(It.IsAny<Expression<Func<SpecialityToUniversity, bool>>>()))
                .ReturnsAsync(listSpicialityToUniversity);
            var response = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel { Name = specialty } }.AsEnumerable();
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyResponseApiModel>>(listSpiciality)).Returns(response);

            // Act
            var result = (await _testService.GetAllSpecialtiesByFilter(request)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(request.SpecialityName, result[0].Name);
        }


    }
}
