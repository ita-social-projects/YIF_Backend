using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        private readonly ILogger<SchoolController> _logger;

        public SchoolController(
            ISchoolService schoolService,
            ILogger<SchoolController> logger)
        {
            _schoolService = schoolService;
            _logger = logger;
        }
        /// <summary>
        /// Get all SchoolNames.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns a list of school names</response>
        /// <response code="404">If there are no schools</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SchoolOnlyNameResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllSchoolsAsync()
        {
            var result = await _schoolService.GetAllSchoolNames();
            _logger.LogInformation("GetAllSchoolNames");
            return Ok(result.Object);
        }

        /// <summary>
        /// Get all SchoolNames.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns a list of school names</response>
        /// <response code="404">If there are no schools</response>
        [HttpGet("GetAllSchoolNamesAsStringsAsync")]
        [ProducesResponseType(typeof(IEnumerable<SchoolOnlyNameResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllSchoolNamesAsStringsAsync()
        {
            var result = await _schoolService.GetAllSchoolNamesAsStrings();
            _logger.LogInformation("GetAllSchoolNames");
            return Ok(result.Object);
        }
    }
}
