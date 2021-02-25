using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
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
        private readonly Mock<IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>> _specialtyToUniversityRepository = new Mock<IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>>();
        private static readonly Mock<IRepository<EducationFormToDescription, EducationFormToDescription>> _educationFormToDescriptionRepository = new Mock<IRepository<EducationFormToDescription, EducationFormToDescription>>();
        private static readonly Mock<IRepository<PaymentFormToDescription, PaymentFormToDescription>> _paymentFormToDescriptionRepository = new Mock<IRepository<PaymentFormToDescription, PaymentFormToDescription>>();

        private readonly Mock<IRepository<Specialty, SpecialtyDTO>> _specialtyRepository = new Mock<IRepository<Specialty, SpecialtyDTO>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();

        private readonly SpecialtyDTO _specialtyDTO = new SpecialtyDTO { Id = "id", Direction = new DirectionDTO() };
        private readonly SpecialtyToUniversityDTO _specialtyToUniversityDTO = new SpecialtyToUniversityDTO { SpecialtyId = "id", University = new UniversityDTO() };
        private readonly IEnumerable<SpecialtyDTO> _listSpecialty;
        private readonly IEnumerable<SpecialtyDTO> _blankListSpecialty = new List<SpecialtyDTO>().AsEnumerable();
        private readonly IEnumerable<SpecialtyResponseApiModel> _blankResponse = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel() }.AsEnumerable();

        public SpecialtyServiceTests()
        {
            _testService = new SpecialtyService(
                _specialtyToUniversityRepository.Object,
                _educationFormToDescriptionRepository.Object,
                _paymentFormToDescriptionRepository.Object,
                _specialtyRepository.Object,
                _mapper.Object,
                _resourceManager.Object
                );

            _listSpecialty = new List<SpecialtyDTO>() { _specialtyDTO }.AsEnumerable();
            var listSpecialtyToUniversity = new List<SpecialtyToUniversityDTO>() { _specialtyToUniversityDTO }.AsEnumerable();
            _specialtyToUniversityRepository.Setup(s => s.Find(It.IsAny<Expression<Func<SpecialtyToUniversity, bool>>>()))
                    .ReturnsAsync(listSpecialtyToUniversity);
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
                SpecialtyName = _specialtyDTO.Name = specialty,
                DirectionName = _specialtyDTO.Direction.Name = direction,
                UniversityName = _specialtyToUniversityDTO.University.Name = uniName,
                UniversityAbbreviation = _specialtyToUniversityDTO.University.Abbreviation = uniAbbr
            };

            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(_listSpecialty);
            var response = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel { Name = specialty } }.AsEnumerable();
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyResponseApiModel>>(_listSpecialty)).Returns(response);

            // Act
            var result = (await _testService.GetAllSpecialtiesByFilter(request)).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(request.SpecialtyName, result[0].Name);
        }

        [Fact]
        public async Task GetAllSpecialties_ShouldReturnAllUsers_IfDatabaseNotEmpty()
        {
            // Arrange
            var list = new List<SpecialtyDTO>() { new SpecialtyDTO() }.AsEnumerable();
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
            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(new List<SpecialtyDTO>().AsEnumerable());
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
                SpecialtyName = _specialtyDTO.Name = specialty,
                DirectionName = _specialtyDTO.Direction.Name = direction,
                UniversityName = _specialtyToUniversityDTO.University.Name = uniName,
                UniversityAbbreviation = _specialtyToUniversityDTO.University.Abbreviation = uniAbbr
            };

            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(_listSpecialty);
            var response = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel { Name = specialty } }.AsEnumerable();
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyResponseApiModel>>(_listSpecialty)).Returns(response);

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
                SpecialtyName = _specialtyDTO.Name = null,
                DirectionName = _specialtyDTO.Direction.Name = null,
                UniversityName = _specialtyToUniversityDTO.University.Name = null,
                UniversityAbbreviation = _specialtyToUniversityDTO.University.Abbreviation = null
            };

            _specialtyRepository.Setup(x => x.GetAll()).ReturnsAsync(_blankListSpecialty);
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyResponseApiModel>>(It.IsAny<IEnumerable<SpecialtyResponseApiModel>>())).Returns(_blankResponse);

            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _testService.GetSpecialtiesNamesByFilter(request));
        }

        [Fact]
        public async Task GetSpecialtyById_ShouldReturnSpecialty_ById()
        {
            // Arrange
            _specialtyRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(new SpecialtyDTO());
            _mapper.Setup(s => s.Map<SpecialtyResponseApiModel>(It.IsAny<SpecialtyDTO>())).Returns(new SpecialtyResponseApiModel());
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
            _specialtyRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync((SpecialtyDTO)null);
            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _testService.GetSpecialtyById(null));
        }

        [Fact]
        public void Dispose_ShouldDisposeRepositories()
        {
            // Arrange
            var specialtyToUniRepo = new Mock<IRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>>();
            var educationFormToDescriptionRepository = new Mock<IRepository<EducationFormToDescription, EducationFormToDescription>>();
            var paymentFormToDescriptionRepository = new Mock<IRepository<PaymentFormToDescription, PaymentFormToDescription>>();
            var specialtyRepo = new Mock<IRepository<Specialty, SpecialtyDTO>>();
            var specToUniResult = false;
            var specResult = false;
            specialtyToUniRepo.Setup(x => x.Dispose()).Callback(() => specToUniResult = true);
            specialtyRepo.Setup(x => x.Dispose()).Callback(() => specResult = true);
            
            // Act
            var service = new SpecialtyService(
                specialtyToUniRepo.Object,
                educationFormToDescriptionRepository.Object,
                paymentFormToDescriptionRepository.Object,
                specialtyRepo.Object,
                _mapper.Object,
                _resourceManager.Object);
            service.Dispose();
            
            // Assert
            Assert.True(specToUniResult);
            Assert.True(specResult);
        }

    }
}
