using System;
using System.Resources;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Moq;
using SendGrid.Helpers.Errors.Model;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Service.Concrete.Services;
using AutoMapper;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class IoEModeratorServiceTests
    {
        private readonly IoEModeratorService _ioEModeratorService;
        private readonly Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>> _specialtyRepository = new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
        private readonly Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>
            _specialtyToIoERepository = new Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>();
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>
            _ioERepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
        private readonly Mock<ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>> 
            _specialtyToIoEDescriptionRepository = new Mock<ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>>();
        private readonly Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>>
            _ioEModeratorRepository = new Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>>();
        private readonly Mock<IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>> _examRequirementRepository = new Mock<IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>>();
        private readonly Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>>
            _ioEAdminRepository = new Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        public IoEModeratorServiceTests()
        {
            _ioEModeratorService = new IoEModeratorService(
                _specialtyRepository.Object,
                _ioERepository.Object,
                _specialtyToIoERepository.Object,
                _specialtyToIoEDescriptionRepository.Object,
                _ioEModeratorRepository.Object,
                _examRequirementRepository.Object,
                _mapper.Object,
                _resourceManager.Object,
                _ioEAdminRepository.Object
            );
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
                .ExceptionAsync(() => _ioEModeratorService.AddSpecialtyToIoe(entity));

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
            Task act() => _ioEModeratorService.AddSpecialtyToIoe(entity);

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
            Task act() => _ioEModeratorService.AddSpecialtyToIoe(entity);

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
                .ExceptionAsync(() => _ioEModeratorService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
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
            Task act() => _ioEModeratorService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
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
            Task act() => _ioEModeratorService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
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
            Task act() => _ioEModeratorService.DeleteSpecialtyToIoe(new SpecialtyToInstitutionOfEducationPostApiModel
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
            var result = await _ioEModeratorService.UpdateSpecialtyDescription(new SpecialtyDescriptionUpdateApiModel());

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetSpecialtyToIoEDescription_ShouldGetSpecialtyToIoEDescription_IfEverythingOk()
        {
            // Arrange  
            var specialty = true;
            var institutionOfEducation = true;
            var specialtyToIoe = new List<SpecialtyToInstitutionOfEducationDTO>
            { new SpecialtyToInstitutionOfEducationDTO{ Id = "SpecialtyToIoeId", InstitutionOfEducationId = "IoEId", SpecialtyId = "SpecialtyId" } };
            var moderator = new InstitutionOfEducationModeratorDTO { Id = "userId", Admin = new InstitutionOfEducationAdminDTO {InstitutionOfEducationId = "IoEId"}};

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _ioEModeratorRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(moderator);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            var exception = await Record
                .ExceptionAsync(() => _ioEModeratorService.GetSpecialtyToIoEDescription("userId", "IoEId"));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void GetSpecialtyToIoEDescription_ShouldThrowBadRequestException_IfInstitutionOfEducationNotFound()
        {
            // Arrange  
            var specialty = true;
            var specialtyToIoe = new List<SpecialtyToInstitutionOfEducationDTO>
            { new SpecialtyToInstitutionOfEducationDTO{ Id = "SpecialtyToIoeId", InstitutionOfEducationId = "IoEId", SpecialtyId = "SpecialtyId" } };
            var moderator = new InstitutionOfEducationModeratorDTO { Id = "userId", Admin = new InstitutionOfEducationAdminDTO { InstitutionOfEducationId = "IoEId" } };

            // InstitutionNotFound
            var institutionOfEducation = false;

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _ioEModeratorRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(moderator);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Task act() => _ioEModeratorService.GetSpecialtyToIoEDescription("userId", "IoEId");

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void GetSpecialtyToIoEDescription_ShouldThrowBadRequestException_IfSpecialtyNotFound()
        {
            // Arrange  
            //Specialty not found
            var specialty = false;

            var moderator = new InstitutionOfEducationModeratorDTO { Id = "userId", Admin = new InstitutionOfEducationAdminDTO { InstitutionOfEducationId = "IoEId" } };
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

            _ioEModeratorRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(moderator);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Task act() => _ioEModeratorService.GetSpecialtyToIoEDescription("userId", "IoEId");

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void GetSpecialtyToIoEDescription_ShouldThrowBadRequestException_IfSpecialtyInInstitutionOfEducationNotFound()
        {
            // Arrange  
            var specialty = true;
            var institutionOfEducation = true;
            var moderator = new InstitutionOfEducationModeratorDTO { Id = "userId", Admin = new InstitutionOfEducationAdminDTO { InstitutionOfEducationId = "IoEId" } };

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

            _ioEModeratorRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(moderator);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Task act() => _ioEModeratorService.GetSpecialtyToIoEDescription("userId", "IoEId");

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async void GetIoEAdminByUserId_ShouldReturnAdminData()
        {
            // Arrange  
            var ioEAdmin = new InstitutionOfEducationAdminDTO { InstitutionOfEducationId = "1" };
            var ioEModerator = new InstitutionOfEducationModeratorDTO { Admin = ioEAdmin };

            _ioEModeratorRepository.Setup(x => x.GetByUserId(It.IsAny<string>())).ReturnsAsync(ioEModerator);
            _mapper.Setup(x => x.Map<IoEAdminForIoEModeratorResponseApiModel>(It.IsAny<InstitutionOfEducationAdminDTO>()));

            // Act
            var result = await _ioEModeratorService.GetIoEAdminByUserId(It.IsAny<string>());

            // Assert  
            Assert.IsType<ResponseApiModel<IoEAdminForIoEModeratorResponseApiModel>>(result);
            Assert.True(result.Success);
        }


        [Fact]
        public async void GetIoEInfoByUserId_ShouldReturnIoE_IfEverythingIsOk()
        {
            // Arrange  
            var userId = "Id";
            _ioEModeratorRepository.Setup(x => x.GetByUserId(It.IsAny<string>())).ReturnsAsync(new InstitutionOfEducationModeratorDTO() {Admin = new InstitutionOfEducationAdminDTO() { InstitutionOfEducationId = "blahblah"}});
            _ioERepository.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new InstitutionOfEducationDTO());
            _mapper.Setup(x => x.Map<IoEInformationResponseApiModel>(It.IsAny<InstitutionOfEducationDTO>()));

            // Act
            var result = await _ioEModeratorService.GetIoEInfoByUserId(userId);

            // Assert  
            Assert.IsType<ResponseApiModel<IoEInformationResponseApiModel>>(result);
            Assert.True(result.Success);
        }
    }
}