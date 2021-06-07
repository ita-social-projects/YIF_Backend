using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF_XUnitTests.Integration.Fixture;
using YIF_XUnitTests.Integration.YIF_Backend.Controllers.DataAttribute;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using YIF.Core.Data.Entities;

namespace YIF_XUnitTests.Integration.YIF_Backend.Controllers
{
    public class InstitutionOfEducationAdminControllerTests : TestServerFixture
    {
        private IoEAdminInputAttribute _adminInputAttribute;
        public InstitutionOfEducationAdminControllerTests(ApiWebApplicationFactory fixture)
        {
            _client = getInstance(fixture);

            _client = fixture.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                });
            }).CreateClient();

            _adminInputAttribute = new IoEAdminInputAttribute(_context);
        }

        [Fact]
        public async Task AddSpecialtyToIoE_ShouldReturnOk()
        {
            //Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var chosen = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().FirstOrDefault();

            _context.SpecialtyToInstitutionOfEducations.Remove(chosen);
            await _context.SaveChangesAsync();

            var model = new SpecialtyToInstitutionOfEducationPostApiModel()
            {
                SpecialtyId = chosen.SpecialtyId,
                InstitutionOfEducationId = chosen.InstitutionOfEducationId
            };

            // Act            
            var response = await _client.PostAsync($"/api/InstitutionOfEducationAdmin/AddSpecialtyToInstitutionOfEducation", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteSpecialtyFromIoE_EndpointReturnNoContent()
        {
            //Arrange
            var specialty = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().FirstOrDefault();

            var model = new SpecialtyToInstitutionOfEducationPostApiModel()
            {
                SpecialtyId = specialty.SpecialtyId,
                InstitutionOfEducationId = specialty.InstitutionOfEducationId
            };

            //Act
            var response = await _client.PatchAsync(
                $"/api/InstitutionOfEducationAdmin/DeleteSpecialtyFromInstitutionOfEducation", ContentHelper.GetStringContent(model));

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task UpdateSpecialtyDescription_EndpointReturnOk()
        {
            //Arrange
            var description = _context.SpecialtyToIoEDescriptions.Where(x => x.Description != null).AsNoTracking().FirstOrDefault();
            var examRequirements = _context.ExamRequirements.AsNoTracking().Where(x => x.SpecialtyToIoEDescriptionId == description.Id).ToList();

            var examRequirementsUpdateApiModel = new List<ExamRequirementUpdateApiModel>();
            foreach (var item in examRequirements)
            {
                examRequirementsUpdateApiModel.Add(new ExamRequirementUpdateApiModel
                {
                    ExamId = item.ExamId,
                    SpecialtyToIoEDescriptionId = item.SpecialtyToIoEDescriptionId,
                    Coefficient = item.Coefficient,
                    MinimumScore = item.MinimumScore
                });
            }

            var model = new SpecialtyDescriptionUpdateApiModel
            {
                Id = description.Id,
                SpecialtyToInstitutionOfEducationId = description.SpecialtyToInstitutionOfEducationId,
                PaymentForm = description.PaymentForm,
                EducationForm = description.EducationForm,
                EducationalProgramLink = description.EducationalProgramLink,
                Description = description.Description,
                ExamRequirements = examRequirementsUpdateApiModel
            };

            // Act            
            var response = await _client.PutAsync($"/api/InstitutionOfEducationAdmin/Specialty/Description/Update", ContentHelper.GetStringContent(model));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void GetModerators_EndpointReturnsListOfModeratorsWithOkStatusCode_IfEverythingIsOk()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();

            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducationAdmin/GetIoEModerators");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void GetIoEInfo_EndpointReturnsIoEWithOkStatusCode_IfEverythingIsOk()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();

            // Act
            var response = await _client.GetAsync($"api/InstitutionOfEducationAdmin/GetIoEInfoByUserId");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetSpecialtyDescription_EndpointReturnsSuccessAndCorrectContentType()
        {
            //Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var admin = _context.InstitutionOfEducationAdmins.AsNoTracking().FirstOrDefault();
            var institutionOfEducation = _context.InstitutionOfEducations.AsNoTracking().Where(i => i.Id == admin.InstitutionOfEducationId).FirstOrDefault();
            var specialtyToIoE = _context.SpecialtyToInstitutionOfEducations.AsNoTracking().Where(x => x.InstitutionOfEducationId == institutionOfEducation.Id).FirstOrDefault();

            //Act
            var response = await _client.GetAsync(
                $"/api/InstitutionOfEducationAdmin/Specialty/Description/Get/" + specialtyToIoE.SpecialtyId);

            //Assert
            response.EnsureSuccessStatusCode(); 
            Assert.Equal("application/json; charset=utf-8",
                 response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task DeleteIoEModerator_EndpointReturnOk()
        {
            //Arrange
            var ioEModerator = _context.InstitutionOfEducationModerators.
                Include(x => x.Admin).AsNoTracking().FirstOrDefault();
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext(ioEModerator.Admin.UserId);

            //Act
            var response = await _client.DeleteAsync(
                $"/api/InstitutionOfEducationAdmin/DeleteIoEModerator?moderatorId={ioEModerator.Id}");

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task BanIoEModerator_ShouldReturnOk()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var ioEModerator = _context.InstitutionOfEducationModerators.AsNoTracking().FirstOrDefault();

            // Act
            var response = await _client.PatchAsync(string.Format("/api/InstitutionOfEducationAdmin/BanIoEModerator/{0}",
                ioEModerator.Id), ContentHelper.GetStringContent(ioEModerator));

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddInstitutionOfEducationModerator_Output_Correct()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var postRequest = new
            {
                Url = "/api/InstitutionOfEducationAdmin/AddIoEModerator",
                Body = new EmailApiModel() { UserEmail = "AdminEmailTest1@gmail.com" }
            };

            //Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddInstitutionOfEducationModerator_Input_WrongEmailApiModel(string moderatorEmail)
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var postRequest = new
            {
                Url = "/api/InstitutionOfEducationAdmin/AddIoEModerator",
                Body = new EmailApiModel() { UserEmail = moderatorEmail }
            };

            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest 
                || response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddInstitutionOfEducationModerator_Output_ByAddingSameModeratorTwoTimes()
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var postRequest = new
            {
                Url = "/api/InstitutionOfEducationAdmin/AddIoEModerator",
                Body = new EmailApiModel() { UserEmail = "AdminEmailTest5@gmail.com" }
            };

            //Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));
            var secondResponse = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.True(secondResponse.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ModifyInstitution_ShouldReturnOk()
        {
            // Arrange
            #region InstitutionOfEducation base64 photo
            var base64Photo = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoHCBUVFBcVFRUYGBcaGxsbGxsaHBshHR0bGxgaGiIdGxsdICwkGx0pHiAdJTYlKS4wMzMzGyI5PjkyPSwyMzABCwsLEA4QHhISHjsqJCkyNDIyMzIyMjQyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMv/AABEIAMIBAwMBIgACEQEDEQH/xAAbAAACAwEBAQAAAAAAAAAAAAAEBQADBgIBB//EAEcQAAECAwUFBgIHBAkEAwEAAAECEQADIQQFEjFBIlFhcYEGEzKRobHB0SNCUmKC4fAUFXKiFiQzQ5KywtLxNFNjc5Oj4gf/xAAZAQADAQEBAAAAAAAAAAAAAAAAAQIDBAX/xAAmEQACAgICAwABBAMAAAAAAAAAAQIRITESQQMTUWEiMnGxgZGh/9oADAMBAAIRAxEAPwD7NEiRIAJHhMePuj0CADyPQI5KwPlrHBBOdB6+enSADpSwKa7hnHLE504DPz+UdhLZUjoCADkBuAjuOVKAzjhych1Pyz9oALCYrxvkH46fn0j0I315/KLIAKsD5l+Gnl84sAj2JAB5EjhawASSABmTkOe6EVs7SoTSUO8O8UTzxa9ARxEA0m9D+Elu7SSkOEfSK+6dl+KsvJ4y1rvSZPJSVFY+wiiBz39cXSLLNdMyZmWTuTQNxUc+j8ozc/hfBLLZLffc2YcKllI+xLd+RLuepA4RTZrBNXQJwDhVXyT6RorDc0tGnlT1zPtDHClCdEpHICJdvY+aWIoQ2a50S2K86H7Si3E0Ff8AmCptnQpLBAGj1KqnN9eUeW63pKgJYxnq3TUwOe8AxFQTuFN70BzPrG0Eq0ZTUnls5t9zqRtyyxZidFcFjTnlyimw29aFsnYXqg+FXLf77iY0VlvIL2AllffyPLed4LHnAl43AJgc1+7oP4Tp5Rm1TtFKVKmH3de6J1AWXql92eE5KHqNQIZxgVylyztAkBtseNO7Fv5v10h7d99sAJpBSclj/V8/MaxSkmDj2jRRI4SoEOC4OREdxRJIkSJABIkSJABIkSJAB4THjb4hIEc1PAev5QAdKWBnHFTwHr8hHqUgZecdtABwlLZeesdtHJWBz3R5U8Pf8oAOioCOHJ4c8/KOkpAjuACtKBnmd5/VIsiRIAPIkD2q1y5ScUxYSN6izncN54CM1eXayjSUsP8AuLoPwozPVjwMJtLY1FvRqJ89KElS1BKRmVEADqYztv7VJDiSnF99eygdKE9cI4xmZi509QUrEs6KW7fgQPcAcYaWLs+7GYX56ckjZHrEOfwvjGP7mLrRbJs87RVMrrsyweAyJ9eMG2S4lrrMNN1Qn/DmeZaNFZ7GhGQrv16bhyim13gmWwSkrUXACWZ9xOh4MYh52HNvET2y3bLQGAfmA3llBC7ShBwlQCtEvtdEisLFptExQxqEmWQo7jQpbXEczSkB39d6JdmmKQlRVhVtq2RUaDM/qsVxbRLS7Ybab6SViXLIMxRYChL8hQfiI5RYm7cSnmzCtX2UV9TQcqRk+zsgd9LGEkOHegb3PWPpICUhqARs/CobyZLyvoT2qzYcIQkSwQcqqOWZMUiQgO9VNmoud+ukG3opylgcjw3dYAKhhIejHwij8Tv8o1joltsdWqxJWHUWOT7+Y16NAsmfMlqCF1QXZSnpT7WZHMU5CGUiWA/uanzNT1hf2gkImSilRZQLoUH2F1YkjIPRzSsYtZNU8BFosYmByRwKePE5iM3bbnVLJUhuX1Vc9Unj6iO+z3frliYFklmIJ1BLnDkfQ6w+kWhKk7agkhwQ+EUpTXo5aM2rHlZQhu29lSzgrxQrMcU/l1AzjUWS1omJdBfeMiOY0hJeF1IX4EktlhDNxSssOjmFUqdNkrdTJamNieiwGbmH65w062PD0bmJC2wXilbBRZTdDxBEMoskkSJEgAkSJEgAoQtJKgKkM+eofPloIuaF1imkqWyFbS1EqIZOyAgM9VOEguARxg7C+flpA0B6V7q8o8YnOnL5x2BHsAHKUgZR1EiQAeRIX3hesqSNtYfRIqo9B7lhGTvHtVNmHDKBQODKX1PhR77jCbS2Uotmwt95S5IeYsJ3DMnkkVMZW8u1yzsyk4HyUraWf4UBwOZccoTSbumzFjE6cQJKiSVEuM1mp5jhGjsFyS5Yyc68eZzPUtwiJSfQ1xW8mel2WfOXiVic5qXtK5N4UDgS26HVj7PpScS9pW8lz5nLoOsGqvKVLASk4iGGFFa8Tl8YU2/tMpKhKTKUJhBOHg5DkkZ0yCT0iVFydLLCU3XxD4plykklkjU1JPxJhbab/CSMEskVck1YJKnA1y3jpGes1qmzpie8WEjE2CtWOuayH4gcIa3lKSJKy+SVFsh4SMhmebxT8bjslOLFlp7RzZqMUsYUbzU1LeHwjkoqhj2OlqVMWozCVYGJ8RAKhQE7I5AQhlI+jSKlmDEsB4ch13RqOyA2pgf6qcqatz0jr9UYp0jBzs0fdoStJ1wqqouc0ZE/CFna5f8AVZtD4TnT8/SGqmSsZDZVzNU9TCntSrFJwMWXsk5NR9Q5y3Rl0UstIz1yH6eW50TlzjehIFcuPzMYW6ZX0ktScRZSXKUmgB1NfhGzBOYR1Wr2bF8IuU1PKJfjcMMEvdadnXPlpqaQvM0sqjDCatw4t8YLvcF0YlfabCG+zvc+UL3SHcaFioh+mIv7RUdD6NDZpqCNl1/w1T5jYEdWoqKQyQkYkZmo20/VFPWPLHPBGylRyejaa4meKb3VM7skYUsRvV8mq1Yxls0jlC7slJ+hKcRYKUGFMlqGY2vWAL6siEWtBSooEwKSt9VAAhYJzLHe9OMMOzksbaCVeJSwASAApROac82rHN+y0pnyCkAbZduKFZ/4YnDQ8oLl2sySlKnUglklmLs7DEQ9NA/CL50kTgWKBxBxGuhFAOReK+0skLs0wFL7JUN6SASFDr6E8iJYLPhkCcgqxBOIgVJZINBqS2Rz4QqHsWW67pkkukkjNnYfhIqk8Dwzg66L2wgpWVKD1ckqTzB0/XCCf3tJmpSp9QTQkM4B055jTlEvG5AraRQjJsxy3jgfSHdIV/R3KmBQBSQQciIsjHWW2rkKZVK/gV/sVGksV4Im+Gihmk0I+YgTsbQdEiRIYhdZ7xC5ikDwgBlbySzQxj5zYb0TIQVBSiUuAgFgC5T+IHN/u5aQZdPbFaxMCwHCXSzOC2rmtW84bolM3MexibL2zSl8brZIbCzlWpUcgPypAlpv202ikt0IOiXHmvxK/CAIm0WlZrLxvuTJopWJf2U1V10T1IjLW7tLOmumWMCctg1/FMNE/hqOMc2Xs+TWYafZGXkM+pPKG/dypCXIbQUc9AMvQRDk+irivyIrJcUxZeYWBqWevM+JXHKH1kuuVLFAKb2amu7rC609odpMpEpYmTCyHAyYHFWjB+OWsC9p7EuXLCjNV3iilDglxiWnw8YFBvJMp3hsOtV4kTCZcvvMCTV2S76ndQMeMD2S2rti1I7tSZSMQKsQAKhpkXHGvKC7okIQjYQR9GC6ztVmTCSSSTV/WAuxiHSZhUdpC1M5YYphNN2lRGy8SvPwyc8WjRWO70IKmZOQ2c8vtna8sPKMjb5WK2TAl0ghALEgnxmpzjaItMsLWnEMVC2rEMK8wfKMNarUTPmTEgsSljlklteJ0eKi4xab0FSndbO7nlgzEDJyvKh358oY34uWmVNQlScZQRhd1E5ANmYT2SxTCxqwfaALV45noDDCRdSXGIkAA7gS7ZA1/lEZ+WalVfDWPj4tuTE6ScIDNUGtN2QqdI03ZOUsGYWLEJb6qczr4ovs1klo8Euu8j/dXyEM7EVEnIZbzv1o3lDU5SeWTJRUaSLwg4xUJ2T4RXNOai7+UUXlLl7ONjUnbL6ZgGg6CLJgAUCtRYJNVKYZjNmHnCW/rwlKlTES1AnAs7ILeH7QDGKemRFNtFtkvmUuYmXLUVqejAtSviIAy3Q9dZ+yPNXyb1jB3HNVilolS0pUz4lHgWoNOsagXXMX/azlKH2U0T6UPlA48cLIOpZeCu+JyAUgzMRDuAajLRDesASrSz4JRZqkAD016tDC2XfLlhIQkDNzmaNvpA6ZfPzP6EUkwuND+7Z6VBgoEhnGo6QXNWACSQBvOUKLNZEzEh6EBOFSaKDjePyjuaiYhJTMAmymOJwMQTq4+tyZ+MZyu8lKqwF2eUkTCUgB01bWubQnvyQrvAsg4QUKB0dIUGf6viVwrHnZdYSqZLCjhTMmJQFGoQF0SAa0G+sNLbeCErTLWSlSwcLg4VcApmfg+o3xPRV0d25QMgv9ZKfVoF7OLeyIIzwBueEQVabP3kkIDCiSHyo0U3LJVLSpCgwSaCjM2h1hgKLksqe7myyPBMWlxm3eMW3Ui+6b2nKxBaAQhSkLZ3BS2W4V1fLOObqV/wBQ2pUobyGSXbm8EXInDaLQn75PmEmG1glPIfOsyJyXpX9MoRnLZY1yTQlhUVqG1SdR90/OC7tsx7+ckKUChZwOaAKCVNyqW55ReL6QsFC2SQ4UVJ2QUlquR8uMRRSxoFl9pJjCiDxch+mkSJ+7ZZyUT0HxVEh5Dkvh84st8CXkgKGagSasC1S7VL89YFXaFKJUzYjVnZzVhrAAwhG8jUn7wzB6xcm07CUpJfE7PQUJNMt1YydvAlSGdnBALgONFU8n1+Uaq4b7ThEtKCpbUCBnxJOXrGHs6CsqBLhIOb72pSlTBd1CZLmFYYYCKls91aQ4xY2zdWa2zLTMVKZUoIAMwpqXLMkHr04wPdNlAtykusolp2QpTpClYnUoOxLFhujjsnaFLmTpmBnIDnkkdPDu1ju5Zr2qeSpvLQIGZpmTHTGEcMylN5Rdem1b5bqOynQt9dG7RgYs7V2lLSgA47xBplskqz6QElKlW3GlBKUqwvpslRcEkCvXKDr3shmzAVKYBIpnVzXcDD6x0xJZz8O1TlCStWQ7uWKMTtDjTV84E7JSMAwrJZKEpTUh9rTJ4NSEpQvEThTgFdyUJzbPKOrkvBE1KlISpKXSxUAHG8AHLm0U7bEqSYdZHE2asSyElMtIOyASkrJo+KmIZjzgCTYkpqEpfeXUfM5eUEzL4lIBBWCp1URtHM7qDrA9ptQloKzkBlvOgjKdF+Pkiu87wlSU4piidyRmeQDBuJjNHtuxZEgYf4mfyS3vGYva8V2iZNq+AVOjjFQfdDe8cXNZcdmtMxRcpSkDg4JpGWTU+lXNf8q0uElpiQ6kEhwMnDZh/wBBxDMImqLS5gQPrFgT0cR8o7I2dYtUtSA2EnEeGEuOrtH2CwnPp8Y1jHKM5SpAyLnQV/SKXNLPtk5vpVx5xX2hkITZ5gQgDYXkK+HzMNH2/wAPxim3ykzMKVVBPyi3FU6JUm2rZjezxPfyiQd2W998fQUwImxy5Y2UgVT/AJhBIVFW3szddAV7/UrqfhC0JTqPOp9YYXvUJ5n4QrUUClK72r84aKWh7c52c/qo9oMtiwJa3IGyRXiG94XXKWSP4E+0MJyApJSWrvDjyjOayClRmrpsCFzp6CKJmzMLHLEoKpFnaNCkS5aVqxpM2WUqPiSygGyq4JqT7tB9jsglTmAAxJORLHKrHLzMUdqSTLwhJPgOLQNNlqL7qJPnEUaKdugmz3mFy8MtkzGZKZjgKI0cb/OLbkt3fIKynCSwUk1ZQJSQ+tREky0rkpU1QklKhQhnZiK5wouAzkd6E4FYZkwKSXBJExdUnTq8A1T0NhYkJWSmmJCktpVstRyga73TaVEg/SVHROfGggW652G0zEFSsBIUlKySUlaTiSCqpGIHhSkG2C9Za5iZIP0ssIxAhvFLdwdRUQVgV5yeWYpFrmgttBJrv2h/pEDWSTJVNtCVBB2jUh2YAu5y1MMZ9lCZwmBR2nxBj9gjSh5GFUw4LRNUUgAgE1ywpd9wqznc+bQFAVkUooH0i6ONkMNklNA1MokIrTbZ6VrCQnCFqahyxHeIkKgyfP1oLoG8PXpw/wCXjtCyFBhkK1IcbqVPJ49xo7wM7CrHQcGPyzgiUUOtZIS74Q5yHQncw1PKMqrQrPZM4Pq27X1/XKCLLalpCiEgBR5ZPkXfWKZFpQjGgpCwoMxzSdCCKgvpryj1NoSzhTEBmKQxoHIDnMwlobNX2OU6pilKehIGKr1cs79fzh7dEqVLUoS2xAAKqCoPVjujGXVa5pqFKO8pSlgDoSQPf829xJmGWrAoSwVrJOEFSnWS9aZEeUdXiyl+DKXf5HEi8XnqRhAQiqlqUBUglgGqN5JGcAX3bkrmSglZUglTiWrxEYWDg1FXzaBrrlJXNm49tla5URLDtk9TEvT+3QEIJCQxwpoCVIPLIGNNJWLt0H2m1Te6mkoCUMvMup8GXBjTpE7PWFE2We8KlBKsOF9mgfIM/OLbTLMySpALYyt+RKnbi0E3HZe7lsS5UrGeqRuh1lpivCoZ2eQhCNhITQ5CuuucZjtbaimWQk1CSR/EXA8qmNNj+j6RmLyShcwJWEkE0xZOAN+tT6xlNaL8b3Zk7msITKtDtiwsmoqcKst+cM7ksqv2WfKCFFcwgJwgkABKQ6iMqvD6ySpISViUjZUEuQCHpVw9A+fAw4l2gqlrKGYPgLMDShrmH9oirZpaQhui550vCUoQkjE5Wc8TaJc0YRsLscJZRc0cjUtuhJY7yQubhMxGIgpCBMC1EhlEnDQUB3ZcWhrY5jJIFTT213RpHaM56Dwrb/CPcwBftqMqWqYkOUAqAPBoLSrbP8I91Qp7Vn+rzP4FfCKlpkx2gK7LfaJk2WZi04CfClJrQ6qJOdabo1gMYm5LOkTkFqjIlyQ9KPwjZvFuKWkRdgN85J5n2hShaQW2R5QxvpilPM+0K5cxILOB1AgLWh/coASOKU728tIYWkqwKwFOJtnF4X+82kKblIH+EfCD7dMwypiq0Qo0JBokmhBDHqIzkiexdYLyVMnmWtGGZLopiFJOJAWClVHDEaDOHE6WlWeehdj0PwjLXMVftSypRUSmUpyzl5Kc2A/WbmsUf/0ATkiVOkzFoKMYVhdiFBJ2qYaYTnvO4xC1Zbj+qkamyAmRhBILKDhndzvpAtzSVS5kxJzU6y4YupVaZZ7i0WXTMPcg+IsTpU58qxxd15JmzlMFJUlJSpKgQUnZUx0NC9CYKBN5QuXZ0LtsxJSClSRio20DMB64cNYq/Zu5taFy04itGGpAfAtKGdtA2b6bodXhLTiSvXEN1HByJy82hbaHExE1aglCFLAcaKm1JIcaDNsjA1SKUrKbZec4WwIIaWpKSErDAEEJNdXevA6QTb7yQJgTgUZgDmiQjAQUk4tUu4eBL8QFz5Sy5BSUkUoHBDOAWzz9IXXuqYi1yhLUwSAQFK++lRllSgyQxHJ4RSJabLNC1BE9GFy2JiWJepUpz1iQmvK1L71TrnJNHSkFgcIcCm+JCpFWzFGY6llsKQwajtUsn7OXDSOLG+E4gS2hyDsMta6RFzEsthhJOEbgCBxzD+sVAu+VC78flGLeSUWJdSXAAAfIcdT84JkydgHM5NrmzhtNIHkyqM7GoL8OZixGycy2vL2/5hS+gjV3HLaQ+EldSMTNmQGbayhjcllVKlqCi5Jf+UCF9lmJlyDMSKMFNvbnygq7bxM6T3hThfFR9ASI7fHSX+DGVv8A2HWCwplkrFVKFTV6sakmsX/tMoFlFONRZLs+gDRnezNrmTFTTMWVAFIS4AADrybPSBLSxvBJbIAvyxGHyqqQVbdmmtN4JlSAtSSoEEMGc4wRRyN++GV1WwzJYXhw1UAOCThB6gPHzm8r6VMIlvsoNAAMwCATx5npFsq1KUNojfWufDabyi8J7BRwfR59sQlBxKAYAZ67hvjJXlascxOBYTL2sRKUqBLow0UD96EqlggjEgO2T6cpQ96xbYLzVJfu5yU4mcFKiC3NFOkLhF5byPK0P1W5abJNUJgxJOyQEJIDIAICQA2entAUq0FV2zTMmKmKVMzWSo/3dA+nzhx/SaykeMg67CyH8oEtF42aYggTEMasQwJGRZQz4xk0ik2KeyMr+sIYYQy6jOiCPwv500j6Ndw2PL2EYC78MuYJsvCVAEbxtUORAjYXBbitKsQAYgBi+ghReQmOEHbP8KfdULe0csrlGWkElYKaaO1YPQvaVyT7qji0qGJOKoq48ouX7SIumhBddkmpmIUopAcOKVHKp9Y1bwJ+0pLJSlgSNw1Gggh4cP5sJ/xQDfDFKXGp9oVpmpGoHUQxvkApS4115QqlzEuBiA6iGxx0N7kI0AqnTWohjbXMuYBqhQHMpOukA3PLS5IzZVd7LaGK1NE3YmqZmrqSpNoBKCkKQhLvicoQUlRVq+cW9uVKNmKUj62InTClC361pyg6YpHeIKQAXrRtDnHV5WJM1IBALOQ760zGUSlhpZKcv1JvBZci3khs6+0L7gVMFqm95hxqCCrDQOZMug5M3SC7lRgQU12VHMvu1hVcUpUu0l3OM54gqoQXc0Z9zU9YT6Bdnt5TpqLwWnGpUpYSQgiiVhMugJFM8Rb7VYvt5wJSFpWApbYwoqAWUBeEpKqBnIoQ/qNeylG27NUgYiBocIf0R6R7fYLJVjJaYg4RRP8AZJS4Bo+Y31ziS0FXqnFiTimgaTAUADCQ4AxF/F9mmHzDvCyhEwTgvHhLJx7QACnJUdkcfFRq0ys7T2hJlIdKUrC0VUzhBoWdiQXD6UDvA15XhLSlBRMBCjgUFLC1HxKJGdHAen1xXSGJWdWu7LRMWVlMoks5JAJYAVDUyyiQzmLSouJ5Ys30czdwiQ8BbPiKEYlAcTUuKBLZhyHMWzZf0aZjYXq3CooRy9YoloJcuKaiuZ9ILmBfdpSQGahG6tPMnzMcyarKKoGUvY1fjDGfJSJQUFbTB0sd+h4bn3wIiVjICU6cTlTyhvZ7ox0c4WyfUby2Tw4xb0hN0MbSprEB/wCMeZT84uuo93ZE4jklROuaifjFtokpUlKFMwbPKgaLgkBAT9WgpufSOqv6M7wCdmUkIUSMyNG0frnnA4syzaTNIAAChUh6IUH84cSCljhduMDG2oIUgFGIJJIBD7nI5mHWrYXvABbOzZP9iRWuFT0JqWUNOY6xT+55yE7Uo8wUn0Bf0h1b70EnC5IxUDB3aDETsUtKq7QSa57TGvGsLGQzSMbNUqVmgiv1kke4in96INFI8mPvD69SJic0llkbJfIFn3FmpGItailbAkUGvExm5tSpFqCatjtE+SoVBHIH/TFdpMkCi1g7m+YHvC+djEkKJIc5vU1McTA0uUo7iTn9tY05QnNlxgi+VNU4ANY+l9nR3cseJWJYfUhwA5fSPmVzLxzGZIzNBwOu6Polqu6ZMlIRKmd2Qt1HEpLpwkNs51anCHC3IXkrjg1kpW0eSfjAHaScpMlSkKwqCSxDUqN8W2FJSkJJcpSgE7yEs8A9qVfQL/hPwjWemZR2im6VSzMlnvlLW/hMwkZH6oLZcI1GKMJ2cmATJadSKdA8bfFFRd6FNUwG+aoS4+tryMLJUxIIqB1EMr4qlL/a+BhXLmBJFQKiBjiPrqIcqYV7wPvHelqwXPVsnkfaAbtAdSmq63Lb1YvJiIKmKjIcjD9lb1nTJgRNUFYWL4QFOSRmM/KNRfd5pkS0rWVBJUEumrEgmo3UjEdktmeocE+hAjQduC9mGrTEn0VEx0xy2hxc9qC0FaS6VFwWIcEDQx1PmBUyXRi5HMMaHhC3sqv6BH8Kf8ohTcUxrbNTiUQJhYElhtrBoaQ7dIKyzS35ZUl1kJcAZkaE8H9YEvCxrmJBGyEBwRtULZ1BA2Bk+UUdp7zVLnSpYSkpmJYu+IFzXdrlBlqvFMpGJZZKkoBLPUqmaDlBgM0UXpIJs9HUomWcRYnZIURhAoGGQGbU1gS81tIlAElIUhg1E0Yl6Pn/ADbsnKEoMsYg4IAq27iPaOE2VKpIxYcqbKaEOMjnFUJSLU2BagFBNCARiIfLWJEkWxWBIGiUirPRIFYkKh2fErGnAhzvrudtd8GTCwTo77Ndci+o/TRWpB7mUHBxKFWrUwXb5STMlhNMR5eb5NGDg0aAsgkLAD7SgBvBehehhzbbTMlqQks6yz8dHY10HzhbaMCZ0oFQCcRxEkaby/rHd62yWZsopWkpSoFRqWDjc70eLhFpZJexvfc1aUp7ssoqA01Lax1eM7+rK2toJYkULgbxxhLe97IXg7pRLFycJ0fLEN7RxMvJBlqlpExRUXKlBI/VOEatrJKWjR3Kr6EGv1s8zUhzvyhRdCQZs5Wu0POakfCKbPf5RLTLEp2DOpY+UByLfMQDhCXUrWv1ir3bygW0D7HPawkmW2jk8BQP6xpJQaWgcEejRmEhUwpmWkpATkkBs2PNy2XsKw1Xetn+8244i3qRDaatiu6RF2BElGFDkFRUcRq5ABOW7SEthlbSnS9UsG/8U0U6lI8oYWm9ZK6BBTkAUs5Z88vjHKL7loUVJSSXBDqNCGaj1EHrzYcsUDXrd8yZLCUS1FWIkhjvO+KLV2dnmVL2Gwhi5auJamHnyhoe1Sj/AHaSevt+cX2a+5k1QCkkoq6QAAXSXcqBPHpCj47eR+xpYMndMgyVlUwKBAIZt4UHB1GUbKx9p0IThTKWak5gZl+MFTrmWDgwpYJZsRLmpcbLuwygWw9mhNWoYly8IBZnFQDR/wBZxUfHTE52iz+lkxyUyQHbNf5QLb76nTklCkS0pIYs7+b8Il59nzLKWmqLh/DltJTVuKh6wPK7Pz1jElYIcjdUKKTmdCIfHoSfYPZTMQpK0rCVJdjm1G1DZQVMvO0HO0LHJh7NC6bd812Yk1DAvVJrrAFplLQopWCFbjBVDeRnOtJV45q1c1ExQBLLg6890LwecdJNRSEwQ+TNXLlyu6mrlsmmFRDjGpsjF0rtDbE/3oWNy0g+rPCm0f2crPwqH86oAs08lShlhLfrdlEModXXa1yphmd275gK4g6vqIZ352jlz5Jl92tCsSSHqmnEV9Iy6bywnCX8t9YKE9TOU0oH/WsKhWa/sxeslMsIVMQFADMtlxU0C3Wv+uziKpK3BGRBW+eRzjMG1S1DwgszkNHqO7oQSniCYK6C+zXdtlNPsh3qI8lI+cd9p62UHcUHyTM+KhGVmT1rwhU1Sikugr2iMsnD6Dygq0XnNmIMpZQUgUahJdOZy03Qhro1dvtKDYpoUAo92QkblEslQpmFNlAtuvNUuxAoUyhhILAhsQFXprCedfKVWdcpUpSVENjSQQ7g7TMRlxiq03pKVZDKSsd4wDMQKEE1KWKizNweGKhzIvlGEYkTCdWWAOgCC0SF1kmScCXKcvtn5x7BkrB8/wD2glIClLLZVoOURU0HMPzMHzbXKSSAiUp6DDLSwL71IciB1DvDQITT6qWHUfKNPWRzByv7oj0zVCjAa+cXrQkZkE0o1W8xpExyyaglgB4d1B/eQnBhyQyuaxy5iT3kzCp9kOwI1ctDRFzySChKiZmdFEhqVdmeEcmzy5gZKlJIGRA9KmOpZXKViSTTUEfoGNFS2iXnRfaLiKXaYSQ9CMmBO+sXWcSJMtCicU2pIqwqW5bLZPrDGzXjLnJwrZK2z0PMfocoSdo5MqUtIkKKkqxYq5EMzMKaw2ksoFbwU2m2KmFyRwAdgOEcg/pngeVMfJ/MeUXYqfMxIyxIf5kMwi1iNSdzdIrkT0kFGF3BBJJYONGzPpB9mta5acKBKNEgFScRGHUF3etSG0i1FvRLkkXWK6VLqtwGCsLh89dwhharfKlAoQAohSmbwgYcOepz9IRTb3nEBC8CU5bLh+JcnF5iKE2h8llhoB+cTY6NFLvyeRifEDV1bWTij5awOu/Jsz63CiWy4hnEBrn4ZBVqEnPnCuwWqiUklyoJTQlyWYFhWsOUqCMbH1mvuYH2n/iBycHUlqgZboKV2mm6EDPJsySXqDqYQLTgXgK2y2i4BG8atDSyWBMwD6Sj6JJ2cyRSvJ3JiE2xulsLT2iZj3YKhiL0zVQk0fPQEQhvO3mbMKyKkAMKCm6sPr6uCUgFUicpaQkE95Jmpc1dlCW27NsxUxkEpUVYWruy83yhWx0GA00d8q8K/rdHaekep/Z0+KZNJ+7LQw6mbXyi1E6z54Zx6yx6MYai2K0guYh5cpkmiVvUUAUXzaFVmG3Mz8Q+MOJYlTJaQhE1kFQBISogviyStJav2TA4s7klEtwGLoJ0+0SMSSQ7ONMoiUWtlJp6KTY1zXUkOzEuUpzfeQI7UBiPhzOvHeIKkz8Mua5ZRCSjH9Y1SRXM7Q8jAaZitl2dq7Lip1BEK8BQNZkFKlhsjoRv9Y6tEsnCp6ApFTUgnNtQMuojuWmilECqzp7bo9AB1AqN+hCvhCGcTFkJOEseXHdHtmmE1euBWTaRYhD5HdrRhWuojiRLwKY54VDJw+EwCycSpxUHL7tSOcdotTuCMjkaRWsEAPvPH2isulzUPzD0GVKwwCP2yX9kekSFsxIc0jyCx0eKtyHfCC9WTpqwG4ZZxz+1pNQgDoH86wWu6JmIKTLLu9S76/oxx/R21Kc92a/dV8opylomkCJmJbJzvJ947RNRu9RBaOzFq/7Sj0WP9MWp7I2w5SFeZHumFyaK4oEl2uWksxHJz5wdLtwcHEnSpb1LPBVk7I25IV/V/FqVp4cR+UWSOwdvdJCUIAYv3iSXBdxSnrC9r+h6y++pGFSkpCRXZIHI+IatCX9lXw9Y2iOydqUSpfduS5OMmprVkwQjsbNOa5Y/xf7YanGssOMukYM2eZuT0p8I8RZdod4CUgGiSc9C+dK9W0ePoaOxatZiB+ExcjsUNZvkj/8AUHsgux8ZPo+Rrs0yuyryLxdZ0ES1qJWlYwhKWNQTV6V+HWPrSOxcrWarokR2eyFnHimqHMoHuIXtj9Dg/h8fUuYK7t2fpWCLMiZVRxElufWPraOy1lGcyYeRSfZJi9HZmyfZmnmFj/QIXsiHrkz5hIUQgAjTIj3EKrwURhKSQxcH48Kx9qT2csg/uJiuqvioQB2h7IyJ0gplWdUuYFJwrxJDB6uSonDhJpvA3Q5eZSVUC8bR8o/bZyC7EKIAJzdhq7xxLn4ihCyQmgcklhQU3Uj7wLBYZYqiQlqbXd/GLE2uwoyXZk8u7+EZe34i+H5PgWBRomWpVWcIVXTSLJQMtQStCgo4mBdNGYHIuHxbuYj7wvtNYkf3yPwpWf8AKkxku2M67LZhUu1TJcxIAxolzFApBJZScFak1DGGpyfQlFLNnzJc6WHBI1yBr+UVTLal2QlXm/wg+wWeSibMQtYUgHZmKQsJUkEEHAUYgaVTkXIqKx3JuazFHeG3olFRJEvuZq1JTiLBRTQHCxbQlno8bexpIhwUm2CSL4VLCsKpgcuwSGJO8k08oir+mO4URvZCHNQc2dnGWtRkSIumXRJYlNtlqADh5akE1YgBRdxQh86tlANpsUtIJTPlrUzhIRMBPB8JS/M+UL2Ni4JMb2btKgN3ktC/4TgU33SAUpPNMaIzLNMlpnSFu5KVImOJiCa7QCsK0kk1AakZKxSJCpDU70vsvtlQdgAdCSMqMA7Vh/cvZ+XLlLXNmrTMSoOEJJGEOGTQiYCqhIbC2TFzEpVsqMfgUEWUktLmgFeI4phauejt1J4w5nKu9CCQiUpg4GKY6iBk7cMzvypGVskqaUhRlgJIKgStFUhyWBU5IYuGekDTpoIoQ/MfOM+CnpvHxhlbPpqOzVhLEykYjkMaiHbdiY+cVzezt3pOIy8BJLOqYKsfvDo0ZQWnu2KrXLUG8IDFJITUgeJmYgdHgG3X4oJSEzJbsWKSoEHi5fcNco85ePzN4kzaom0n3Pd8tKldwMlMSqYXLHMKVy0OcYKdaEbS02eWAGqVrLBTEbOJnYimkLJdqmzJie8KiFKFTxYO7coaS7tKUlIHebIDJEyp30duUdfia8cq8km29Gc4tq4nH7c1DZ5Xr84kF/sMzQn/AOGcfVqxI7Ofh+P/AKZcfIaZEqWMpeD/ANaly/8AIoD0ghMxT7M+ange7WnriRi/mgbrHmPjC42aWGJtdpBcTJC9wVLmSz/iStXtBMu9Zw8dnCv/AFTUK9Jgln3hYJgjsTucTwQ+THAvhAIxy50vnKmFPVSApPrBFnvGQskImSyrUYkv1DuIRItZGRI/XCLF2sLDLCVjcsBQ/mETwHzNIC7EV9o6Ct/vGSTJkjwyky+Mta5R/wDrIgiXbMOU+YOCiiYPNSMZ/wAUS/GPmjT4x7b46K/0Yzqbym/VXJmcClcs+YUqO03xM+tKrr3a0LH8ykk+ULgx8kaAEx7iaEKr3QA6lKT/ABy5ifVsJ6Ex1KvmWrKYg8lg+ghcGPkh5iHH3jxQAqP1lCWZfP3gOgPvFf77RrMD8KfBhD4MOSNCEmOwD+s4zv7/AEj69OYPwipfaYDwrfmkfKDhIOaH06xSlEqXKlk6koSSeZZzFZuqzf8AYldEgfKM4vtbMGQSejfGOFdsphp3cvqFfOH65EucTSfuay5qs6A54/Ax0m4LG3/Tp9fnSMirtZNr9HK5YVf7o5Pa+0NQIHEA/ExXrn9FzibI9nLER/YJHVX+6KJt1XcjxSZfHxH/AJjE2i/7RMzmHkKe0ATLRMP1g/Eq93il45dslzj0jczLLdKamXLPDCunJhAi7Rc6XPdI4ApV8Ywy+8OYlnniPvFeCY+Usckk+yhFev8ALJ5msmXjdiVhUtBSRTZQggilGcPzodS5rA0y/wCS5whRBdgRVzvzBHBgIzi5UzSZTikk+pjzupmsw9EIz6gw+CDkxpab2lqlqlBCkoXVSUqVgKqVIYPkPIZQqtE5BQlKZWAj6yQra5pJIf8AhblEMg6zVHokeyY8NkGZXMP4lCvRoOC+ByYsXZmOJSstCkuWbiQICvFLBJ3vpyh6qwpLOVH8SvnFC7ql6gn8SvnC4/gVmfRalpYhZpUAmgIypBSe0c4fXHkmDZl0S9AfMwuFyFLmh3PRoUvGnlqylIv/AKUT/tj+WJA37oX9kef5R5C4L4PkfSL4DKU1OVITTlneY9iRtHZmypM1X2j5mL0zVfaPmY9iRoSzxSzvMVYzvMSJAJFyFneY9VEiRHY3opMeGPIkaISJJmq+0fMx2EhQ2hi51949iRLGL5qyMiRXSkMJWUSJEIo7EdCJEgDo8MV6xIkAiwRzEiQ30B1E3RIkMDpXxisxIkAiJzjlUSJCF2RX6844/P2iRICjxGUSZ84kSEB4qKlxIkJASJEiRQH/2Q==";
            #endregion

            var operations = new List<Operation<InstitutionOfEducationPostApiModel>>() { 
                new Operation<InstitutionOfEducationPostApiModel>("replace", "name", null, "New institution name"),
                new Operation<InstitutionOfEducationPostApiModel>("replace", "institutionOfEducationType", null, InstitutionOfEducationType.College),
                new Operation<InstitutionOfEducationPostApiModel>("replace", "imageApiModel", null, new ImageApiModel{ Photo = base64Photo }),
            };

            var resolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();

            var postRequest = new
            {
                Url = "/api/InstitutionOfEducationAdmin/ModifyInstitution",
                Body = new JsonPatchDocument<InstitutionOfEducationPostApiModel>(operations, resolver)
            };

            //Act
            var response = await _client.PatchAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task AddIoELector_ShouldReturnOk()
        {
            //Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var postRequest = new
            {
                Url = "/api/InstitutionOfEducationAdmin/AddLectorToIoE",
                Body = new EmailApiModel() { UserEmail = "fakeEmail@gmail.com" }
            };

            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            //Assert 
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddIoELector_Input_WrongEmailApiModel(string lectorEmail)
        {
            // Arrange
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var postRequest = new
            {
                Url = "/api/InstitutionOfEducationAdmin/AddLectorToIoE",
                Body = new EmailApiModel() { UserEmail = lectorEmail }
            };

            // Act
            var response = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            // Assert
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest
               || response.StatusCode == System.Net.HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task AddDepartment_ShouldReturnOk()
        {
            //Arrange 
            _adminInputAttribute.SetUserIdByIoEAdminUserIdForHttpContext();
            var postRequest = new
            {
                Url = "/api/InstitutionOfEducationAdmin/AddDepartment",
                Body = new DepartmentApiModel {Name = "FakeName", Description = "FakeDescription"}
            };

            //Act
            var responce = await _client.PostAsync(postRequest.Url, ContentHelper.GetStringContent(postRequest.Body));

            //Assert
            responce.EnsureSuccessStatusCode();
        }

        
    }
}