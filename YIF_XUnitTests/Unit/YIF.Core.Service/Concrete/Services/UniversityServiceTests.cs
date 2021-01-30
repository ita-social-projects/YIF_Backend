﻿using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
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

        [Fact]
        public async Task Get_UniversityByNameAndDirection()
        {
            // Arrange
            var university = new UniversityDTO
            {
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
                    UniversityId = "university",
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
    }
}
