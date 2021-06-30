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
    public class DisciplineServiceTests
    {
        private readonly DisciplineService _disciplineService;
        private readonly Mock<IDisciplineRepository<Discipline, DisciplineDTO>> _disciplineRepository = new Mock<IDisciplineRepository<Discipline, DisciplineDTO>>();
        private readonly Mock<ResourceManager> _resourceManager = new Mock<ResourceManager>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        public DisciplineServiceTests()
        {
            _disciplineService = new DisciplineService(
                _disciplineRepository.Object,
                _resourceManager.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task AddDiscipline_ReturnsSuccess()
        {
            //Arrange
            List<DisciplineDTO> disciplines = null;

            _disciplineRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Discipline, bool>>>())).ReturnsAsync(disciplines);
            _disciplineRepository.Setup(x => x.Add(new Discipline { Name = It.IsAny<string>(), Description = It.IsAny<string>() })).Returns(Task.FromResult(string.Empty)); ;

            //Act
            var result = await _disciplineService.AddDiscipline(It.IsAny<DisciplineApiModel>());

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task AddDiscipline_ShouldThrowBadRequestIfDisciplineAlreadyExist()
        {
            //Arrange
            List<DisciplineDTO> disciplines = new List<DisciplineDTO>
            {
                new DisciplineDTO
                {
                Description = It.IsAny<string>(),
                Name = It.IsAny<string>()
                }
            };

            _disciplineRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Discipline, bool>>>())).ReturnsAsync(disciplines);
            _disciplineRepository.Setup(x => x.Add(new Discipline { Name = It.IsAny<string>(), Description = It.IsAny<string>() })).Returns(Task.FromResult(string.Empty)); ;

            //Act
            Func<Task> result = () => _disciplineService.AddDiscipline(It.IsAny<DisciplineApiModel>());

            //Assert
            await Assert.ThrowsAsync<BadRequestException>(result);
        }
    }
}
