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
        /// Get all specialties.
        /// </summary>
        /// <returns>List of specialties</returns>
        /// <response code="200">Returns a list of specialties</response>
        /// <response code="404">If there are not specialties</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<SpecialtyResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllSpecialtiesAsync(
            string DirectionName,
            string SpecialtyName,
            string UniversityName,
            string UniversityAbbreviation,
            string PaymentForm,
            string EducationForm)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                UniversityName = UniversityName,
                UniversityAbbreviation = UniversityAbbreviation,
                PaymentForm = PaymentForm,
                EducationForm = EducationForm
            };

            var result = await _specialtyService.GetAllSpecialtiesByFilter(filterModel);
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
            string UniversityName,
            string UniversityAbbreviation)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                UniversityName = UniversityName,
                UniversityAbbreviation = UniversityAbbreviation
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
        [ProducesResponseType(typeof(IEnumerable<SpecialtyToUniversityResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetSpecialtyDescrriptionsAsync(string id)
        {
            var result = await _specialtyService.GetAllSpecialtyDescriptionsById(id);
            _logger.LogInformation("Getting a specialty descriptions");
            return Ok(result.Object);
        }
    }
}
