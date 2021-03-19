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
        private readonly Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>> _specialtyToInstitutionOfEducationRepository = new Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>();
        private readonly Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>> _educationFormToDescriptionRepository = new Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>>();
        private readonly Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>> _paymentFormToDescriptionRepository = new Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>>();
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> _institutionOfEducationRepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
        private readonly Mock<IGraduateRepository<Graduate, GraduateDTO>> _graduateRepository = new Mock<IGraduateRepository<Graduate, GraduateDTO>>();
        private readonly Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>> _specialtyRepository = new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();

        private readonly SpecialtyDTO _specialtyDTO = new SpecialtyDTO { Id = "id", Direction = new DirectionDTO() };
        private readonly SpecialtyToInstitutionOfEducationDTO _specialtyToInstitutionOfEducationDTO = new SpecialtyToInstitutionOfEducationDTO { SpecialtyId = "id", InstitutionOfEducation = new InstitutionOfEducationDTO(), SpecialtyToIoEDescription = new SpecialtyToIoEDescriptionDTO() };
        private readonly IEnumerable<SpecialtyDTO> _listSpecialty;
        private readonly IEnumerable<SpecialtyToInstitutionOfEducationDTO> _listSpecialityToInstitutionOfEducationDTO;
        private readonly IEnumerable<SpecialtyDTO> _blankListSpecialty = new List<SpecialtyDTO>().AsEnumerable();
        private readonly IEnumerable<SpecialtyResponseApiModel> _blankResponse = new List<SpecialtyResponseApiModel>() { new SpecialtyResponseApiModel() }.AsEnumerable();

        public SpecialtyServiceTests()
        {
            _specialtyService = new SpecialtyService(
                _specialtyToInstitutionOfEducationRepository.Object,
                _educationFormToDescriptionRepository.Object,
                _paymentFormToDescriptionRepository.Object,
                _specialtyRepository.Object,
                _institutionOfEducationRepository.Object,
                _graduateRepository.Object,
                _mapper.Object,
                _resourceManager.Object
                );

            _listSpecialty = new List<SpecialtyDTO>() { _specialtyDTO }.AsEnumerable();
            _listSpecialityToInstitutionOfEducationDTO = new List<SpecialtyToInstitutionOfEducationDTO>() { _specialtyToInstitutionOfEducationDTO }.AsEnumerable();
            var listSpecialtyToInstitutionOfEducation = new List<SpecialtyToInstitutionOfEducationDTO>() { _specialtyToInstitutionOfEducationDTO }.AsEnumerable();
            _specialtyToInstitutionOfEducationRepository.Setup(s => s.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                    .ReturnsAsync(listSpecialtyToInstitutionOfEducation);
        }

        [Theory]
        [InlineData("Specialty", "Direction", "InstitutionOfEducation", "Abbreviation")]
        [InlineData("Specialty", null, "InstitutionOfEducation", null)]
        [InlineData(null, "Direction", null, "Abbreviation")]
        [InlineData(null, null, null, null)]
        public async Task GetAllSpecialtiesByFilter_ShouldReturnOk(string specialty, string direction, string uniName, string uniAbbr)
        {
            // Arrange
            var request = new FilterApiModel
            {
                SpecialtyName = _specialtyDTO.Name = specialty,
                DirectionName = _specialtyDTO.Direction.Name = direction,
                InstitutionOfEducationName = _specialtyToInstitutionOfEducationDTO.InstitutionOfEducation.Name = uniName,
                InstitutionOfEducationAbbreviation = _specialtyToInstitutionOfEducationDTO.InstitutionOfEducation.Abbreviation = uniAbbr
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
        [InlineData("Specialty", "Direction", "InstitutionOfEducation", "Abbreviation")]
        [InlineData("Specialty", null, "InstitutionOfEducation", null)]
        [InlineData("Specialty", null, null, null)]
        public async Task GetSpecialtiesNamesByFilter_ShouldReturnList_IfDatabaseNotEmpty2(string specialty, string direction, string uniName, string uniAbbr)
        {
            // Arrange
            var request = new FilterApiModel
            {
                SpecialtyName = _specialtyDTO.Name = specialty,
                DirectionName = _specialtyDTO.Direction.Name = direction,
                InstitutionOfEducationName = _specialtyToInstitutionOfEducationDTO.InstitutionOfEducation.Name = uniName,
                InstitutionOfEducationAbbreviation = _specialtyToInstitutionOfEducationDTO.InstitutionOfEducation.Abbreviation = uniAbbr
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
                InstitutionOfEducationName = _specialtyToInstitutionOfEducationDTO.InstitutionOfEducation.Name = null,
                InstitutionOfEducationAbbreviation = _specialtyToInstitutionOfEducationDTO.InstitutionOfEducation.Abbreviation = null
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
            _specialtyToInstitutionOfEducationRepository.Setup(s => s.GetSpecialtyToIoEDescriptionsById(It.IsAny<string>())).ReturnsAsync(_listSpecialityToInstitutionOfEducationDTO);

            // Act
            var result = await _specialtyService.GetAllSpecialtyDescriptionsById(It.IsAny<string>());

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetSpecialtyDescriptionsById_ShouldReturnException_IfNotFoundSpecialtyDescriptions()
        {
            // Arrange
            _specialtyToInstitutionOfEducationRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync((SpecialtyToInstitutionOfEducationDTO)null);
            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _specialtyService.GetAllSpecialtyDescriptionsById(null));
        }

        [Fact]
        public async Task AddSpecialtyAndInstitutionOfEducationToFavorite_ShouldAddSpecialtyAndInstitutionOfEducationToFavorite_IfEverythingOk()
        {
            // Arrange  
            bool favorite = false;
            bool institutionOfEducation = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => _specialtyService.AddSpecialtyAndInstitutionOfEducationToFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void AddSpecialtyAndInstitutionOfEducationToFavorite_ShouldThrowBadRequestException_IfSpecialtyAndInstitutionOfEducationHasAlreadyBeenAddedToFavorites()
        {
            // Arrange  
            bool favorite = false;
            bool institutionOfEducation = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                 .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndInstitutionOfEducationToFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddSpecialtyAndInstitutionOfEducationToFavorite_ShouldThrowBadRequestException_IfInstitutionOfEducationNotFound()
        {
            bool favorite = true;
            bool institutionOfEducation = false;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                 .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndInstitutionOfEducationToFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddSpecialtyAndInstitutionOfEducationToFavorite_ShouldThrowBadRequestException_IfSpecialtyNotFound()
        {
            bool favorite = true;
            bool institutionOfEducation = true;
            bool specialty = false;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                 .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndInstitutionOfEducationToFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddSpecialtyAndInstitutionOfEducationToFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            bool favorite = true;
            bool institutionOfEducation = true;
            bool specialty = true;
            var graduate = new GraduateDTO {};

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                .Setup(ur => ur.AddFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.AddSpecialtyAndInstitutionOfEducationToFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task DeleteInstitutionOfEducationFromFavorite_ShouldDeleteInstitutionOfEducationFromFavorite_IfEverythingOk()
        {
            bool favorite = true;
            bool institutionOfEducation = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            var exception = await Record
               .ExceptionAsync(() => _specialtyService.DeleteSpecialtyAndInstitutionOfEducationFromFavorite(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DeleteSpecialtyAndInstitutionOfEducationFromFavorite_ShouldThrowBadRequestException_IfSpecialtyAndInstitutionOfEducationHasNotBeenAddedToFavorites()
        {
            bool favorite = true;
            bool institutionOfEducation = true;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndInstitutionOfEducationFromFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteSpecialtyAndInstitutionOfEducationFromFavorite_ShouldThrowBadRequestException_IfInstitutionOfEducationNotFound()
        {
            bool favorite = true;
            bool institutionOfEducation = false;
            bool specialty = true;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndInstitutionOfEducationFromFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteSpecialtyAndInstitutionOfEducationFromFavorite_ShouldThrowBadRequestException_IfSpecialtyNotFound()
        {
            bool favorite = true;
            bool institutionOfEducation = true;
            bool specialty = false;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndInstitutionOfEducationFromFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteSpecialtyAndInstitutionOfEducationFromFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            bool favorite = true;
            bool institutionOfEducation = true;
            bool specialty = false;
            var graduate = new GraduateDTO {};

            var entity = GetFavoriteSpecialtyAndInstitutionOfEducations();

            _specialtyToInstitutionOfEducationRepository
                .Setup(su => su.FavoriteContains(entity))
                .ReturnsAsync(favorite);

            _institutionOfEducationRepository
                .Setup(ur => ur.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyRepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _specialtyToInstitutionOfEducationRepository
               .Setup(ur => ur.RemoveFavorite(It.IsAny<SpecialtyToInstitutionOfEducationToGraduate>()));

            // Act
            Func<Task> act = () => _specialtyService.DeleteSpecialtyAndInstitutionOfEducationFromFavorite(entity.SpecialtyId, entity.InstitutionOfEducationId, entity.GraduateId);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        private SpecialtyToInstitutionOfEducationToGraduate GetFavoriteSpecialtyAndInstitutionOfEducations()
        {
           return   new SpecialtyToInstitutionOfEducationToGraduate
                    {
                    SpecialtyId = "1",
                    InstitutionOfEducationId = "1",
                    GraduateId = "1"
                    };
        }

        [Fact]
        public void Dispose_ShouldDisposeRepositories()
        {
            // Arrange
            var specialtyToUniRepo = new Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>();
            var educationFormToDescriptionRepository = new Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>>();
            var paymentFormToDescriptionRepository = new Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>>();
            var specialtyRepo = new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
            var institutionOfEducationRepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
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
                institutionOfEducationRepository.Object,
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
