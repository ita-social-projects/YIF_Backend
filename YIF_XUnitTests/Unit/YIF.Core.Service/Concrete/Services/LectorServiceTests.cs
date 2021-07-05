using AutoMapper;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class LectorServiceTests
    {

        private readonly LectorService _lectorService;
        private readonly Mock<IDepartmentRepository<Department, DepartmentDTO>> _departmentRepository = new Mock<IDepartmentRepository<Department, DepartmentDTO>>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        public LectorServiceTests()
        {
            _lectorService = new LectorService(
                _departmentRepository.Object,
                _resourceManager.Object,
                _mapper.Object
            );
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
