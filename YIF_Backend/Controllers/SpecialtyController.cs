using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;
        private readonly ILogger<SpecialtyController> _logger;
        public SpecialtyController(ISpecialtyService specialtyService, ILogger<SpecialtyController> logger)
        {
            _specialtyService = specialtyService;
            _logger = logger;
        }

        /// <summary>
        /// Get all specialties for anonymous.
        /// </summary>
        /// <returns>List of specialties</returns>
        /// <response code="200">Returns a list of specialties</response>
        /// <response code="404">If there are not specialties</response>
        [HttpGet("Anonymous")]
        [ProducesResponseType(typeof(IEnumerable<SpecialtyResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllSpecialtiesAsync(
            string DirectionName,
            string SpecialtyName,
            string InstitutionOfEducationName,
            string InstitutionOfEducationAbbreviation,
            string PaymentForm,
            string EducationForm)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                InstitutionOfEducationName = InstitutionOfEducationName,
                InstitutionOfEducationAbbreviation = InstitutionOfEducationAbbreviation,
                PaymentForm = PaymentForm,
                EducationForm = EducationForm
            };

            var result = await _specialtyService.GetAllSpecialtiesByFilter(filterModel);
            _logger.LogInformation("Getting all specialties");
            return Ok(result);
        }

        /// <summary>
        /// Get all specialties for authorized user with checking favorites.
        /// </summary>
        /// <returns>List of specialties</returns>
        /// <response code="200">Returns a list of specialties</response>
        /// <response code="404">If there are not specialties</response>
        [HttpGet("Authorized")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<SpecialtyResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllSpecialtiesAsyncForAuthorizedUser(
            string DirectionName,
            string SpecialtyName,
            string InstitutionOfEducationName,
            string InstitutionOfEducationAbbreviation,
            string PaymentForm,
            string EducationForm)
        {

            var userId = User.FindFirst("id").Value;

            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                InstitutionOfEducationName = InstitutionOfEducationName,
                InstitutionOfEducationAbbreviation = InstitutionOfEducationAbbreviation,
                PaymentForm = PaymentForm,
                EducationForm = EducationForm
            };

            var result = await _specialtyService.GetAllSpecialtiesByFilterForUser(filterModel, userId);
            _logger.LogInformation("Getting all specialties");
            return Ok(result);
        }

        /// <summary>
        /// Get all specialties names.
        /// </summary>
        /// <returns>List of specialties names</returns>
        /// <response code="200">Returns a list of specialties names</response>
        /// <response code="404">If there are not specialties</response>
        [HttpGet("Names")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllSpecialtiesNamesAsync(
            string DirectionName,
            string SpecialtyName,
            string InstitutionOfEducationName,
            string InstitutionOfEducationAbbreviation)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                InstitutionOfEducationName = InstitutionOfEducationName,
                InstitutionOfEducationAbbreviation = InstitutionOfEducationAbbreviation
            };

            var result = await _specialtyService.GetSpecialtiesNamesByFilter(filterModel); 
            _logger.LogInformation("Getting all specialties names");
            return Ok(result);
        }

        /// <summary>
        /// Get specialty by id.
        /// </summary>
        /// <returns>A specialty</returns>
        /// <response code="200">Returns a specialty</response>
        /// <response code="404">If specialty not found</response>
        /// <param name="id" example="28bf4f2e-6c43-42c0-8391-cbbaba6b5a5a">Specialty ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<SpecialtyResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetSpecialtyAsync(string id)
        {
            Guid guid = Guid.Parse(id);
            var result = await _specialtyService.GetSpecialtyById(guid.ToString("D"));
            _logger.LogInformation("Getting a specialty");
            return Ok(result.Object);
        }
        /// <summary>
        /// Get specialty descriptions by id.
        /// </summary>
        /// <returns>A specialty descriptions</returns>
        /// <response code="200">Returns a specialty descriptions</response>
        /// <response code="404">If specialty descriptions not found</response>
        /// <param name="id" example="28bf4f2e-6c43-42c0-8391-cbbaba6b5a5a">Specialty Id</param>
        [HttpGet("Descriptions/{id}")]
        [ProducesResponseType(typeof(IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetSpecialtyDescriptionsAsync(string id)
        {
            var result = await _specialtyService.GetAllSpecialtyDescriptionsById(id);
            _logger.LogInformation("Getting a specialty descriptions");
            return Ok(result);
        }

        /// <summary>
        /// Add specialty with institution of education to favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="200">Returns if the specialty with institution of education has been successfully added to the favorites list</response>
        /// <response code="400">If id is not valid or specialty with institution of education has already been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("Favorites")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> AddSpecialtyAndInstitutionOfEducationToFavorite([FromBody] SpecialtyAndInstitutionOfEducationToFavoritePostApiModel request)
        {
            var userId = User.FindFirst("id")?.Value;
            await _specialtyService.AddSpecialtyAndInstitutionOfEducationToFavorite(request, userId);
            return Created($"{Request.Scheme}://{Request.Host}{Request.Path}", new { request.SpecialtyId, request.InstitutionOfEducationId });
        }

        /// <summary>
        /// Delete specialty with institution of education from favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="204">Returns if the specialty with institution of education has been successfully deleted from the favorites list</response>
        /// <response code="400">If id is not valid or specialty with institution of education has not been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("Favorites")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> RemoveSpecialtyAndInstitutionOfEducationFromFavorite(string specialtyId, string institutionOfEducationId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _specialtyService.DeleteSpecialtyAndInstitutionOfEducationFromFavorite(specialtyId, institutionOfEducationId, userId);
            return Ok(value: new { specialtyId, institutionOfEducationId });
        }
        /// <summary>
        /// Add specialty to favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="200">Returns if the specialty has been successfully added to the favorites list</response>
        /// <response code="400">If id is not valid or specialty has already been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("Favorites/{specialtyId}")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> AddSpecialtyToFavorite(string specialtyId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _specialtyService.AddSpecialtyToFavorite(specialtyId, userId);
            return Created($"{Request.Scheme}://{Request.Host}{Request.Path}", specialtyId);
        }

        /// <summary>
        /// Delete specialty from favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="204">Returns if the specialty has been successfully deleted from the favorites list</response>
        /// <response code="400">If id is not valid or specialty has not been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("Favorites/{specialtyId}")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> DeleteSpecialtyFromFavorite(string specialtyId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _specialtyService.DeleteSpecialtyFromFavorite(specialtyId, userId);
            return Ok(value: specialtyId);
        }

        /// <summary>
        /// Get all exams names.
        /// </summary>
        /// <returns>List of exams names</returns>
        /// <response code="200">Returns a list of exams names</response>
        [HttpGet("Exams")]
        [ProducesResponseType(typeof(IEnumerable<ExamsResponseApiModel>), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllExamsNames()
        {
            var result = await _specialtyService.GetExams();
            return Ok(result.Object);
        }

        /// <summary>
        /// Get all education forms names.
        /// </summary>
        /// <returns>List of education forms names</returns>
        /// <response code="200">Returns a list of education forms names</response>
        [HttpGet("EducationForms")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllEducationFormsNames()
        {
            var result = await _specialtyService.GetEducationForms();
            return Ok(result.Object);
        }

        /// <summary>
        /// Get all payment forms names.
        /// </summary>
        /// <returns>List of payment forms names</returns>
        /// <response code="200">Returns a list of payment forms names</response>
        [HttpGet("PaymentForms")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllPaymentFormsNames()
        {
            var result = await _specialtyService.GetPaymentForms();
            return Ok(result.Object);
        }
    }
}
