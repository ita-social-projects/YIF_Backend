﻿using AutoMapper;
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
    public class DirectionServiceTests
    {
        private readonly IDirectionService _directionService;
        private readonly Mock<IRepository<Direction, DirectionDTO>> _repositoryDirection;
        private readonly Mock<IRepository<Speciality, SpecialityDTO>> _specialtyRepository;
        private readonly Mock<IRepository<DirectionToUniversity, DirectionToUniversityDTO>> _directionToUniversityRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IPaginationService> _paginationService;

        public DirectionServiceTests()
        {
            _repositoryDirection = new Mock<IRepository<Direction, DirectionDTO>>();
            _specialtyRepository = new Mock<IRepository<Speciality, SpecialityDTO>>();
            _directionToUniversityRepository = new Mock<IRepository<DirectionToUniversity, DirectionToUniversityDTO>>();
            _mapper = new Mock<IMapper>();
            _paginationService = new Mock<IPaginationService>();

            _directionService = new DirectionService(
                _repositoryDirection.Object,
                _specialtyRepository.Object,
                _directionToUniversityRepository.Object,
                _mapper.Object,
                _paginationService.Object);
                        
        }

        [Theory]
        [InlineData(null, null, null, null)]
        [InlineData("", "", "", "")]
        [InlineData("Direction 1", null, null, null)]
        [InlineData(null, "Specialty 1", null, null)]
        [InlineData(null, null, "University name 1", null)]
        [InlineData(null, null, null, "University abbreviation 1")]
        public async Task GetAllDirectionsByFilter_ShouldReturnDirections_IfEverythingOk(
            string directionName, 
            string specialityName,
            string universityName,
            string universityAbbreviation)
        {
            // Arrange
            var filterModel = new FilterApiModel
            {
                DirectionName = directionName,
                SpecialityName = specialityName,
                UniversityName = universityName,
                UniversityAbbreviation = universityAbbreviation
            };

            var responseList = GetResponseList();
            var directions = GetDirectionDTOs();
            var specialties = GetSpecialityDTOs();
            var directionsToUniversities = GetDirectionToUniversityDTOs()
                .Where(du => filterModel.UniversityName == null ||
                             filterModel.UniversityName == string.Empty ||
                             filterModel.UniversityName == du.University.Name)
                .Where(du => filterModel.UniversityAbbreviation == null ||
                             filterModel.UniversityAbbreviation == string.Empty ||
                             filterModel.UniversityAbbreviation == du.University.Abbreviation);

            _repositoryDirection
                .Setup(rd => rd.GetAll())
                .ReturnsAsync(directions);

            _specialtyRepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<Speciality, bool>>>()))
                .ReturnsAsync(GetSpecialityDTOs().Where(s => s.Name == filterModel.SpecialityName));

            _directionToUniversityRepository
                .Setup(du => du.Find(It.IsAny<Expression<Func<DirectionToUniversity, bool>>>()))
                .ReturnsAsync(directionsToUniversities);

            _mapper
                .Setup(m => m.Map<IEnumerable<DirectionResponseApiModel>>(It.IsAny<object>()))
                .Returns(responseList);

            // Act
            var result = await _directionService.GetAllDirectionsByFilter(filterModel);

            // Assert
            Assert.IsType<List<DirectionResponseApiModel>>(result);
        }

        [Fact]
        public async Task GetDirectionsNamesByFilter_ShouldReturnDirectionNames_IfEverythingOk()
        {
            // Arrange
            var filterModel = new FilterApiModel() { };
            var responseList = GetResponseList();

            _mapper
                .Setup(m => m.Map<IEnumerable<DirectionResponseApiModel>>(It.IsAny<object>()))
                .Returns(responseList);

            // Act
            var result = await _directionService.GetDirectionsNamesByFilter(filterModel);

            // Assert
            Assert.True(result is IEnumerable<string>);
        }

        [Fact]
        public void GetDirectionsNamesByFilter_ShouldReturnNotFoundException_IfDirectionsNotFound()
        {
            // Arrange
            var filterModel = new FilterApiModel() { };
            var responseList = new List<DirectionResponseApiModel>();
            
            _mapper
                .Setup(m => m.Map<IEnumerable<DirectionResponseApiModel>>(It.IsAny<object>()))
                .Returns(responseList);

            // Act
            Func<Task<IEnumerable<string>>> act = () => _directionService.GetDirectionsNamesByFilter(filterModel);

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        private IEnumerable<DirectionDTO> GetDirectionDTOs()
        {
            var directionDTOs = new List<DirectionDTO>();
            var numOfDirections = 3;
            var numOfSpecialties = 3;

            for (int i = 0; i < numOfDirections; i++)
            {
                directionDTOs.Add(new DirectionDTO
                {
                    Id = (i + 1).ToString(),
                    Name = $"Direction {i + 1}"
                });

                var specialtyDTOs = new List<SpecialityDTO>();
                for (int j = 0; j < numOfSpecialties; j++)
                {
                    specialtyDTOs.Add(new SpecialityDTO
                    {
                        Id = (j + 1 + i * numOfSpecialties).ToString(),
                        Name = $"Specialty {j + 1 + i * numOfSpecialties}",
                        Direction = directionDTOs[i],
                        DirectionId = directionDTOs[i].Id
                    });                    
                }

                directionDTOs[i].Specialities = specialtyDTOs;
            }

            return directionDTOs;
        }

        private IEnumerable<SpecialityDTO> GetSpecialityDTOs()
        {
            return GetDirectionDTOs()
                .SelectMany(x=>x.Specialities);
        }

        private IEnumerable<DirectionToUniversityDTO> GetDirectionToUniversityDTOs()
        {
            var directions = GetDirectionDTOs().ToList();
            var result = new List<DirectionToUniversityDTO>();
            var numOfUniversities = 3;

            for (int i = 0; i < numOfUniversities; i++)
            {
                for (int j = 0; j < directions.Count; j++)
                {
                    result.Add(new DirectionToUniversityDTO
                    {
                        UniversityId = (i + 1).ToString(),
                        University = new UniversityDTO
                        {
                            Name = $"University name {i + 1}",
                            Abbreviation = $"University abbreviation {i + 1}",
                        },
                        DirectionId = directions[j].Id,
                        Direction = directions[j]
                    });
                }
            }

            return result;
        }
        private IEnumerable<DirectionResponseApiModel> GetResponseList()
        {
            var result = new List<DirectionResponseApiModel>();
            foreach (var item in GetDirectionDTOs())
            {
                result.Add(new DirectionResponseApiModel
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return result;
        }
    }
}