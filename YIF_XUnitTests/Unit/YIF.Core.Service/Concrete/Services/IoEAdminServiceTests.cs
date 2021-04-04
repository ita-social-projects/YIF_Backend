using System;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Moq;
using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Service.Concrete.Services;
using YIF_XUnitTests.Unit.TestData;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class IoEAdminServiceTests
    {
        private readonly IoEAdminService _ioEAdminService;
        private readonly Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>> _specialtyRepository= new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
        private readonly Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO>> _ioEAdminRepository =
            new Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO>>();
        private readonly Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>> 
            _specialtyToIoERepository = new Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>();
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> 
            _ioERepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
        private readonly Mock<ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>> _specialtyToIoEDescriptionRepository = new Mock<ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>>();
        private readonly Mock<IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>> _examRequirementRepository = new Mock<IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IWebHostEnvironment> _env = new Mock<IWebHostEnvironment>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        public IoEAdminServiceTests()
        {
            _ioEAdminService = new IoEAdminService(
                _specialtyRepository.Object,
                _ioERepository.Object,
                _specialtyToIoERepository.Object,
                _ioEAdminRepository.Object,
                _specialtyToIoEDescriptionRepository.Object,
                _examRequirementRepository.Object,
                _mapper.Object,
                _env.Object,
                _configuration.Object,
                _resourceManager.Object
            );
        }

        [Fact]
        public void ModifyDescriptionOfInstitution_WrongAdminId()
        {
            // Arrange
            var wrongAdminId = "0";
            var listOfAdmins = InstitutionOfEducationAdminTestData.GetIEnumerableInstitutionOfEducationAdminDTO();
            _ioEAdminRepository.Setup(x => x.GetAllUniAdmins())
                .Returns(Task.FromResult(listOfAdmins));

            // Act
            var result = _ioEAdminService.ModifyDescriptionOfInstitution(wrongAdminId, new InstitutionOfEducationPostApiModel());

            // Assert
            Assert.False(result.Result.Success);
        }

        [Fact]
        public void ModifyDescriptionOfInstitution_ReturnTrue()
        {
            // Arrange
            var listOfAdmins = InstitutionOfEducationAdminTestData.GetIEnumerableInstitutionOfEducationAdminDTO();
            var institutionDTO = InstitutionOfEducationTestData.GetInstitutionOfEducationDTO();
            var institution = InstitutionOfEducationTestData.GetInstitutionOfEducation();

            _ioEAdminRepository.Setup(x => x.GetAllUniAdmins())
                .Returns(Task.FromResult(listOfAdmins));
            _mapper.Setup(x => x.Map<InstitutionOfEducationDTO>(It.IsAny<InstitutionOfEducationPostApiModel>()))
                .Returns(institutionDTO);
            _mapper.Setup(x => x.Map<InstitutionOfEducation>(It.IsAny<InstitutionOfEducationDTO>()))
                .Returns(institution);
            _ioERepository.Setup(x => x.Update(It.IsAny<InstitutionOfEducation>()))
                .Returns(Task.FromResult(true));
            _resourceManager.Setup(x => x.GetString(It.IsAny<string>()))
                .Returns("");

            // Act
            var result = _ioEAdminService.ModifyDescriptionOfInstitution(listOfAdmins.FirstOrDefault().Id, new InstitutionOfEducationPostApiModel());

            // Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public async Task AddSpecialtyToIoE_ShouldAddSpecialty()
        {
            //Arrange
            var specialty = true;
            var institution = true;
            var entity = new SpecialtyToInstitutionOfEducationPostApiModel
            {
                SpecialtyId = "1",
                InstitutionOfEducationId = "1"
            };
            _specialtyRepository.Setup(x => x.ContainsById(It.IsAny<string>())).ReturnsAsync(specialty);
            _ioERepository.Setup(x => x.ContainsById(It.IsAny<string>())).ReturnsAsync(institution);
            _specialtyToIoERepository.Setup(x => x.AddSpecialty(It.IsAny<SpecialtyToInstitutionOfEducation>()));
            // Act
            var exception = await Record
                .ExceptionAsync(() => _ioEAdminService.AddSpecialtyToIoe(entity));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void AddSpecialtyToIoE_ShouldReturnBadRequestException_IfSpecialtyNotFound()
        {
            //Arrange
            var specialty = false;
            var institution = true;
            var entity = new SpecialtyToInstitutionOfEducationPostApiModel
            {
                SpecialtyId = "1",
                InstitutionOfEducationId = "1"
            };
            _specialtyRepository.Setup(x => x.ContainsById(It.IsAny<string>())).ReturnsAsync(specialty);
            _ioERepository.Setup(x => x.ContainsById(It.IsAny<string>())).ReturnsAsync(institution);
            _specialtyToIoERepository.Setup(x => x.AddSpecialty(It.IsAny<SpecialtyToInstitutionOfEducation>()));
            // Act
            Func<Task> act = () => _ioEAdminService.AddSpecialtyToIoe(entity);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void AddSpecialtyToIoE_ShouldReturnBadRequestException_IfIoENotFound()
        {
            //Arrange
            var specialty = true;
            var institution = false;
            var entity = new SpecialtyToInstitutionOfEducationPostApiModel
            {
                SpecialtyId = "1",
                InstitutionOfEducationId = "1"
            };
            _specialtyRepository.Setup(x => x.ContainsById(It.IsAny<string>())).ReturnsAsync(specialty);
            _ioERepository.Setup(x => x.ContainsById(It.IsAny<string>())).ReturnsAsync(institution);
            _specialtyToIoERepository.Setup(x => x.AddSpecialty(It.IsAny<SpecialtyToInstitutionOfEducation>()));
            // Act
            Func<Task> act = () => _ioEAdminService.AddSpecialtyToIoe(entity);

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task DeleteSpecialtyFromInstitutionOfEducation_ShouldDeleteSpecialtyFromInstitutionOfEducation_IfEverythingOk()
        {
            // Arrange  
            var specialty = true;
            var institutionOfEducation = true;
            var specialtyToIoe = new List<SpecialtyToInstitutionOfEducationDTO>
            { new SpecialtyToInstitutionOfEducationDTO{ Id = "SpecialtyToIoeId", InstitutionOfEducationId = "IoEId", SpecialtyId = "SpecialtyId" } };

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => _ioEAdminService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
                {
                    InstitutionOfEducationId = "IoEId",
                    SpecialtyId = "SpecialtyId"
                }));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void DeleteSpecialtyFromInstitutionOfEducation_ShouldThrowBadRequestException_IfInstitutionOfEducationNotFound()
        {
            // Arrange  
            var specialty = true;
            // InstitutionNotFound
            var institutionOfEducation = false;
            var specialtyToIoe = new List<SpecialtyToInstitutionOfEducationDTO>
            { new SpecialtyToInstitutionOfEducationDTO{ Id = "SpecialtyToIoeId", InstitutionOfEducationId = "IoEId", SpecialtyId = "SpecialtyId" } };

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Func<Task> act = () => _ioEAdminService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
            {
                InstitutionOfEducationId = "IoEId",
                SpecialtyId = "SpecialtyId"
            });

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void DeleteSpecialtyFromInstitutionOfEducation_ShouldThrowBadRequestException_IfSpecialtyNotFound()
        {
            // Arrange  
            //Specialty not found
            var specialty = false;

            var institutionOfEducation = true;
            var specialtyToIoe = new List<SpecialtyToInstitutionOfEducationDTO>
            { new SpecialtyToInstitutionOfEducationDTO{ Id = "SpecialtyToIoeId", InstitutionOfEducationId = "IoEId", SpecialtyId = "SpecialtyId" } };

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Func<Task> act = () => _ioEAdminService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
            {
                InstitutionOfEducationId = "IoEId",
                SpecialtyId = "SpecialtyId"
            });

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }
        [Fact]
        public void DeleteSpecialtyFromInstitutionOfEducation_ShouldThrowBadRequestException_IfSpecialtyInInstitutionOfEducationNotFound()
        {
            // Arrange  
            var specialty = true;
            var institutionOfEducation = true;
            //SpecialtyToInstitutionOfEducation not found
            List<SpecialtyToInstitutionOfEducationDTO> specialtyToIoe = null;

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Func<Task> act = () => _ioEAdminService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
            {
                InstitutionOfEducationId = "IoEId",
                SpecialtyId = "SpecialtyId"
            });

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }
        [Fact]
        public async Task UpdateSpecialtyDescription_ShouldUpdateDescriptionAndReturnCorrectMessage_IfEverythingIsOk()
        {
            //Arrange
            _mapper.Setup(sr => sr.Map<SpecialtyToIoEDescriptionDTO>(It.IsAny<SpecialtyDescriptionUpdateApiModel>())).Returns(It.IsAny<SpecialtyToIoEDescriptionDTO>());
            _examRequirementRepository.Setup(sr => sr.DeleteRangeByDescriptionId(It.IsAny<string>()));
            _mapper.Setup(sr => sr.Map<SpecialtyToIoEDescription>(It.IsAny<SpecialtyToIoEDescriptionDTO>())).Returns(It.IsAny<SpecialtyToIoEDescription>());
            _specialtyToIoEDescriptionRepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToIoEDescription>())).ReturnsAsync(true);

            //Act
            var result = await _ioEAdminService.UpdateSpecialtyDescription(new SpecialtyDescriptionUpdateApiModel());

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }
    }
}
