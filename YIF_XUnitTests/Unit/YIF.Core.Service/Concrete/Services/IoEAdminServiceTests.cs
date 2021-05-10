using System;
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
using Microsoft.AspNetCore.JsonPatch;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.DtoModels.IdentityDTO;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class IoEAdminServiceTests
    {
        private readonly Mock<FakeUserManager<DbUser>> _userManager = new Mock<FakeUserManager<DbUser>>();
        private readonly Mock<IUserRepository<DbUser, UserDTO>> _userRepository = new Mock<IUserRepository<DbUser, UserDTO>>();
        private readonly IoEAdminService _ioEAdminService;
        private readonly Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>> _specialtyRepository= new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
        private readonly Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>> _ioEAdminRepository =
            new Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>>();
        private readonly Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>> 
            _specialtyToIoERepository = new Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>();
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> 
            _ioERepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
        private readonly Mock<ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>> _specialtyToIoEDescriptionRepository = new Mock<ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO>>();
        private readonly Mock<IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>> _examRequirementRepository = new Mock<IExamRequirementRepository<ExamRequirement, ExamRequirementDTO>>();
        private readonly Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>> _ioEModeratorRepository = new Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IWebHostEnvironment> _env = new Mock<IWebHostEnvironment>();
        private readonly Mock<IConfiguration> _configuration = new Mock<IConfiguration>();

        public IoEAdminServiceTests()
        {
            _ioEAdminService = new IoEAdminService(
                _userManager.Object,
                _userRepository.Object,
                _specialtyRepository.Object,
                _ioERepository.Object,
                _specialtyToIoERepository.Object,
                _ioEAdminRepository.Object,
                _specialtyToIoEDescriptionRepository.Object,
                _examRequirementRepository.Object,
                _ioEModeratorRepository.Object,
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
            Func<Task> act = () => _ioEAdminService.ModifyDescriptionOfInstitution(wrongAdminId, new JsonPatchDocument<InstitutionOfEducationPostApiModel>());

            // Assert
            Assert.ThrowsAsync<NullReferenceException>(act);
        }

        [Fact]
        public void ModifyDescriptionOfInstitution_ReturnTrue()
        {
            // Arrange
            var institutionOfEducationAdminDTO = new InstitutionOfEducationAdminDTO()
            {
                InstitutionOfEducationId = "id"
            };

            _ioEAdminRepository.Setup(x => x.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducationAdminDTO);

            _ioERepository.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new InstitutionOfEducationDTO());

            _mapper.Setup(x => x.Map<JsonPatchDocument<InstitutionOfEducationDTO>>(It.IsAny<JsonPatchDocument<InstitutionOfEducationPostApiModel>>()))
                .Returns(new JsonPatchDocument<InstitutionOfEducationDTO>());

            _mapper.Setup(x => x.Map<InstitutionOfEducation>(It.IsAny<InstitutionOfEducationDTO>()))
                .Returns(It.IsAny<InstitutionOfEducation>());

            _ioERepository.Setup(x => x.Update(It.IsAny<InstitutionOfEducation>()))
                .Returns(Task.FromResult(true));

            _resourceManager.Setup(x => x.GetString(It.IsAny<string>()))
                .Returns("");

            // Act
            var result = _ioEAdminService.ModifyDescriptionOfInstitution(It.IsAny<string>(), new JsonPatchDocument<InstitutionOfEducationPostApiModel>());

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

            _mapper.Setup(x => x.Map<SpecialtyToInstitutionOfEducationDTO>(It.IsAny<SpecialtyToInstitutionOfEducationPostApiModel>())).Returns(It.IsAny<SpecialtyToInstitutionOfEducationDTO>());
            _mapper.Setup(x => x.Map<SpecialtyToInstitutionOfEducation>(It.IsAny<SpecialtyToInstitutionOfEducationDTO>())).Returns(new SpecialtyToInstitutionOfEducation());

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

        [Fact]
        public async void GetIoEModeratorsByUserId_ShouldReturnListOfModerators_IfEverythingIsOk()
        {
            // Arrange  
            _ioEAdminRepository.Setup(x => x.GetByUserId(It.IsAny<string>())).ReturnsAsync(new InstitutionOfEducationAdminDTO());
            _ioEModeratorRepository.Setup(x => x.GetByIoEId(It.IsAny<string>())).ReturnsAsync(It.IsAny<IEnumerable<InstitutionOfEducationModeratorDTO>>);
            _mapper.Setup(x => x.Map<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>(It.IsAny<IEnumerable<InstitutionOfEducationModeratorDTO>>()));

            // Act
            var result = await _ioEAdminService.GetIoEModeratorsByUserId(It.IsAny<string>());

            // Assert  
            Assert.IsType<ResponseApiModel<IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async void GetIoEInfoByUserId_ShouldReturnIoE_IfEverythingIsOk()
        {
            // Arrange  
            _ioEAdminRepository.Setup(x => x.GetByUserId(It.IsAny<string>())).ReturnsAsync(new InstitutionOfEducationAdminDTO());
            _mapper.Setup(x => x.Map<IoEInformationResponseApiModel>(It.IsAny<InstitutionOfEducationDTO>()));

            // Act
            var result = await _ioEAdminService.GetIoEInfoByUserId(It.IsAny<string>());

            // Assert  
            Assert.IsType<ResponseApiModel<IoEInformationResponseApiModel>>(result);
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
            var admin = new InstitutionOfEducationAdminDTO { Id = "userId", InstitutionOfEducationId = "IoEId" };

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _ioEAdminRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(admin);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            _mapper.Setup(x => x.Map<SpecialtyToInstitutionOfEducationDTO>(It.IsAny<SpecialtyToInstitutionOfEducationPostApiModel>())).Returns(It.IsAny<SpecialtyToInstitutionOfEducationDTO>());
            _mapper.Setup(x => x.Map<SpecialtyToInstitutionOfEducation>(It.IsAny<SpecialtyToInstitutionOfEducationDTO>())).Returns(new SpecialtyToInstitutionOfEducation());

            // Act
            var exception = await Record
                .ExceptionAsync(() => _ioEAdminService.GetSpecialtyToIoEDescription("userId","IoEId"));

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
            var admin = new InstitutionOfEducationAdminDTO { Id = "userId", InstitutionOfEducationId = "IoEId" };

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

            _ioEAdminRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(admin);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Func<Task> act = () => _ioEAdminService.GetSpecialtyToIoEDescription("userId", "IoEId");

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void GetSpecialtyToIoEDescription_ShouldThrowBadRequestException_IfSpecialtyNotFound()
        {
            // Arrange  
            //Specialty not found
            var specialty = false;

            var institutionOfEducation = true;
            var specialtyToIoe = new List<SpecialtyToInstitutionOfEducationDTO>
            { new SpecialtyToInstitutionOfEducationDTO{ Id = "SpecialtyToIoeId", InstitutionOfEducationId = "IoEId", SpecialtyId = "SpecialtyId" } };
            var admin = new InstitutionOfEducationAdminDTO { Id = "userId", InstitutionOfEducationId = "IoEId" };

            _specialtyRepository.
                Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(specialty);

            _ioERepository
                .Setup(sr => sr.ContainsById(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducation);

            _specialtyToIoERepository
                .Setup(sr => sr.Find(It.IsAny<Expression<Func<SpecialtyToInstitutionOfEducation, bool>>>()))
                .ReturnsAsync(specialtyToIoe);

            _ioEAdminRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(admin);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Func<Task> act = () => _ioEAdminService.GetSpecialtyToIoEDescription("userId", "IoEId");

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public void GetSpecialtyToIoEDescription_ShouldThrowBadRequestException_IfSpecialtyInInstitutionOfEducationNotFound()
        {
            // Arrange  
            var specialty = true;
            var institutionOfEducation = true;
            var admin = new InstitutionOfEducationAdminDTO { Id = "userId", InstitutionOfEducationId = "IoEId" };

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

            _ioEAdminRepository
                .Setup(sr => sr.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(admin);

            _specialtyToIoERepository.Setup(sr => sr.Update(It.IsAny<SpecialtyToInstitutionOfEducation>()));

            // Act
            Func<Task> act = () => _ioEAdminService.GetSpecialtyToIoEDescription("userId", "IoEId");

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task DeleteIoEModerator_ShouldDeleteIoEModerator_IfEverythingOk()
        {
            // Arrange  
            _ioEAdminRepository.Setup(p => p.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(new InstitutionOfEducationAdminDTO());
            var user = new UserDTO() { Id = "blabla" };
            _ioEModeratorRepository.Setup(p => p.GetModeratorForAdmin(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new InstitutionOfEducationModeratorDTO() { User = user });
            _userRepository.Setup(x => x.GetUserWithRoles(It.IsAny<string>())).ReturnsAsync(new DbUser());
            _userManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<DbUser>(), It.IsAny<string>()));

            // Act
            var result = await  _ioEAdminService.DeleteIoEModerator(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteIoEModerator_ShouldThrowNotFoundException_IfModeratorWasNotFound()
        {
            // Arrange  
            InstitutionOfEducationModeratorDTO moderator = null;
            _ioEAdminRepository.Setup(p => p.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(new InstitutionOfEducationAdminDTO());
            _ioEModeratorRepository.Setup(p => p.GetModeratorForAdmin(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(moderator);

            // Act
            Func<Task> act = () => _ioEAdminService.DeleteIoEModerator(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task DeleteIoEModerator_ShouldThrowBadRequestException_IfModeratorIsAlreadyDeleted()
        {
            // Arrange  
            InstitutionOfEducationModeratorDTO moderator = new InstitutionOfEducationModeratorDTO() { IsDeleted = true };
            _ioEAdminRepository.Setup(p => p.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(new InstitutionOfEducationAdminDTO());
            _ioEModeratorRepository.Setup(p => p.GetModeratorForAdmin(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(moderator);

            // Act
            Func<Task> act = () => _ioEAdminService.DeleteIoEModerator(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.ThrowsAsync<BadRequestException>(act);
        }

        [Fact]
        public async Task DisableIoE_ReturnsSuccessDisableMessage()
        {
            //Arrange
            var ioEMonederator = new InstitutionOfEducationModeratorDTO() { IsBanned = false };
            _ioEModeratorRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(ioEMonederator);

            _ioEModeratorRepository.Setup(x => x.Disable(It.IsAny<InstitutionOfEducationModerator>())).Returns(Task.FromResult("IoE Moderator isBanned was set to true"));
            _mapper.Setup(x => x.Map<InstitutionOfEducationModerator>(It.IsAny<InstitutionOfEducationModeratorDTO>())).Returns(It.IsAny<InstitutionOfEducationModerator>());

            //Act
            var result = await _ioEAdminService.ChangeBannedStatusOfIoEModerator(It.IsAny<string>());

            //Assert
            Assert.Equal("IoE Moderator isBanned was set to true", result.Object.Message);
        }

        [Fact]
        public async Task DisableIoE_ReturnsSuccessEnableMessage()
        {
            //Arrange
            var ioEMonederator = new InstitutionOfEducationModeratorDTO() { IsBanned = true };
            _ioEModeratorRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(ioEMonederator);

            _ioEModeratorRepository.Setup(x => x.Enable(It.IsAny<InstitutionOfEducationModerator>())).Returns(Task.FromResult("IoE Moderator isBanned was set to false"));
            _mapper.Setup(x => x.Map<InstitutionOfEducationModerator>(It.IsAny<InstitutionOfEducationModeratorDTO>())).Returns(It.IsAny<InstitutionOfEducationModerator>());

            //Act
            var result = await _ioEAdminService.ChangeBannedStatusOfIoEModerator(It.IsAny<string>());

            //Assert
            Assert.Equal("IoE Moderator isBanned was set to false", result.Object.Message);
        }
    }
}
