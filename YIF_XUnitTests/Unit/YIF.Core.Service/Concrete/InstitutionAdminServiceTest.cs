using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using Moq;
using Xunit;
using YIF.Core.Service.Concrete.Services;
using YIF_XUnitTests.Unit.TestData;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete
{
    public class InstitutionAdminServiceTest
    {
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> _institutionOfEducationRepository;
        private readonly Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO>> _institutionOfEducationAdminRepository;
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<IConfiguration> _configuration;

        private readonly IInstitutionAdminService _institutionAdminService;

        public InstitutionAdminServiceTest()
        {
            _institutionOfEducationRepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
            _institutionOfEducationAdminRepository = new Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdminDTO>>();
            _resourceManager = new Mock<ResourceManager>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();
            _configuration = new Mock<IConfiguration>();

            _institutionAdminService = new InstitutionAdminService(
                _institutionOfEducationRepository.Object,
                _institutionOfEducationAdminRepository.Object,
                _resourceManager.Object,
                _mapper.Object,
                _env.Object,
                _configuration.Object);
        }

        [Fact]
        public void ModifyDescriptionOfInstitution_WrongAdminId()
        {
            // Arrange
            var wrongAdminId = "0";
            var listOfAdmins = InstitutionOfEducationAdminTestData.GetIEnumerableInstitutionOfEducationAdminDTO();
            _institutionOfEducationAdminRepository.Setup(x => x.GetAllUniAdmins())
                .Returns(Task.FromResult(listOfAdmins));

            // Act
            var result = _institutionAdminService.ModifyDescriptionOfInstitution(wrongAdminId, new InstitutionOfEducationPostApiModel());

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

            _institutionOfEducationAdminRepository.Setup(x => x.GetAllUniAdmins())
                .Returns(Task.FromResult(listOfAdmins));
            _mapper.Setup(x => x.Map<InstitutionOfEducationDTO>(It.IsAny<InstitutionOfEducationPostApiModel>()))
                .Returns(institutionDTO);
            _mapper.Setup(x => x.Map<InstitutionOfEducation>(It.IsAny<InstitutionOfEducationDTO>()))
                .Returns(institution);
            _institutionOfEducationRepository.Setup(x => x.Update(It.IsAny<InstitutionOfEducation>()))
                .Returns(Task.FromResult(true));
            _resourceManager.Setup(x => x.GetString(It.IsAny<string>()))
                .Returns("");

            // Act
            var result = _institutionAdminService.ModifyDescriptionOfInstitution(listOfAdmins.FirstOrDefault().Id, new InstitutionOfEducationPostApiModel());

            // Assert
            Assert.True(result.Result.Success);
        }
    }
}
