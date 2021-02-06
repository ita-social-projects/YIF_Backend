using AutoMapper;
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
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class UniversityServiceTests
    {
        private static readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private static readonly Mock<IUniversityRepository<University, UniversityDTO>> _universityReposotiry = new Mock<IUniversityRepository<University, UniversityDTO>>();
        private static readonly Mock<IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>> _specialityRepository = new Mock<IRepository<SpecialityToUniversity, SpecialityToUniversityDTO>>();
        private static readonly Mock<IGraduateRepository<Graduate, GraduateDTO>> _graduateRepository = new Mock<IGraduateRepository<Graduate, GraduateDTO>>();
        private static readonly Mock<IRepository<DirectionToUniversity, DirectionToUniversityDTO>> _directionRepository = new Mock<IRepository<DirectionToUniversity, DirectionToUniversityDTO>>();
        private static readonly Mock<IPaginationService> _paginationService = new Mock<IPaginationService>();

        private static readonly UniversityService universityService = new UniversityService(
            _universityReposotiry.Object,
            _specialityRepository.Object,
            _directionRepository.Object,
            _graduateRepository.Object,
            _mapperMock.Object,
            _paginationService.Object);

        // Tests for university filter
        [Fact]
        public async Task Get_UniversityByNameAndDirection()
        {
            // Arrange
            var university = new UniversityDTO
            {
                Id = "universityId",
                Name = "Name"
            };

            var direction = new DirectionDTO
            {
                Name = "Direction"
            };

            var apiModel = new FilterApiModel()
            {
                DirectionName = "Direction",
                SpecialityName = "",
                UniversityName = "Name"
            };

            var directionsList = new List<DirectionToUniversityDTO>
            {
                new DirectionToUniversityDTO
                {
                    Direction = direction,
                    DirectionId = "Direction",
                    UniversityId = "universityId",
                    University = university
                }
            };

            var universitiesList = new List<UniversityDTO>
            {
                university
            };

            _universityReposotiry.Setup(x => x.GetAll())
                .ReturnsAsync(universitiesList);

            _directionRepository.Setup(x => x.Find(It.IsAny<Expression<Func<DirectionToUniversity, bool>>>()))
                .ReturnsAsync(directionsList);

            _universityReposotiry.Setup(x => x.Find(x => x.Name == apiModel.UniversityName))
                .ReturnsAsync(universitiesList);

            _mapperMock.Setup(x => x.Map<IEnumerable<UniversityResponseApiModel>>(universitiesList))
                .Returns(new List<UniversityResponseApiModel>
                {
                    new UniversityResponseApiModel
                    {
                        Name = "Name"
                    }
                });

            // Act
            var results = await universityService.GetUniversitiesByFilter(apiModel);

            // Assert
            Assert.True(results.Count() > 0);
        }

        [Fact]
        public async Task Get_UniversityBySpeciality()
        {
            // Arrange
            var university = new UniversityDTO
            {
                Name = "Name",
                Id = ""
            };

            var speciality = new SpecialityDTO
            {
                Name = "Speciality"
            };

            var direction = new DirectionDTO
            {
                Name = "Direction"
            };

            var apiModel = new FilterApiModel()
            {
                DirectionName = "",
                SpecialityName = "Speciality",
                UniversityName = ""
            };

            var specialityList = new List<SpecialityToUniversityDTO>
            {
                new SpecialityToUniversityDTO
                {
                    Speciality = speciality,
                    SpecialityId = "",
                    UniversityId = "",
                    University = university
                }
            };

            var directionList = new List<DirectionToUniversityDTO>
            {
                new DirectionToUniversityDTO
                {
                    Direction = direction,
                    DirectionId = "",
                    UniversityId = "",
                    University = university
                }
            };

            var universitiesList = new List<UniversityDTO>
            {
                university
            };

            _universityReposotiry.Setup(x => x.GetAll())
                .ReturnsAsync(universitiesList);

            _specialityRepository.Setup(x => x.Find(It.IsAny<Expression<Func<SpecialityToUniversity, bool>>>()))
                .ReturnsAsync(specialityList);

            _directionRepository.Setup(x => x.Find(It.IsAny<Expression<Func<DirectionToUniversity, bool>>>()))
                .ReturnsAsync(directionList);

            _mapperMock.Setup(x => x.Map<IEnumerable<UniversityResponseApiModel>>(universitiesList))
                .Returns(new List<UniversityResponseApiModel>
                {
                    new UniversityResponseApiModel
                    {
                        Name = "Name"
                    }
                });

            // Act
            var results = await universityService.GetUniversitiesByFilter(apiModel);

            // Assert
            Assert.True(results.Count() > 0);
        }

        [Fact]
        public async Task GetUniversityById_ShouldReturnUniversityReponseModel_WhenUserIsFound()
        {
            // Arrange
            var universityDTO = new UniversityDTO
            {
                Id = "universityId",
                Name = "TestName"
            };

            var favorites = new List<UniversityDTO>
            {
                universityDTO
            };

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(universityDTO);

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favorites);

            _mapperMock
                .Setup(m => m.Map<UniversityResponseApiModel>(It.IsAny<UniversityDTO>()))
                .Returns(new UniversityResponseApiModel { Id = "universityId", Name = "TestName" });

            // Act
            var result = await universityService.GetUniversityById(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.IsType<UniversityResponseApiModel>(result);
        }

        [Fact]
        public void GetUniversityById_ShouldThrowNotFound_IfUniversityNotFound()
        {
            // Arrange
            _universityReposotiry.Setup(ur => ur.Get(It.IsAny<string>())).ReturnsAsync((UniversityDTO)null);

            // Act
            Func<Task<UniversityResponseApiModel>> act = () => universityService.GetUniversityById(It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetUniversitiesPage_ShouldReturnUniversityPage_IfEverythingOk()
        {
            // Arrange  
            var filterModel = new FilterApiModel();

            var universityList = GetUniversities();
            var favoriteUniversityList = GetFavoriteUniversities();
            var universityResponseList = GetResponseUnivesities();
            var universityPage = GetUniversityPage();

            _mapperMock
                .Setup(m => m.Map<IEnumerable<UniversityResponseApiModel>>(universityList))
                .Returns(universityResponseList);

            _paginationService
                .Setup(ps => ps.GetPageFromCollection(universityResponseList, It.IsAny<PageApiModel>()))
                .Returns(universityPage);

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityList);

            _universityReposotiry
                .Setup(ur => ur.GetAll())
                .ReturnsAsync(universityList);

            // Act
            var result = await universityService.GetUniversitiesPage(filterModel, It.IsAny<PageApiModel>());

            // Assert
            Assert.IsType<PageResponseApiModel<UniversityResponseApiModel>>(result);
        }

        [Fact]
        public void GetUniversitiesPage_ShouldReturnNotFoundException_IfUniversityNotFound()
        {
            // Arrange  
            var filterModel = new FilterApiModel();

            var universityList = new List<UniversityDTO>();
            var universityResponseList = new List<UniversityResponseApiModel>();

            _universityReposotiry
                .Setup(ur => ur.GetAll())
                .ReturnsAsync(universityList);

            _mapperMock
                .Setup(m => m.Map<IEnumerable<UniversityResponseApiModel>>(universityList))
                .Returns(universityResponseList);

            // Act
            Func<Task<PageResponseApiModel<UniversityResponseApiModel>>> act = () => universityService.GetUniversitiesPage(filterModel, It.IsAny<PageApiModel>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetFavoriteUniversities_ShouldReturnUniversityList_IfEverythingOk()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            var favoriteResponseList = GetResponseOfFavoriteUnivesities();

            _mapperMock
                .Setup(m => m.Map<IEnumerable<UniversityResponseApiModel>>(favoriteUniversityDTOList))
                .Returns(favoriteResponseList);

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            // Act
            var result = await universityService.GetFavoriteUniversities(It.IsAny<string>());

            // Assert
            Assert.IsType<List<UniversityResponseApiModel>>(result);
        }

        [Fact]
        public void GetFavoriteUniversities_ShouldReturnNotFoundException_IfFavoriteUniversitiesNotFound()
        {
            // Arrange  
            var favoriteUniversityDTOList = new List<UniversityDTO>();

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            // Act
            Func<Task<IEnumerable<UniversityResponseApiModel>>> act = () => universityService.GetFavoriteUniversities(It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task AddUniversityToFavorite_ShouldAddUniversityToFavorite_IfEverythngOk()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            var university = GetUniversities().ToList()[1];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _universityReposotiry
                .Setup(ur => ur.AddFavorite(It.IsAny<UniversityToGraduate>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => universityService.AddUniversityToFavorite(It.IsAny<string>(), It.IsAny<string>()));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void AddUniversityToFavorite_ShouldThrowBadRequestException_IfUniversityHasAlreadyBeenAddedToFavorites()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            // Get university which is in the collection of the favorites
            var university = GetUniversities().ToList()[0];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => universityService.AddUniversityToFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddUniversityToFavorite_ShouldThrowBadRequestException_IfUniversityNotFound()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            // University not found
            UniversityDTO university = null;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => universityService.AddUniversityToFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddUniversityToFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            var university = GetUniversities().ToList()[1];
            // Graduate not found
            GraduateDTO graduate = null;

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => universityService.AddUniversityToFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task DeleteUniversityFromFavorite_ShouldDeleteUniversityFromFavorite_IfEverythngOk()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            var university = GetUniversities().ToList()[0];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            _universityReposotiry
                .Setup(ur => ur.RemoveFavorite(It.IsAny<UniversityToGraduate>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => universityService.DeleteUniversityFromFavorite(university.Id, It.IsAny<string>()));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DeleteUniversityFromFavorite_ShouldThrowBadRequestException_IfUniversityHasNotBeenAddedToFavorites()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            // Get university which is not in the collection of favorites
            var university = GetUniversities().ToList()[1];
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => universityService.DeleteUniversityFromFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteUniversityFromFavorite_ShouldThrowBadRequestException_IfUniversityNotFound()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            // University not found
            UniversityDTO university = null;
            var graduate = new GraduateDTO { Id = "GraduateId" };

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => universityService.DeleteUniversityFromFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteUniversityFromFavorite_ShouldThrowBadRequestException_IfGraduateNotFound()
        {
            // Arrange  
            var favoriteUniversityDTOList = GetFavoriteUniversities();
            var university = GetUniversities().ToList()[1];
            // Graduate not found
            GraduateDTO graduate = null;

            _universityReposotiry
                .Setup(ur => ur.GetFavoritesByUserId(It.IsAny<string>()))
                .ReturnsAsync(favoriteUniversityDTOList);

            _universityReposotiry
                .Setup(ur => ur.Get(It.IsAny<string>()))
                .ReturnsAsync(university);

            _graduateRepository
                .Setup(gr => gr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(graduate);

            // Act
            Func<Task> act = () => universityService.DeleteUniversityFromFavorite(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        private IEnumerable<UniversityDTO> GetUniversities()
        {
            return new List<UniversityDTO>
            {
                new UniversityDTO
                {
                    Id = "1",
                    Name ="University 1"
                },
                new UniversityDTO
                {
                    Id = "2",
                    Name ="University 2"
                },
                new UniversityDTO
                {
                    Id = "3",
                    Name ="University 3"
                },
            };
        }

        private IEnumerable<UniversityDTO> GetFavoriteUniversities()
        {
            return new List<UniversityDTO>
            {
                new UniversityDTO
                {
                    Id = "1",
                    Name ="University 1"
                },
                new UniversityDTO
                {
                    Id = "3",
                    Name ="University 3"
                },
            };
        }

        private IEnumerable<UniversityResponseApiModel> GetResponseUnivesities()
        {
            return new List<UniversityResponseApiModel>
            {
                new UniversityResponseApiModel
                {
                    Id = "1",
                    Name = "University 1"
                },
                new UniversityResponseApiModel
                {
                    Id = "2",
                    Name = "University 2"
                },
                new UniversityResponseApiModel
                {
                    Id = "3",
                    Name = "University 3"
                }
            };
        }

        private IEnumerable<UniversityResponseApiModel> GetResponseOfFavoriteUnivesities()
        {
            return new List<UniversityResponseApiModel>
            {
                new UniversityResponseApiModel
                {
                    Id = "1",
                    Name ="University 1"
                },
                new UniversityResponseApiModel
                {
                    Id = "3",
                    Name ="University 3"
                },
            };
        }

        private PageResponseApiModel<UniversityResponseApiModel> GetUniversityPage()
        {
            return new PageResponseApiModel<UniversityResponseApiModel>
            {
                PageSize = 1,
                CurrentPage = 1,
                TotalPages = 1,
                ResponseList = GetResponseUnivesities()
            };
        }
    }
}
