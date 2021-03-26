using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class InstitutionOfEducationServiceTests
    {
        private static readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private static readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> _institutionOfEducationRepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
        private static readonly Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>> _educationFormToDescriptionRepository = new Mock<IRepository<EducationFormToDescription, EducationFormToDescriptionDTO>>();
        private static readonly Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>> _paymentFormToDescriptionRepository = new Mock<IRepository<PaymentFormToDescription, PaymentFormToDescriptionDTO>>();
        private static readonly Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>> _specialtyToInstitutionOfEducationRepository = new Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>();
        private static readonly Mock<IGraduateRepository<Graduate, GraduateDTO>> _graduateRepository = new Mock<IGraduateRepository<Graduate, GraduateDTO>>();
        private static readonly Mock<IDirectionRepository<Direction, DirectionDTO>> _directionRepository = new Mock<IDirectionRepository<Direction, DirectionDTO>>();
        private static readonly Mock<IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO>> _directionToIoERepository = new Mock<IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO>>();
        private static readonly Mock<IPaginationService> _paginationService = new Mock<IPaginationService>();
        private static readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        private static readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        private readonly Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

        private static readonly InstitutionOfEducationService institutionOfEducationService = new InstitutionOfEducationService(
            _institutionOfEducationRepository.Object,
            _directionToIoERepository.Object,
            _directionRepository.Object,
            _educationFormToDescriptionRepository.Object,
            _paymentFormToDescriptionRepository.Object,
            _specialtyToInstitutionOfEducationRepository.Object,
            _graduateRepository.Object,
            _mapperMock.Object,
            _paginationService.Object,
            _resourceManager.Object,
            _configuration.Object);

        public InstitutionOfEducationServiceTests()
        {
            httpRequest.Setup(x => x.Scheme).Returns("");
            httpRequest.Setup(x => x.Host).Returns(new HostString(""));
        }

        // Tests for institutionOfEducation filter
        [Fact]
        public async Task Get_InstitutionOfEducationByNameAndDirection()
        {
            // Arrange
            var institutionOfEducation = new InstitutionOfEducationDTO
            {
                Id = "institutionOfEducationId",
                Name = "Name"
            };

            var direction = new DirectionDTO
            {
                Name = "Direction"
            };

            var apiModel = new FilterApiModel()
            {
                DirectionName = "Direction",
                SpecialtyName = "",
                InstitutionOfEducationName = "Name"
            };

            var directionsList = new List<DirectionToInstitutionOfEducationDTO>
            {
                new DirectionToInstitutionOfEducationDTO
                {
                    Direction = direction,
                    DirectionId = "Direction",
                    InstitutionOfEducationId = "institutionOfEducationId",
                    InstitutionOfEducation = institutionOfEducation
                }
            };

            var institutionOfEducationsList = new List<InstitutionOfEducationDTO>
            {
                institutionOfEducation
            };

            _institutionOfEducationRepository.Setup(x => x.GetAll())
                .ReturnsAsync(institutionOfEducationsList);

            _directionToIoERepository.Setup(x => x.Find(It.IsAny<Expression<Func<DirectionToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(directionsList);

            _institutionOfEducationRepository.Setup(x => x.Find(x => x.Name == apiModel.InstitutionOfEducationName))
                .ReturnsAsync(institutionOfEducationsList);

            _mapperMock.Setup(x => x.Map<IEnumerable<InstitutionsOfEducationResponseApiModel>>(institutionOfEducationsList))
                .Returns(new List<InstitutionsOfEducationResponseApiModel>
                {
                    new InstitutionsOfEducationResponseApiModel
                    {
                        Name = "Name"
                    }
                });

            // Act
            var results = await institutionOfEducationService.GetInstitutionOfEducationsByFilter(apiModel);

            // Assert
            Assert.True(results.Count() > 0);
        }

        [Fact]
        public async Task Get_InstitutionOfEducationBySpeciality()
        {
            // Arrange
            var institutionOfEducation = new InstitutionOfEducationDTO
            {
                Name = "Name",
                Id = ""
            };

            var specialty = new SpecialtyDTO
            {
                Name = "Specialty"
            };

            var direction = new DirectionDTO
            {
                Name = "Direction"
            };

            var apiModel = new FilterApiModel()
            {
                DirectionName = "",
                SpecialtyName = "Specialty",
                InstitutionOfEducationName = ""
            };

            var specialtyList = new List<SpecialtyToInstitutionOfEducationDTO>
            {
                new SpecialtyToInstitutionOfEducationDTO
                {
                    Specialty = specialty,
                    SpecialtyId = "",
                    InstitutionOfEducationId = "",
                    InstitutionOfEducation = institutionOfEducation
                }
            };

            var directionList = new List<DirectionToInstitutionOfEducationDTO>
            {
                new DirectionToInstitutionOfEducationDTO
                {
                    Direction = direction,
                    DirectionId = "",
                    InstitutionOfEducationId = "",
                    InstitutionOfEducation = institutionOfEducation
                }
            };

            var institutionOfEducationsList = new List<InstitutionOfEducationDTO>
            {
                institutionOfEducation
            };

            _institutionOfEducationRepository.Setup(x => x.GetAll())
                .ReturnsAsync(institutionOfEducationsList);

            _specialtyToInstitutionOfEducationRepository.Setup(x => x.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyList);

            _directionToIoERepository.Setup(x => x.Find(It.IsAny<Expression<Func<DirectionToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(directionList);

            _mapperMock.Setup(x => x.Map<IEnumerable<InstitutionsOfEducationResponseApiModel>>(institutionOfEducationsList))
                .Returns(new List<InstitutionsOfEducationResponseApiModel>
                {
                    new InstitutionsOfEducationResponseApiModel
                    {
                        Name = "Name"
                    }
                });

            // Act
            var results = await institutionOfEducationService.GetInstitutionOfEducationsByFilter(apiModel);

            // Assert
            Assert.True(results.Count() > 0);
        }

        [Fact]
        public async Task GetInstitutionOfEducationById_ShouldReturnInstitutionOfEducationReponseModel_WhenUserIsFound()
        {
            // Arrange
            var institutionOfEducationDTO = new InstitutionOfEducationDTO();

            var directions = new List<DirectionDTO>();

            var favorites = new List<InstitutionOfEducationDTO>();

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducationDTO);

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favorites);

            _directionRepository
                .Setup(x => x.GetByIoEId(It.IsAny<string>()))
                .ReturnsAsync(directions);

            _mapperMock
                .Setup(m => m.Map<InstitutionOfEducationResponseApiModel>(It.IsAny<InstitutionOfEducationDTO>()))
                .Returns(new InstitutionOfEducationResponseApiModel());

            _mapperMock
                .Setup(x => x.Map<DirectionForIoEResponseApiModel>(It.IsAny<DirectionDTO>()))
                .Returns(new DirectionForIoEResponseApiModel());

            _configuration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(new Mock<IConfigurationSection>().Object);

            // Act
            var result = await institutionOfEducationService.GetInstitutionOfEducationById(It.IsAny<string>(), httpRequest.Object, It.IsAny<string>());

            // Assert
            Assert.IsType<InstitutionOfEducationResponseApiModel>(result);
        }

        [Fact]
        public void GetInstitutionOfEducationById_ShouldThrowNotFound_IfInstitutionOfEducationNotFound()
        {
            // Arrange
            _institutionOfEducationRepository.Setup(ur => ur.Get(It.IsAny<string>())).ReturnsAsync((InstitutionOfEducationDTO)null);

            // Act
            Func<Task<InstitutionOfEducationResponseApiModel>> act = () => institutionOfEducationService.GetInstitutionOfEducationById(It.IsAny<string>(), httpRequest.Object);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetInstitutionOfEducationsPage_ShouldReturnInstitutionOfEducationPage_IfEverythingOk()
        {
            // Arrange  
            var filterModel = new FilterApiModel();

            var institutionOfEducationList = GetInstitutionOfEducations();
            var favoriteInstitutionOfEducationList = GetFavoriteInstitutionOfEducations();
            var institutionOfEducationResponseList = GetResponseInstitutionOfEducations();
            var institutionOfEducationPage = GetInstitutionOfEducationPage();

            _mapperMock
                .Setup(m => m.Map<IEnumerable<InstitutionsOfEducationResponseApiModel>>(institutionOfEducationList))
                .Returns(institutionOfEducationResponseList);

            _paginationService
                .Setup(ps => ps.GetPageFromCollection(institutionOfEducationResponseList, It.IsAny<PageApiModel>()))
                .Returns(institutionOfEducationPage);

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationList);

            _institutionOfEducationRepository
                .Setup(ur => ur.GetAll())
                .ReturnsAsync(institutionOfEducationList);

            // Act
            var result = await institutionOfEducationService.GetInstitutionOfEducationsPage(filterModel, It.IsAny<PageApiModel>());

            // Assert
            Assert.IsType<PageResponseApiModel<InstitutionsOfEducationResponseApiModel>>(result);
        }

        [Fact]
        public void GetInstitutionOfEducationsPage_ShouldReturnNotFoundException_IfInstitutionOfEducationNotFound()
        {
            // Arrange  
            var filterModel = new FilterApiModel();

            var institutionOfEducationList = new List<InstitutionOfEducationDTO>();
            var institutionOfEducationResponseList = new List<InstitutionOfEducationResponseApiModel>();

            _institutionOfEducationRepository
                .Setup(ur => ur.GetAll())
                .ReturnsAsync(institutionOfEducationList);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<InstitutionOfEducationResponseApiModel>>(institutionOfEducationList))
                .Returns(institutionOfEducationResponseList);

            // Act
            Func<Task<PageResponseApiModel<InstitutionsOfEducationResponseApiModel>>> act = () => institutionOfEducationService.GetInstitutionOfEducationsPage(filterModel, It.IsAny<PageApiModel>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetFavoriteInstitutionOfEducations_ShouldReturnInstitutionOfEducationList_IfEverythingOk()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            var favoriteResponseList = GetResponseOfFavoriteInstitutionOfEducations();

            _mapperMock
                .Setup(m => m.Map<IEnumerable<InstitutionOfEducationResponseApiModel>>(favoriteInstitutionOfEducationDTOList))
                .Returns(favoriteResponseList);

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            // Act
            var result = await institutionOfEducationService.GetFavoriteInstitutionOfEducations(It.IsAny<string>());

            // Assert
            Assert.IsType<List<InstitutionOfEducationResponseApiModel>>(result);
        }

        [Fact]
        public void GetFavoriteInstitutionOfEducations_ShouldReturnNotFoundException_IfFavoriteInstitutionOfEducationsNotFound()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = new List<InstitutionOfEducationDTO>();

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            // Act
            Func<Task<IEnumerable<InstitutionOfEducationResponseApiModel>>> act = () => institutionOfEducationService.GetFavoriteInstitutionOfEducations(It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetInstitutionOfEducationAbbreviations_ShouldReturnListOfAbbreviations_IfEverythingOk()
        {
            // Arrange  
            var filterModel = new FilterApiModel();

            var institutionOfEducationList = GetInstitutionOfEducations();
            var institutionOfEducationResponseList = GetResponseInstitutionOfEducations();

            _mapperMock
                .Setup(m => m.Map<IEnumerable<InstitutionsOfEducationResponseApiModel>>(institutionOfEducationList))
                .Returns(institutionOfEducationResponseList);

            _institutionOfEducationRepository
                .Setup(ur => ur.GetAll())
                .ReturnsAsync(institutionOfEducationList);

            // Act
            var result = await institutionOfEducationService.GetInstitutionOfEducationAbbreviations(filterModel);

            // Assert
            Assert.True(result is IEnumerable<string>);
        }

        [Fact]
        public void GetInstitutionOfEducationAbbreviations_ShouldReturnNotFoundException_IfInstitutionOfEducationNotFound()
        {
            // Arrange  
            var filterModel = new FilterApiModel();

            var institutionOfEducationList = GetInstitutionOfEducations();
            var institutionOfEducationResponseList = new List<InstitutionOfEducationResponseApiModel>();

            _mapperMock
                .Setup(m => m.Map<IEnumerable<InstitutionOfEducationResponseApiModel>>(institutionOfEducationList))
                .Returns(institutionOfEducationResponseList);

            _institutionOfEducationRepository
                .Setup(ur => ur.GetAll())
                .ReturnsAsync(institutionOfEducationList);

            // Act
            Func<Task<IEnumerable<string>>> act = () => institutionOfEducationService.GetInstitutionOfEducationAbbreviations(filterModel);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task AddInstitutionOfEducationToFavorite_ShouldAddInstitutionOfEducationToFavorite_IfEverythingOk()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            var institutionOfEducation = GetInstitutionOfEducations().ToList()[1];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _institutionOfEducationRepository
                .Setup(ur => ur.AddFavorite(It.IsAny<InstitutionOfEducationToGraduate>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => institutionOfEducationService.AddInstitutionOfEducationToFavorite(It.IsAny<string>(), It.IsAny<string>()));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void AddInstitutionOfEducationToFavorite_ShouldThrowBadRequestException_IfInstitutionOfEducationHasAlreadyBeenAddedToFavorites()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            // Get institutionOfEducation which is in the collection of the favorites
            var institutionOfEducation = GetInstitutionOfEducations().ToList()[0];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => institutionOfEducationService.AddInstitutionOfEducationToFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddInstitutionOfEducationToFavorite_ShouldThrowBadRequestException_IfInstitutionOfEducationNotFound()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            // InstitutionOfEducation not found
            InstitutionOfEducationDTO institutionOfEducation = null;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => institutionOfEducationService.AddInstitutionOfEducationToFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddInstitutionOfEducationToFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            var institutionOfEducation = GetInstitutionOfEducations().ToList()[1];
            // Graduate not found
            GraduateDTO graduate = null;

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => institutionOfEducationService.AddInstitutionOfEducationToFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task DeleteInstitutionOfEducationFromFavorite_ShouldDeleteInstitutionOfEducationFromFavorite_IfEverythingOk()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            var institutionOfEducation = GetInstitutionOfEducations().ToList()[0];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _institutionOfEducationRepository
                .Setup(ur => ur.RemoveFavorite(It.IsAny<InstitutionOfEducationToGraduate>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => institutionOfEducationService.DeleteInstitutionOfEducationFromFavorite(institutionOfEducation.Id, It.IsAny<string>()));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DeleteInstitutionOfEducationFromFavorite_ShouldThrowBadRequestException_IfInstitutionOfEducationHasNotBeenAddedToFavorites()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            // Get institutionOfEducation which is not in the collection of favorites
            var institutionOfEducation = GetInstitutionOfEducations().ToList()[1];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => institutionOfEducationService.DeleteInstitutionOfEducationFromFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteInstitutionOfEducationFromFavorite_ShouldThrowBadRequestException_IfInstitutionOfEducationNotFound()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            // InstitutionOfEducation not found
            InstitutionOfEducationDTO institutionOfEducation = null;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => institutionOfEducationService.DeleteInstitutionOfEducationFromFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteInstitutionOfEducationFromFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            // Arrange  
            var favoriteInstitutionOfEducationDTOList = GetFavoriteInstitutionOfEducations();
            var institutionOfEducation = GetInstitutionOfEducations().ToList()[1];
            // Graduate not found
            GraduateDTO graduate = null;

            _institutionOfEducationRepository
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteInstitutionOfEducationDTOList);

            _institutionOfEducationRepository
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => institutionOfEducationService.DeleteInstitutionOfEducationFromFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        private IEnumerable<InstitutionOfEducationDTO> GetInstitutionOfEducations()
        {
            return new List<InstitutionOfEducationDTO>
            {
                new InstitutionOfEducationDTO
                {
                    Id = "1",
                    Name ="InstitutionOfEducation 1"
                },
                new InstitutionOfEducationDTO
                {
                    Id = "2",
                    Name ="InstitutionOfEducation 2"
                },
                new InstitutionOfEducationDTO
                {
                    Id = "3",
                    Name ="InstitutionOfEducation 3"
                },
            };
        }

        private IEnumerable<InstitutionOfEducationDTO> GetFavoriteInstitutionOfEducations()
        {
            return new List<InstitutionOfEducationDTO>
            {
                new InstitutionOfEducationDTO
                {
                    Id = "1",
                    Name ="InstitutionOfEducation 1"
                },
                new InstitutionOfEducationDTO
                {
                    Id = "3",
                    Name ="InstitutionOfEducation 3"
                },
            };
        }

        private IEnumerable<InstitutionsOfEducationResponseApiModel> GetResponseInstitutionOfEducations()
        {
            return new List<InstitutionsOfEducationResponseApiModel>
            {
                new InstitutionsOfEducationResponseApiModel
                {
                    Id = "1",
                    Name = "InstitutionOfEducation 1"
                },
                new InstitutionsOfEducationResponseApiModel
                {
                    Id = "2",
                    Name = "InstitutionOfEducation 2"
                },
                new InstitutionsOfEducationResponseApiModel
                {
                    Id = "3",
                    Name = "InstitutionOfEducation 3"
                }
            };
        }

        private IEnumerable<InstitutionOfEducationResponseApiModel> GetResponseOfFavoriteInstitutionOfEducations()
        {
            return new List<InstitutionOfEducationResponseApiModel>
            {
                new InstitutionOfEducationResponseApiModel
                {
                    Id = "1",
                    Name ="InstitutionOfEducation 1"
                },
                new InstitutionOfEducationResponseApiModel
                {
                    Id = "3",
                    Name ="InstitutionOfEducation 3"
                },
            };
        }

        private PageResponseApiModel<InstitutionsOfEducationResponseApiModel> GetInstitutionOfEducationPage()
        {
            return new PageResponseApiModel<InstitutionsOfEducationResponseApiModel>
            {
                PageSize = 1,
                CurrentPage = 1,
                TotalPages = 1,
                ResponseList = GetResponseInstitutionOfEducations()
            };
        }
    }
}
