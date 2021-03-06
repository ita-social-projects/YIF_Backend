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
        private readonly SpecialtyService _specialtyService;
        private readonly Mock<ISpecialtyToUniversityRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>> _specialtyToUniversityRepository = new Mock<ISpecialtyToUniversityRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>>();
        private readonly Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>> _educationFormToDescriptionRepository = new Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>>();
        private readonly Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>> _paymentFormToDescriptionRepository = new Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>>();
        private readonly Mock<IUniversityRepository<University, UniversityDTO>> _universityRepository = new Mock<IUniversityRepository<University, UniversityDTO>>();
        private readonly Mock<IGraduateRepository<Graduate, GraduateDTO>> _graduateRepository = new Mock<IGraduateRepository<Graduate, GraduateDTO>>();
        private readonly Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>> _specialtyRepository = new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();

        private readonly SpecialtyDTO _specialtyDTO = new SpecialtyDTO { Id = "id", Direction = new DirectionDTO() };
        private readonly SpecialtyToUniversityDTO _specialtyToUniversityDTO = new SpecialtyToUniversityDTO { SpecialtyId = "id", University = new UniversityDTO(), SpecialtyInUniversityDescription = new SpecialtyInUniversityDescriptionDTO() };
        private readonly IEnumerable<SpecialtyDTO> _listSpecialty;
        private readonly IEnumerable<SpecialtyDTO> _blankListSpecialty = new List<SpecialtyDTO>().AsEnumerable();
        private readonly IEnumerable<SpecialtyResponseApiModel> _blankResponse = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel() }.AsEnumerable();

        public SpecialtyServiceTests()
        {
            _specialtyService = new SpecialtyService(
                _specialtyToUniversityRepository.Object,
                _educationFormToDescriptionRepository.Object,
                _paymentFormToDescriptionRepository.Object,
                _specialtyRepository.Object,
                _universityRepository.Object,
                _graduateRepository.Object,
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
            var result = (await _specialtyService.GetAllSpecialtiesByFilter(request)).ToList();

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
            var result = await _specialtyService.GetAllSpecialties();

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
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _specialtyService.GetAllSpecialties());
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
            var result = (await _specialtyService.GetSpecialtiesNamesByFilter(request)).ToList();

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
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _specialtyService.GetSpecialtiesNamesByFilter(request));
        }

        [Fact]
        public async Task GetSpecialtyById_ShouldReturnSpecialty_ById()
        {
            // Arrange
            _specialtyRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(new SpecialtyDTO());
            _mapper.Setup(s => s.Map<SpecialtyResponseApiModel>(It.IsAny<SpecialtyDTO>())).Returns(new SpecialtyResponseApiModel());
            // Act
            var result = await _specialtyService.GetSpecialtyById("id");
            
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
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _specialtyService.GetSpecialtyById(null));
        }

        //not working
        [Fact]
        public async Task GetSpecialtyDescriptionsById_ShouldReturnSpecialtyDescriptions_ById()
        {
            // Arrange
            _specialtyToUniversityRepository.Setup(s => s.GetAll()).ReturnsAsync(new List<SpecialtyToUniversityDTO>());
            _mapper.Setup(s => s.Map<IEnumerable<SpecialtyToUniversityResponseApiModel>>(It.IsAny<SpecialtyToUniversityDTO>())).Returns(new List<SpecialtyToUniversityResponseApiModel>());

            // Act
            var result = await _specialtyService.GetAllSpecialtyDescriptionsById("id");

            // Assert
            Assert.IsType<SpecialtyToUniversityResponseApiModel>(result);
        }

        [Fact]
        public async Task GetSpecialtyDescriptionsById_ShouldReturnException_IfNotFoundSpecialtyDescriptions()
        {
            // Arrange
            _specialtyToUniversityRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync((SpecialtyToUniversityDTO)null);
            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _specialtyService.GetAllSpecialtyDescriptionsById(null));
        }

        [Fact]
        public async Task AddSpecialtyAndUniversityToFavorite_ShouldAddSpecialtyAndUniversityToFavorite_IfEverythingOk()
        {
            // Arrange  
            bool favorite = false;
            bool university = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => _specialtyService.AddSpecialtyAndUniversityToFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void AddSpecialtyAndUniversityToFavorite_ShouldThrowBadRequestException_IfSpecialtyAndUniversityHasAlreadyBeenAddedToFavorites()
        {
            // Arrange  
            bool favorite = false;
            bool university = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                 .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndUniversityToFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddSpecialtyAndUniversityToFavorite_ShouldThrowBadRequestException_IfUniversityNotFound()
        {
            bool favorite = true;
            bool university = false;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                 .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndUniversityToFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddSpecialtyAndUniversityToFavorite_ShouldThrowBadRequestException_IfSpecialtyNotFound()
        {
            bool favorite = true;
            bool university = true;
            bool specialty = false;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                 .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndUniversityToFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddSpecialtyAndUniversityToFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            bool favorite = true;
            bool university = true;
            bool specialty = true;
            var graduate = new GraduateDTO {};

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndUniversityToFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task DeleteUniversityFromFavorite_ShouldDeleteUniversityFromFavorite_IfEverythingOk()
        {
            bool favorite = true;
            bool university = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(It.IsAny<SpecialtyToUniversityToGraduate>()))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            var exception = await Record
               .ExceptionAsync(() => _specialtyService.DeleteSpecialtyAndUniversityFromFavorite(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DeleteSpecialtyAndUniversityFromFavorite_ShouldThrowBadRequestException_IfSpecialtyAndUniversityHasNotBeenAddedToFavorites()
        {
            bool favorite = true;
            bool university = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndUniversityFromFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteSpecialtyAndUniversityFromFavorite_ShouldThrowBadRequestException_IfUniversityNotFound()
        {
            bool favorite = true;
            bool university = false;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndUniversityFromFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteSpecialtyAndUniversityFromFavorite_ShouldThrowBadRequestException_IfSpecialtyNotFound()
        {
            bool favorite = true;
            bool university = true;
            bool specialty = false;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndUniversityFromFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteSpecialtyAndUniversityFromFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            bool favorite = true;
            bool university = true;
            bool specialty = false;
            var graduate = new GraduateDTO {};

            var entity = GetFavoriteSpecialtyAndUniversities();

            _specialtyToUniversityRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _universityRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(university);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToUniversityRepository
               .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToUniversityToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndUniversityFromFavorite(entity.SpecialtyId, entity.UniversityId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        private SpecialtyToUniversityToGraduate GetFavoriteSpecialtyAndUniversities()
        {
           return   new SpecialtyToUniversityToGraduate
                    {
                    SpecialtyId = "1",
                    UniversityId = "1",
                    GraduateId = "1"
                    };
        }

        [Fact]
        public void Dispose_ShouldDisposeRepositories()
        {
            // Arrange
            var specialtyToUniRepo = new Mock<ISpecialtyToUniversityRepository<SpecialtyToUniversity, SpecialtyToUniversityDTO>>();
            var educationFormToDescriptionRepository = new Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>>();
            var paymentFormToDescriptionRepository = new Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>>();
            var specialtyRepo = new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
            var universityRepository = new Mock<IUniversityRepository<University, UniversityDTO>>();
            var graduateRepository = new Mock<IGraduateRepository<Graduate, GraduateDTO>>();
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
                universityRepository.Object,
                graduateRepository.Object,
                _mapper.Object,
                _resourceManager.Object);
            service.Dispose();
            
            // Assert
            Assert.True(specToUniResult);
            Assert.True(specResult);
        }

    }
}
