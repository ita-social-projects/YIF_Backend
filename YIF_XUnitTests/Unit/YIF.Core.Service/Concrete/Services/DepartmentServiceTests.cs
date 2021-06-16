using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Moq;
using System.Resources;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using SendGrid.Helpers.Errors.Model;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Service.Concrete.Services;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class DepartmentServiceTests
    {
        private readonly DepartmentService _departmentService;
        private readonly Mock<IDepartmentRepository<Department, DepartmentDTO>> _departmentRepository = new Mock<IDepartmentRepository<Department, DepartmentDTO>>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        public DepartmentServiceTests()
        {
            _departmentService = new DepartmentService(
                _departmentRepository.Object,
                _resourceManager.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task AddDepartment_ReturnsSuccess()
        {
            //Arrange
            List<DepartmentDTO> departments = null;

            _departmentRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Department, bool>>>())).ReturnsAsync(departments);
            _departmentRepository.Setup(x => x.Add(new Department { Name = It.IsAny<string>(), Description = It.IsAny<string>() })).Returns(Task.FromResult(string.Empty));

            //Act
            var result = await _departmentService.AddDepartment(It.IsAny<DepartmentApiModel>());

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task AddDepartment_ShouldThrowBadRequestIfDepartmentAlreadyExist()
        {
            //Arrange
            List<DepartmentDTO> departments = new List<DepartmentDTO>
            {
                new DepartmentDTO
                {
                    Name = It.IsAny<string>(),
                    Description = It.IsAny<string>()
                }
            };

            _departmentRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Department, bool>>>())).ReturnsAsync(departments);
            _departmentRepository.Setup(x => x.Add(new Department { Name = It.IsAny<string>(), Description = It.IsAny<string>() })).Returns(Task.FromResult(string.Empty));

            //Act
            Func<Task> act = () => _departmentService.AddDepartment(It.IsAny<DepartmentApiModel>());

            //Assert
            await Assert.ThrowsAsync<BadRequestException>(act);
        }
    }
}
