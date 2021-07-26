using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Resources;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class LectorServiceTests
    {
        private readonly Mock<IDepartmentRepository<Department, DepartmentDTO>> _departmentRepository;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<ILectorRepository<Lector, LectorDTO>> _lectorRepository;
        private readonly LectorService lectorService;

        public LectorServiceTests() 
        {
            _departmentRepository = new Mock<IDepartmentRepository<Department, DepartmentDTO>>();
            _mapperMock = new Mock<IMapper>();
            _resourceManager = new Mock<ResourceManager>();
            _env = new Mock<IWebHostEnvironment>();
            _configuration = new Mock<IConfiguration>();
            _lectorRepository = new Mock<ILectorRepository<Lector, LectorDTO>>();

            lectorService = new LectorService(
                _mapperMock.Object,
                _resourceManager.Object,
                _env.Object,
                _configuration.Object,
                _lectorRepository.Object,
                _departmentRepository.Object);
        }

        [Fact]
        public void ModifyLector_ReturnTrue()
        {
            // Arrange
            var user = new UserDTO()
            {
                Id = "ID!"
            };

            var lectorDTO = new LectorDTO()
            {
                Id = "id",
                UserId = "fakeUserId",
                User = user
            };
            var newUser = new UserDTO()
            {
                Id = "ID!"
            };

            var newlectorDTO = new LectorDTO()
            {
                Id = "id",
                UserId = "fakeUserId",
                User = newUser
            };

            _lectorRepository.Setup(x => x.GetLectorByUserId(It.IsAny<string>()))
                .ReturnsAsync(lectorDTO);

            _mapperMock.Setup(x => x.Map<LectorDTO>(It.IsAny<LectorApiModel>()))
                .Returns(newlectorDTO);

            _mapperMock.Setup(x => x.Map<Lector>(It.IsAny<LectorDTO>()))
                .Returns(It.IsAny<Lector>());

            _lectorRepository.Setup(x => x.Update(It.IsAny<Lector>()))
                .Returns(Task.FromResult(true));

            _resourceManager.Setup(x => x.GetString(It.IsAny<string>()))
                .Returns("");

            // Act
            var result = lectorService.ModifyLector(It.IsAny<string>(), new JsonPatchDocument<LectorApiModel>());

            // Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public async Task GetAllDepartmets_ShouldReturnAllDepartment_IfDatabaseNotEmpty()
        {
            // Arrange
            var list = new List<DepartmentDTO>() { new DepartmentDTO() }.AsEnumerable();
            _departmentRepository.Setup(x => x.GetAll()).ReturnsAsync(list);

            // Act
            var result = await _lectorService.GetAllDepartments();

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllDescriptions_ShouldReturnException_IfDatabaseIsEmpty()
        {
            // Arrange
            _departmentRepository.Setup(x => x.GetAll()).ReturnsAsync(new List<DepartmentDTO>().AsEnumerable());
            // Assert
            await Assert.ThrowsAnyAsync<NotFoundException>(() => _lectorService.GetAllDepartments());
        }
    }
}
