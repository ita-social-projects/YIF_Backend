using System;
using System.Resources;
using System.Threading.Tasks;
using Moq;
using SendGrid.Helpers.Errors.Model;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class IoEAdminServiceTests
    {
        private readonly IoEAdminService _ioEAdminService;
        private readonly Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>> _specialtyRepository= new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
        private readonly Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>> 
            _specialtyToIoERepository = new Mock<ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO>>();
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> 
            _ioERepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        
        public IoEAdminServiceTests()
        {
            _ioEAdminService = new IoEAdminService(
                _specialtyRepository.Object,
                _ioERepository.Object,
                _specialtyToIoERepository.Object,
                _resourceManager.Object
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
    }
}
