using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class SpecialtyServiceTests
    {
        private readonly SpecialtyService _testService;
        private readonly Mock<IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>> _specialtyToUniversityRepository;
        private readonly Mock<IRepository<Speciality, SpecialityDTO>> _specialtyRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly SpecialityDTO _specialtyDTO = new SpecialityDTO { Id = "id", Direction = new DirectionDTO() };
        private readonly SpecialityToUniversityDTO _specialtyToUniversityDTO = new SpecialityToUniversityDTO { SpecialityId = "id", University = new UniversityDTO() };
        private readonly IEnumerable<SpecialityDTO> _listSpiciality;
        private readonly IEnumerable<SpecialityDTO> _blankListSpiciality = new List<SpecialityDTO>().AsEnumerable();
        private readonly IEnumerable<SpecialtyResponseApiModel> _blankResponse = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel() }.AsEnumerable();

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

            _listSpiciality = new List<SpecialityDTO>() { _specialtyDTO }.AsEnumerable();
            var listSpicialityToUniversity = new List<SpecialityToUniversityDTO>() { _specialtyToUniversityDTO }.AsEnumerable();
            _specialtyToUniversityRepository.Setup(s => s.Find(It.IsAny<Expression<Func<SpecialityToUniversity, bool>>>()))
                    .ReturnsAsync(listSpicialityToUniversity);
        }

        [Theory]
        [InlineData("Specialty", "Direction", "University", "Abbreviation")]
        [InlineData("Specialty", null, "University", null)]
        [InlineData(null, "Direction", null, "Abbreviation")]
        [InlineData(null, null, null, null)]
        public async Task GetAllSpecialtiesByFilter_ShouldReturnOk(string specialty, string direction, string uniName, string uniAbbr)
        {
            // Arrange
            var request = new FilterApiModel
            {
                SpecialityName = _specialtyDTO.Name = specialty,
                DirectionName = _specialtyDTO.Direction.Name = direction,
                UniversityName = _specialtyToUniversityDTO.University.Name = uniName,
                UniversityAbbreviation = _specialtyToUniversityDTO.University.Abbreviation = uniAbbr
            };

            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(_listSpiciality);
            var response = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel { Name = specialty } }.AsEnumerable();
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyResponseApiModel>>(_listSpiciality)).Returns(response);

            // Act
            var result = (await _testService.GetAllSpecialtiesByFilter(request)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(request.SpecialityName, result[0].Name);
        }

        [Fact]
        public async Task GetAllSpecialties_ShouldReturnAllUsers_IfDatabaseNotEmpty()
        {
            // Arrange
            var list = new List<SpecialityDTO>() { new SpecialityDTO() }.AsEnumerable();
            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(list);
            // Act
            var result = await _testService.GetAllSpecialties();
            // Assert
            Assert.True(result.Success);
            Assert.IsAssignableFrom<IEnumerable<SpecialtyResponseApiModel>>(result.Object);
        }

        [Fact]
        public async Task GetAllSpecialties_ShouldReturnException_IfDatabaseIsEmpty()
        {
            // Arrange
            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(new List<SpecialityDTO>().AsEnumerable());
            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _testService.GetAllSpecialties());
        }

        [Theory]
        [InlineData("Specialty", "Direction", "University", "Abbreviation")]
        [InlineData("Specialty", null, "University", null)]
        [InlineData("Specialty", null, null, null)]
        public async Task GetSpecialtiesNamesByFilter_ShouldReturnList_IfDatabaseNotEmpty2(string specialty, string direction, string uniName, string uniAbbr)
        {
            // Arrange
            var request = new FilterApiModel
            {
                SpecialityName = _specialtyDTO.Name = specialty,
                DirectionName = _specialtyDTO.Direction.Name = direction,
                UniversityName = _specialtyToUniversityDTO.University.Name = uniName,
                UniversityAbbreviation = _specialtyToUniversityDTO.University.Abbreviation = uniAbbr
            };

            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(_listSpiciality);
            var response = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel { Name = specialty } }.AsEnumerable();
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyResponseApiModel>>(_listSpiciality)).Returns(response);

            // Act
            var result = (await _testService.GetSpecialtiesNamesByFilter(request)).ToList();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetSpecialtiesNamesByFilter_ShouldReturnList_IfDatabaseIsEmpty()
        {
            // Arrange
            var request = new FilterApiModel
            {
                SpecialityName = _specialtyDTO.Name = null,
                DirectionName = _specialtyDTO.Direction.Name = null,
                UniversityName = _specialtyToUniversityDTO.University.Name = null,
                UniversityAbbreviation = _specialtyToUniversityDTO.University.Abbreviation = null
            };

            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(_blankListSpiciality);
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyResponseApiModel>>(It.IsAny<IEnumerable<SpecialtyResponseApiModel>>())).Returns(_blankResponse);

            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _testService.GetSpecialtiesNamesByFilter(request));
        }

        [Fact]
        public async Task GetSpecialtyById_ShouldReturnSpecialty_ById()
        {
            // Arrange
            _specialtyRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(new SpecialityDTO());
            _mapper.Setup(s => s.Map<SpecialtyResponseApiModel>(It.IsAny<SpecialityDTO>())).Returns(new SpecialtyResponseApiModel());
            // Act
            var result = await _testService.GetSpecialtyById("id");
            // Assert
            Assert.True(result.Success);
            Assert.IsType<SpecialtyResponseApiModel>(result.Object);
        }

        [Fact]
        public async Task GetSpecialtyById_ShouldReturnException_IfNotFoundSpecialty()
        {
            // Arrange
            _specialtyRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync((SpecialityDTO)null);
            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _testService.GetSpecialtyById(null));
        }

        [Fact]
        public void Dispose_ShouldDisposeRepositories()
        {
            // Arrange
            var specialtyToUniRepo = new Mock<IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>>();
            var specialtyRepo = new Mock<IRepository<Speciality, SpecialityDTO>>();
            var specToUniResult = false;
            var specResult = false;
            specialtyToUniRepo.Setup(x => x.Dispose()).Callback(() => specToUniResult = true);
            specialtyRepo.Setup(x => x.Dispose()).Callback(() => specResult = true);
            // Act
            var service = new SpecialtyService(specialtyToUniRepo.Object, specialtyRepo.Object, _mapper.Object);
            service.Dispose();
            // Assert
            Assert.True(specToUniResult);
            Assert.True(specResult);
        }

    }
}
