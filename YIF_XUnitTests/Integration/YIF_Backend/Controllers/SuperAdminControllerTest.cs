using AutoMapper;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF_XUnitTests.Integration.Fixture;
using YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class SuperAdminControllerTest : TestServerFixture
    {
        private readonly IMapper _mapper;
        private readonly IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO> _institutionOfEducationAdminRepository;
        public SuperAdminControllerTest(ApiWebApplicationFactory fixture)
        {
            _client = getInstance(fixture);
            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            _mapper = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IMapper>();
            _institutionOfEducationAdminRepository = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<IInstitutionOfEducationAdminRepository<InstitutionOfEducationAdmin, InstitutionOfEducationAdminDTO>>();
        }

        [Fact]
        public async Task AddInstitutionOfEducationAdmin_Output_Correct()
        {
            // Arrange
            var InstitutionOfEducation = new InstitutionOfEducation() { Name = "newInstitutionOfEducationTest1" };
            _context.InstitutionOfEducations.Add(InstitutionOfEducation);
            _context.SaveChanges();

            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAdmin",
                Body = new InstitutionOfEducationAdminApiModel() { InstitutionOfEducationId = InstitutionOfEducation.Id, AdminEmail = "AdminEmailTest1@gmial.com" }
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(null, "test@gmail.com")]
        [InlineData("123", null)]
        [InlineData("", "test@gmail.com")]
        [InlineData("123", "")]
        public async Task AddInstitutionOfEducationAdmin_Input_WrongInstitutionOfEducationAdminApiModel(string institutionOfEducationId, string adminEmail)
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAdmin",
                Body = new InstitutionOfEducationAdminApiModel() { InstitutionOfEducationId = institutionOfEducationId, AdminEmail = adminEmail }
            };
            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddInstitutionOfEducationAdmin_Output_ByAddingSameAdminTwoTimes()
        {
            // Arrange
            var InstitutionOfEducation = new InstitutionOfEducation() { Name = "newInstitutionOfEducation" };
            _context.InstitutionOfEducations.Add(InstitutionOfEducation);
            _context.SaveChanges();

            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAdmin",
                Body = new InstitutionOfEducationAdminApiModel() { InstitutionOfEducationId = InstitutionOfEducation.Id, AdminEmail = "AdminEmailTest@gmial.com" }
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.AdminEmail = "AdminEmailTest2@gmial.com";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Theory]
        [MemberData(nameof(SuperAdminInputAttribute.GetWrongData), MemberType = typeof(SuperAdminInputAttribute))]
        public async Task AddInstitutionOfEducationAndAdmin_Input_WrongInstitutionOfEducationPostApiModel_site(StringContent content)
        {
            // Act
            var response = await _client.PostAsync("/api/SuperAdmin/AddInstitutionOfEducationAndAdmin", content);

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddInstitutionOfEducationAndAdmin_Output_WithCorectData()
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };

            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddInstitutionOfEducationAndAdmin_Output_ByAddingSameInstitutionOfEducationTwoTimes()
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.InstitutionOfEducationAdminEmail = "name@gmail.com";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task AddInstitutionOfEducationAndAdmin_Output_ByAddingSameAdminTwoTimes()
        {
            // Arrange
            var postRequest = new
            {
                Url = "/api/SuperAdmin/AddInstitutionOfEducationAndAdmin",
                Body = SuperAdminInputAttribute.GetCorrectData
            };
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            response.EnsureSuccessStatusCode();

            postRequest.Body.Name = "name";
            // Act
            response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task GetAllInstitutionOfEducationsAdmins()
        {
            // Arrange
            var request = "/api/SuperAdmin/GetAllInstitutionOfEducationsAdmins";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAllInstitutionOfEducationsAdmins_withAllParams()
        {
            // Arrange
            var compareDTO = await _institutionOfEducationAdminRepository.GetAllUniAdmins();
            compareDTO = compareDTO.OrderBy(x => x.User.UserName).Take(2).ToList();
            var compare = _mapper.Map<IEnumerable<InstitutionOfEducationAdminResponseApiModel>>(compareDTO);

            // Act
            var response = await _client.GetAsync("/api/SuperAdmin/GetAllInstitutionOfEducationsAdmins?UserName=true&Email=true&InstitutionOfEducationName=true&IsBanned=true&page=1&pageSize=2");

            // Assert
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<PageResponseApiModel<InstitutionOfEducationAdminResponseApiModel>>(stringResponse);

            Assert.Equal(compare.FirstOrDefault().Id, models.ResponseList.FirstOrDefault().Id);
        }

        [Fact]
        public async Task DeleteInstitutionOfEducationAdmin()
        {
            // Arrange
            var admin = _context.InstitutionOfEducationAdmins.First();

            // Act
            var response = await _client.DeleteAsync(string.Format("/api/SuperAdmin/DeleteInstitutionOfEducationAdmin/{0}", admin.Id));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DisableInstitutionOfEducationAdmin()
        {
            // Arrange
            var admin = _context.InstitutionOfEducationAdmins.Last();
            var content = ContentHelper.GetStringContent(admin);

            // Act
            var response = await _client.PatchAsync(string.Format("/api/SuperAdmin/DisableInstitutionOfEducationAdmin/{0}", admin.Id), content);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("Інформація", "Системний аналіз", "66")]
        public async Task AddSpecialtyToListOfSpecialties_ShouldReturnOk(string name, string description, string code)
        {
            //Arrange
            var directionId = _context.Directions.AsNoTracking().FirstOrDefault().Id;

            var model = new SpecialtyPostApiModel()
            {
                Name = name,
                DirectionId = directionId,
                Description = description,
                Code = code
            };

            // Act            
            var response = await _client.PostAsync($"/api/SuperAdmin/AddSpecialty", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateSpecialtyDescription_EndpointReturnOk()
        {
            //Arrange
            var specialty = _context.Specialties.AsNoTracking().FirstOrDefault();
            var model = new SpecialtyPutApiModel
            {
                Id = specialty.Id,
                Description = specialty.Description,
                Name = specialty.Name,
                DirectionId = specialty.DirectionId,
                Code = specialty.Code
            };

            // Act            
            var response = await _client.PutAsync($"/api/SuperAdmin/UpdateSpecialty", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void GetModeratorsByIoEId_EndpointReturnsListOfModeratorsWithOkStatusCode_IfEverythingIsOk()
        {
            // Arrange
            var ioEId = _context.InstitutionOfEducations.FirstOrDefault().Id;

            // Act
            var response = await _client.GetAsync($"api/SuperAdmin/GetIoEModeratorsById/{ioEId}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task BanInstitutionOfEducation()
        {
            // Arrange
            var IoE = _context.InstitutionOfEducations.First();

            // Act
            var response = await _client.PatchAsync(string.Format("/api/SuperAdmin/BanInstitutionOfEducation/{0}", IoE.Id), ContentHelper.GetStringContent(IoE));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void ChooseIoEAdminFromModerators_IfEverythingOk()
        {
            //Arrange
            var moderator = _context.InstitutionOfEducationModerators
                .Include(x => x.User)
                .Include(x => x.Admin)
                .ThenInclude(x => x.InstitutionOfEducation).AsNoTracking().FirstOrDefault();

            var list = _context.InstitutionOfEducationAdmins
                .Where(x => x.InstitutionOfEducationId == moderator.Admin.InstitutionOfEducationId).ToList();

            foreach (var item in list)
            {
                item.IsDeleted = true;
            }
            await _context.SaveChangesAsync();

            var model = new IoEAdminAddFromModeratorsApiModel
            {
                IoEId = moderator.Admin.InstitutionOfEducationId,
                UserId = moderator.UserId
            };

            //Act
            var response = await _client.PutAsync($"api/SuperAdmin/ChooseIoEAdminFromModerators", ContentHelper.GetStringContent(model));

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteInstitutionOfEducation_EndpointReturnsOk()
        {
            // Arrange
            var institutionOfEducation = _context.InstitutionOfEducations.First();

            // Act
            var response = await _client.DeleteAsync(string.Format("/api/SuperAdmin/DeleteInstitutionOfEducation/{0}", institutionOfEducation.Id));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetIoEAdminIdByIoEId_EndpointReturnsOk()
        {
            // Arrange
            var institutionOfEducation = _context.InstitutionOfEducations.First();

            // Act
            var response = await _client.GetAsync(string.Format("/api/SuperAdmin/GetIoEAdminIdByIoEId/{0}", institutionOfEducation.Id));

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}