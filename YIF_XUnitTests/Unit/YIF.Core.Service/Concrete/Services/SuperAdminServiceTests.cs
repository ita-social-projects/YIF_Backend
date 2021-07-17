using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Resources;                                
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;
using YIF.Shared;
using YIF_XUnitTests.Unit.TestData;

namespace YIF_XUnitTests.Unit.YIF.Core.Service.Concrete.Services
{
    public class SuperAdminServiceTests
    {
        private readonly Mock<IUserService<DbUser>> _userService;
        private readonly Mock<IUserRepository<DbUser, UserDTO>> _userRepository;
        private readonly Mock<FakeUserManager<DbUser>> _userManager;
        private readonly FakeSignInManager<DbUser> _signInManager;
        private readonly Mock<IJwtService> _jwtService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IDirectionRepository<Direction, DirectionDTO>> _directionRepository;
        private readonly Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>> _institutionOfEducationAdminRepository;
        private readonly Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>> _institutionOfEducationRepository;
        private readonly Mock<ISchoolRepository<SchoolDTO>> _schoolRepository;
        private readonly Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>> _specialtyRepository;
        private readonly Mock<ISchoolAdminRepository<SchoolAdminDTO>> _schoolAdminRepository;
        private readonly Mock<ISchoolModeratorRepository<SchoolModeratorDTO>> _schoolModeratorRepository;
        private readonly Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>> _ioEModeratorRepository;
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<ITokenRepository<TokenDTO>> _tokenRepository;
        private readonly Mock<ResourceManager> _resourceManager;
        private readonly SuperAdminService superAdminService;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IPaginationService> _paginationService;
        private readonly Mock<IIoEBufferRepository<IoEBuffer, IoEBufferDTO>> _ioEBufferRepository;

        private readonly DbUser _user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" };
        private readonly InstitutionOfEducationAdmin uniAdmin = new InstitutionOfEducationAdmin { Id = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", InstitutionOfEducationId = "007a43f8-7553-4eec-9e91-898a9cba37c9", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };
        private readonly InstitutionOfEducation uni = new InstitutionOfEducation { Id = "007a43f8-7553-4eec-9e91-898a9cba37c9", Name = "Uni1Stub", Description = "Descripton1Stub", ImagePath = "Image1Path" };
        private readonly InstitutionOfEducationModerator institutionOfEducationModerator = new InstitutionOfEducationModerator { Id = "057f5632-56a6-4d64-97fa-1842d02ffb2c", AdminId = "3b16d794-7aaa-4ca5-943a-36d328f86ed3", UserId = "b87613a2-e535-4c95-a34c-ecd182272cba" };
        private readonly Specialty specialty = new Specialty { Id = "39e9621f-5baf-47bc-8c04-09cb25e84f44", IsDeleted = false };

        private readonly List<InstitutionOfEducationAdmin> _databaseUniAdmins = new List<InstitutionOfEducationAdmin>();
        private readonly List<InstitutionOfEducationAdminDTO> _institutionOfEducationAdminsDTO;
        private readonly List<DbUser> _databaseDbUsers = new List<DbUser>();
        private readonly List<InstitutionOfEducation> _databaseInstitutionOfEducations = new List<InstitutionOfEducation>();
        private readonly List<InstitutionOfEducationModerator> _databaseInstitutionOfEducationModerators = new List<InstitutionOfEducationModerator>();
        private readonly List<InstitutionOfEducationAdminResponseApiModel> _listViewModel;

        private readonly Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();

        public readonly InstitutionOfEducationAdminApiModel model = new InstitutionOfEducationAdminApiModel
        {
            InstitutionOfEducationId = "Name",
            AdminEmail = "AdminEmail",
        };
        public SuperAdminServiceTests()
        {
            _userService = new Mock<IUserService<DbUser>>();
            _userRepository = new Mock<IUserRepository<DbUser, UserDTO>>();
            _userManager = new Mock<FakeUserManager<DbUser>>();
            _signInManager = new FakeSignInManager<DbUser>(_userManager);
            _jwtService = new Mock<IJwtService>();
            _mapperMock = new Mock<IMapper>();
            _directionRepository = new Mock<IDirectionRepository<Direction, DirectionDTO>>();
            _institutionOfEducationAdminRepository = new Mock<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>>();
            _institutionOfEducationRepository = new Mock<IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO>>();
            _schoolRepository = new Mock<ISchoolRepository<SchoolDTO>>();
            _specialtyRepository = new Mock<ISpecialtyRepository<Specialty, SpecialtyDTO>>();
            _schoolAdminRepository = new Mock<ISchoolAdminRepository<SchoolAdminDTO>>();
            _schoolModeratorRepository = new Mock<ISchoolModeratorRepository<SchoolModeratorDTO>>();
            _ioEModeratorRepository = new Mock<IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO>>();
            _dbContextMock = new Mock<IApplicationDbContext>();
            _tokenRepository = new Mock<ITokenRepository<TokenDTO>>();
            _resourceManager = new Mock<ResourceManager>();
            _env = new Mock<IWebHostEnvironment>();
            _configuration = new Mock<IConfiguration>();
            _paginationService = new Mock<IPaginationService>();
            _ioEBufferRepository = new Mock<IIoEBufferRepository<IoEBuffer, IoEBufferDTO>>();

            superAdminService = new SuperAdminService(
                                                    _userService.Object,
                                                    _userRepository.Object,
                                                    _userManager.Object,
                                                    _signInManager,
                                                    _jwtService.Object,
                                                    _mapperMock.Object,
                                                    _directionRepository.Object,
                                                    _institutionOfEducationRepository.Object,
                                                    _institutionOfEducationAdminRepository.Object,
                                                    _schoolRepository.Object,
                                                    _specialtyRepository.Object,
                                                    _schoolAdminRepository.Object,
                                                    _schoolModeratorRepository.Object,
                                                    _ioEModeratorRepository.Object,
                                                    _tokenRepository.Object,
                                                    _resourceManager.Object,
                                                    _env.Object,
                                                    _configuration.Object,
                                                    _paginationService.Object,
                                                    _ioEBufferRepository.Object);

            _dbContextMock.Setup(p => p.InstitutionOfEducationAdmins).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationAdmin>(_databaseUniAdmins));
            _dbContextMock.Setup(p => p.Users).Returns(DbContextMock.GetQueryableMockDbSet<DbUser>(_databaseDbUsers));
            _dbContextMock.Setup(p => p.InstitutionOfEducations).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducation>(_databaseInstitutionOfEducations));
            _dbContextMock.Setup(p => p.InstitutionOfEducationModerators).Returns(DbContextMock.GetQueryableMockDbSet<InstitutionOfEducationModerator>(_databaseInstitutionOfEducationModerators));


            _dbContextMock.Setup(s => s.SaveChangesAsync()).Verifiable();
            _databaseDbUsers.Add(_user);
            _databaseInstitutionOfEducations.Add(uni);
            _databaseUniAdmins.Add(uniAdmin);
            _databaseInstitutionOfEducationModerators.Add(institutionOfEducationModerator);

            _institutionOfEducationAdminsDTO = new List<InstitutionOfEducationAdminDTO>()
            {
                new InstitutionOfEducationAdminDTO {Id="1", InstitutionOfEducationId = "007a43f8-7553-4eec-9e91-898a9cba37c9" },
                new InstitutionOfEducationAdminDTO {Id="2", InstitutionOfEducationId = "107a43f8-7553-4eec-9e91-898a9cba37c9" },
            };
            _listViewModel = new List<InstitutionOfEducationAdminResponseApiModel>()
            {
                new InstitutionOfEducationAdminResponseApiModel { Id = _institutionOfEducationAdminsDTO[0].Id,  },
                new InstitutionOfEducationAdminResponseApiModel { Id = _institutionOfEducationAdminsDTO[1].Id,  }
            };
        }

        [Fact]
        public async Task AddDirection_ShouldAddDirection()
        {
            //Arrange
            _mapperMock.Setup(sr => sr.Map<DirectionDTO>(It.IsAny<DirectionPostApiModel>())).Returns(It.IsAny<DirectionDTO>());
            _mapperMock.Setup(sr => sr.Map<Direction>(It.IsAny<DirectionDTO>())).Returns(It.IsAny<Direction>());
            _directionRepository.Setup(sr => sr.Add(It.IsAny<Direction>()));

            //Act
            var result = await superAdminService.AddDirection(new DirectionPostApiModel());

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnEmpty_IfThereAreNoAdmins()
        {
            //Arrange
            var sortingModel = InstitutionOfEducationAdminTestData.GetEmptyInstitutionOfEducationAdminSortingModel();
            var page = new PageApiModel();
            IEnumerable<InstitutionOfEducationAdminDTO> admins = new List<InstitutionOfEducationAdminDTO>();
            var responseApiModel = new PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>() { ResponseList = new List<InstitutionOfEducationAdminResponseApiModel>() };

            _institutionOfEducationAdminRepository.Setup(s => s.GetAllUniAdmins()).Returns(Task.FromResult(admins));
            _mapperMock.Setup(s => s.Map<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(admins)).Returns(InstitutionOfEducationAdminTestData.GetInstitutionOfEducationAdminResponseApiModels());
            _paginationService
                .Setup(ps => ps.GetPageFromCollection(It.IsAny<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(), It.IsAny<PageApiModel>()))
                .Returns(responseApiModel);

            // Act
            var result = await superAdminService.GetAllInstitutionOfEducationAdmins(sortingModel, page);

            //Assert            
            Assert.Empty(result.ResponseList);
        }

        [Fact]
        public async Task GetAllUniAdmins_ShouldReturnAllAdmins_IfThereAreMoreThanOneAdmin()
        {            
            // Arrange
            var sortingModel = InstitutionOfEducationAdminTestData.GetEmptyInstitutionOfEducationAdminSortingModel();
            var page = new PageApiModel();
            var admins = InstitutionOfEducationAdminTestData.GetIEnumerableInstitutionOfEducationAdminDTO();
            var institutionAdminResponseApiModels = InstitutionOfEducationAdminTestData.GetInstitutionOfEducationAdminResponseApiModels();
            var responseApiModel = new PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>() { ResponseList = institutionAdminResponseApiModels };

            _institutionOfEducationAdminRepository.Setup(s => s.GetAllUniAdmins()).Returns(Task.FromResult(admins));
            _mapperMock.Setup(s => s.Map<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(admins)).Returns(institutionAdminResponseApiModels);
            _paginationService
                .Setup(ps => ps.GetPageFromCollection(It.IsAny<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(), It.IsAny<PageApiModel>()))
                .Returns(responseApiModel);

            // Act
            var result = await superAdminService.GetAllInstitutionOfEducationAdmins(sortingModel, page);

            //Assert            
            Assert.IsType<PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>>(result);
            Assert.NotEmpty(result.ResponseList);
        }

        [Theory]
        [InlineData("institutionId", "adminEmail")]
        public async Task AddInstitutionOfEducationAdmin_InstituionAlreadyExist(string institutionId, string adminEmail)
        {
            // Act
            _institutionOfEducationRepository.Setup(p => p.ContainsById(It.IsAny<string>())).ReturnsAsync(false);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.AddInstitutionOfEducationAdmin(institutionId, adminEmail, httpRequest.Object));
        }

        [Theory]
        [InlineData("institutionId", "adminEmail")]
        public async Task AddInstitutionOfEducationAdmin_ThisInstitutionAlreadyHaveAdmin(string institutionId, string adminEmail)
        {
            //Arrange
            var admin = new InstitutionOfEducationAdminDTO();

            _institutionOfEducationRepository.Setup(p => p.ContainsById(It.IsAny<string>())).ReturnsAsync(true);
            _institutionOfEducationAdminRepository.Setup(p => p.GetByInstitutionOfEducationIdWithoutIsDeletedCheck(It.IsAny<string>())).ReturnsAsync(admin);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => superAdminService.AddInstitutionOfEducationAdmin(institutionId, adminEmail, httpRequest.Object));
        }

        [Theory]
        [InlineData("institutionId", "adminEmail")]
        public async Task AddInstitutionOfEducationAdmin_UserExistAndHaveAdminPermision(string institutionId, string adminEmail)
        {            
            //Arrange
            InstitutionOfEducationAdminDTO admin = new InstitutionOfEducationAdminDTO();
            DbUser dbUser = new DbUser();
            IEnumerable<InstitutionOfEducationAdminDTO> institutionOfEducationAdmins = new List<InstitutionOfEducationAdminDTO>()
            {
                new InstitutionOfEducationAdminDTO(){ Id = institutionId}
            };

            _institutionOfEducationRepository.Setup(p => p.ContainsById(It.IsAny<string>())).ReturnsAsync(true);
            _institutionOfEducationAdminRepository.Setup(p => p.GetByInstitutionOfEducationIdWithoutIsDeletedCheck(It.IsAny<string>())).ReturnsAsync(admin);
            _userManager.Setup(p => p.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dbUser);
            _institutionOfEducationAdminRepository.Setup(p => p.GetAllUniAdmins()).ReturnsAsync(institutionOfEducationAdmins);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => superAdminService.AddInstitutionOfEducationAdmin(institutionId, adminEmail, httpRequest.Object));
        }

        [Fact]
        public async Task DeleteAdmin_ReturnsSuccessDeleteMessage()
        {
            //Arrange
            var user = new DbUser { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", Email = "shadj_hadjf@maliberty.com", UserName= "Jeremiah Gibson"};
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                {
                    Id = uniAdmin.Id,
                    UserId = uniAdmin.UserId,
                    User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" }
                }));
            _userManager.Setup(s => s.FindByIdAsync(uniAdmin.UserId)).ReturnsAsync(uniAdmin.UserId == "" ? null : user);
            _institutionOfEducationAdminRepository.Setup(s => s.Delete(uniAdmin.Id)).Returns(Task.FromResult<bool>(true));
            _userManager.Setup(s => s.RemoveFromRoleAsync(user, ProjectRoles.InstitutionOfEducationAdmin));
            _userRepository.Setup(s => s.Delete(user.Id));
           
            //Act
            var result = await superAdminService.DeleteInstitutionOfEducationAdmin(uniAdmin.Id);

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteAdmin_NoAdminfound()
        {
            //Arrange
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(null));

            //Act

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.DeleteInstitutionOfEducationAdmin(uniAdmin.Id));
        }

        [Fact]
        public async Task DisableAdmin_ReturnsSuccessDisableMessage()
        {
            //Arrange
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                {
                    Id = uniAdmin.Id,
                    UserId = uniAdmin.UserId,
                    User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" }
                }));
            _institutionOfEducationAdminRepository.Setup(x => x.Disable(uniAdmin)).Returns(Task.FromResult("Admin IsBanned was set to true"));
            _mapperMock.Setup(x => x.Map<InstitutionOfEducationAdmin>(It.IsAny<InstitutionOfEducationAdminDTO>())).Returns(uniAdmin);

            //Act
            var result = await superAdminService.DisableInstitutionOfEducationAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("Admin IsBanned was set to true", result.Object.Message);
        }

        [Fact]
        public async Task DisableAdmin_ReturnsSuccessEnableMessage()
        {
            //Arrange
            _institutionOfEducationAdminRepository
                .Setup(p => p.GetUserByAdminId(uniAdmin.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                {
                    Id = uniAdmin.Id,
                    UserId = uniAdmin.UserId,
                    User = new UserDTO { Id = "b87613a2-e535-4c95-a34c-ecd182272cba", UserName = "Jeremiah Gibson", Email = "shadj_hadjf@maliberty.com" },
                    IsBanned = true
                }));
            _institutionOfEducationAdminRepository.Setup(x => x.Enable(uniAdmin)).Returns(Task.FromResult("Admin IsBanned was set to false"));
            _mapperMock.Setup(x => x.Map<InstitutionOfEducationAdmin>(It.IsAny<InstitutionOfEducationAdminDTO>())).Returns(uniAdmin);

            //Act
            var result = await superAdminService.DisableInstitutionOfEducationAdmin(uniAdmin.Id);

            //Assert
            Assert.Equal("Admin IsBanned was set to false", result.Object.Message);
        }

        [Fact]
        public async Task AddSpecialtyToListOfSpecialties_ShouldAddSpecialty()
        {
            //Arrange
            _mapperMock.Setup(sr => sr.Map<SpecialtyDTO>(It.IsAny<SpecialtyPostApiModel>())).Returns(It.IsAny<SpecialtyDTO>());
            _mapperMock.Setup(sr => sr.Map<Specialty>(It.IsAny<SpecialtyDTO>())).Returns(It.IsAny<Specialty>());
            _specialtyRepository.Setup(sr => sr.Add(It.IsAny<Specialty>()));

            //Act
            var result = await superAdminService.AddSpecialtyToTheListOfAllSpecialties(new SpecialtyPostApiModel());

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateSpecialtyById_ShouldUpdateSpecialtyByIdAndReturnCorrectMessage_IfEverythingIsOk()
        {
            //Arrange
            _mapperMock.Setup(sr => sr.Map<SpecialtyDTO>(It.IsAny<SpecialtyDescriptionUpdateApiModel>())).Returns(It.IsAny<SpecialtyDTO>());
            _mapperMock.Setup(sr => sr.Map<Specialty>(It.IsAny<SpecialtyDTO>())).Returns(It.IsAny<Specialty>());
            _specialtyRepository.Setup(sr => sr.Update(It.IsAny<Specialty>())).ReturnsAsync(true);

            //Act
            var result = await superAdminService.UpdateSpecialtyById(new SpecialtyPutApiModel());

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async void GetIoEModeratorsByIoEId_ShouldReturnListOfModerators_IfEverythingIsOk()
        {
            // Arrange  
            _ioEModeratorRepository.Setup(x => x.GetByIoEId(It.IsAny<string>())).ReturnsAsync(It.IsAny<IEnumerable<InstitutionOfEducationModeratorDTO>>);
            _mapperMock.Setup(x => x.Map<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>(It.IsAny<IEnumerable<InstitutionOfEducationModeratorDTO>>()));

            // Act
            var result = await superAdminService.GetIoEModeratorsByIoEId(It.IsAny<string>());

            // Assert  
            Assert.IsType<ResponseApiModel<IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DisableIoE_ReturnsSuccessDisableMessage()
        {
            //Arrange
            _institutionOfEducationRepository
                .Setup(p => p.Get(uni.Id))
                .ReturnsAsync(new InstitutionOfEducationDTO());
            _institutionOfEducationRepository.Setup(x => x.Disable(uni)).Returns(Task.FromResult("InstitutionOfEducation isBanned was set to true"));
            _mapperMock.Setup(x => x.Map<InstitutionOfEducation>(It.IsAny<InstitutionOfEducationDTO>())).Returns(uni);

            //Act
            var result = await superAdminService.ChangeBannedStatusOfIoE(uni.Id);
            
            //Assert
            Assert.Equal("InstitutionOfEducation isBanned was set to true", result.Object.Message);
        }

        [Fact]
        public async Task DisableIoE_ReturnsSuccessEnableMessage()
        {
            //Arrange
            _institutionOfEducationRepository
                .Setup(p => p.Get(uni.Id))
                .ReturnsAsync(new InstitutionOfEducationDTO() { 
                IsBanned = true
                });
            _institutionOfEducationRepository.Setup(x => x.Enable(uni)).Returns(Task.FromResult("InstitutionOfEducation isBanned was set to false"));
            _mapperMock.Setup(x => x.Map<InstitutionOfEducation>(It.IsAny<InstitutionOfEducationDTO>())).Returns(uni);

            //Act
            var result = await superAdminService.ChangeBannedStatusOfIoE(uni.Id);

            //Assert
            Assert.Equal("InstitutionOfEducation isBanned was set to false", result.Object.Message);
        }

        [Fact]
        public async void ChooseIoEAdminFromModerators_ShouldAddAdmin_IfEverythingIsOk()
        {
            //Arrange
            _ioEModeratorRepository.Setup(x => x.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(new InstitutionOfEducationModeratorDTO { Id = It.IsAny<string>(), User = new UserDTO { Id = It.IsAny<string>() } });
            _userRepository.Setup(x => x.GetUserWithRoles(It.IsAny<string>())).ReturnsAsync(new DbUser());
            _userManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<DbUser>(), It.IsAny<string>()));
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<DbUser>(), It.IsAny<string>()));
            _ioEModeratorRepository.Setup(x => x.Delete(It.IsAny<string>())).ReturnsAsync(true);
            _institutionOfEducationAdminRepository.Setup(x => x.AddUniAdmin(It.IsAny<InstitutionOfEducationAdmin>())).ReturnsAsync(It.IsAny<string>());

            //Act
            var result =
                await superAdminService.ChooseIoEAdminFromModerators(new IoEAdminAddFromModeratorsApiModel { IoEId = It.IsAny<string>(), UserId = It.IsAny<string>() });

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async void DeleteInstitutionOfEducation_ReturnsSuccess()
        {
            //Arrange
            _institutionOfEducationRepository.Setup(x => x.Get(uni.Id))
                .Returns(Task.FromResult<InstitutionOfEducationDTO>(new InstitutionOfEducationDTO
                {
                    Id = uni.Id
                }));
            _institutionOfEducationRepository.Setup(x => x.Delete(uni.Id))
                .Returns(Task.FromResult<bool>(true));
            //Act
            var result = await superAdminService.DeleteInstitutionOfEducation(uni.Id);
            //Assert
            Assert.Equal("IoEIsDeleted", result.Object.Message);
        }

        [Fact]
        public async void DeleteInstitutionOfEducation_ReturnsNotFoundMessage()
        {
            //Arrange
            _institutionOfEducationRepository
                .Setup(x => x.Get(uni.Id))
                .Returns(Task.FromResult<InstitutionOfEducationDTO>(null));
            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.DeleteInstitutionOfEducation(uni.Id));
        }

        [Fact]
        public async Task GetIoEAdminIdByIoEId_ReturnsSuccess()
        {
            //Arrange
            _institutionOfEducationAdminRepository.Setup(x => x.GetByInstitutionOfEducationId(uni.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                {
                    InstitutionOfEducationId = uni.Id
                }));

            //Act
            var result = await superAdminService.GetIoEAdminIdByIoEId(uni.Id);

            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetIoEAdminIdByIoEId_ReturnsNotFoundIfThereIsNoIoEWithSuchId()
        {
            //Arrange
            InstitutionOfEducationAdminDTO nullAdmin = null;
            _institutionOfEducationAdminRepository.Setup(x => x.GetByInstitutionOfEducationId(It.IsAny<string>()))
                .ReturnsAsync(nullAdmin);

            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.GetIoEAdminIdByIoEId(It.IsAny<string>()));
        }

        [Fact]
        public async void DeleteSpecialty_ReturnsSuccess()
        {
            //Arrange
            var specialtyDTO = new SpecialtyDTO { Id = specialty.Id, IsDeleted = specialty.IsDeleted};

            _specialtyRepository.Setup(x => x.Get(specialty.Id)).Returns(Task.FromResult<SpecialtyDTO>(specialtyDTO));
            _specialtyRepository.Setup(x => x.Delete(specialty.Id)).Returns(Task.FromResult<bool>(true));
           
            //Act
            var result = await superAdminService.DeleteSpecialty(specialty.Id);
            
            //Assert
            Assert.IsType<ResponseApiModel<DescriptionResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async void DeleteSpecialty_ReturnsNotFoundMessage()
        {
            //Arrange
            _specialtyRepository.Setup(x => x.Get(specialty.Id)).Returns(Task.FromResult<SpecialtyDTO>(null));
          
            //Act
            
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.DeleteSpecialty(specialty.Id));
        }

        [Fact]
        public async void DeleteSpecialty_BadRequestExceptionIfSpecialtyAlreadyDeleted()
        {
            //Arrange
            var specialtyDTO = new SpecialtyDTO { Id = specialty.Id, IsDeleted = true };

            _specialtyRepository.Setup(x => x.Get(specialty.Id)).Returns(Task.FromResult<SpecialtyDTO>(specialtyDTO));
            
            //Act
            
            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => superAdminService.DeleteSpecialty(specialty.Id));
        }

        [Fact]
        public async Task GetIoEInfoByIoEId_ReturnsSuccess()
        {
            //Arrange
            var ioE = new InstitutionOfEducationDTO()
            {
                Id = uni.Id
            };
            _institutionOfEducationAdminRepository.Setup(x => x.GetByInstitutionOfEducationId(uni.Id))
                .Returns(Task.FromResult<InstitutionOfEducationAdminDTO>(new InstitutionOfEducationAdminDTO
                {
                    Id = "FakeId",
                    InstitutionOfEducationId = uni.Id
                }));
            _institutionOfEducationRepository.Setup(x => x.Get(uni.Id)).ReturnsAsync(ioE);
            _mapperMock.Setup(x => x.Map<IoEforSuperAdminResponseApiModel>(ioE)).Returns(new IoEforSuperAdminResponseApiModel());

            //Act
            var result = await superAdminService.GetIoEInfoByIoEId(uni.Id);

            //Assert
            Assert.IsType<ResponseApiModel<IoEforSuperAdminResponseApiModel>>(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task GetIoEInfoByIoEId_ReturnsBadRequestIfAdminIsDeleted()
        {
            //Arrange
            InstitutionOfEducationAdminDTO nullAdmin = null;
            var ioE = new InstitutionOfEducationDTO()
            {
                Id = uni.Id
            };
            _institutionOfEducationRepository.Setup(x => x.Get(uni.Id)).ReturnsAsync(ioE);
            _institutionOfEducationAdminRepository.Setup(x => x.GetByInstitutionOfEducationId(It.IsAny<string>()))
                .ReturnsAsync(nullAdmin);

            //Act
            //Assert
            await Assert.ThrowsAsync<BadRequestException>(() => superAdminService.GetIoEInfoByIoEId(uni.Id));
        }

        [Fact]
        public async Task GetIoEInfoByIoEId_ReturnsNotFoundIfThereIsNoIoEWithSuchId()
        {
            //Arrange
            InstitutionOfEducationDTO nullIoE = null;
            _institutionOfEducationRepository.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(nullIoE);

            //Act
            //Assert
            await Assert.ThrowsAsync<NotFoundException>(() => superAdminService.GetIoEInfoByIoEId(uni.Id));
        }

        [Fact]
        public void ModifyInstitution_WrongAdminId()
        {
            // Arrange
            var wrongAdminId = "0";
            var listOfAdmins = InstitutionOfEducationAdminTestData.GetIEnumerableInstitutionOfEducationAdminDTO();
            _institutionOfEducationAdminRepository.Setup(x => x.GetAllUniAdmins())
                .Returns(Task.FromResult(listOfAdmins));

            // Act
            Func<Task> act = () => superAdminService.ModifyIoE(wrongAdminId, new JsonPatchDocument<InstitutionOfEducationPostApiModel>());

            // Assert
            Assert.ThrowsAsync<NullReferenceException>(act);
        }

        [Fact]
        public void ModifyInstitution_ReturnTrue()
        {
            // Arrange
            var institutionOfEducationAdminDTO = new InstitutionOfEducationAdminDTO()
            {
                InstitutionOfEducationId = "id"
            };

            _institutionOfEducationAdminRepository.Setup(x => x.GetByUserId(It.IsAny<string>()))
                .ReturnsAsync(institutionOfEducationAdminDTO);

            _institutionOfEducationRepository.Setup(x => x.Get(It.IsAny<string>()))
                .ReturnsAsync(new InstitutionOfEducationDTO());

            _mapperMock.Setup(x => x.Map<JsonPatchDocument<InstitutionOfEducationDTO>>(It.IsAny<JsonPatchDocument<InstitutionOfEducationPostApiModel>>()))
                .Returns(new JsonPatchDocument<InstitutionOfEducationDTO>());

            _mapperMock.Setup(x => x.Map<InstitutionOfEducation>(It.IsAny<InstitutionOfEducationDTO>()))
                .Returns(It.IsAny<InstitutionOfEducation>());

            _institutionOfEducationRepository.Setup(x => x.Update(It.IsAny<InstitutionOfEducation>()))
                .Returns(Task.FromResult(true));

            _resourceManager.Setup(x => x.GetString(It.IsAny<string>()))
                .Returns("");

            // Act
            var result = superAdminService.ModifyIoE(It.IsAny<string>(), new JsonPatchDocument<InstitutionOfEducationPostApiModel>());

            // Assert
            Assert.True(result.Result.Success);
        }
    }
}
