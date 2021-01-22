using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;
        private readonly ILogger<SuperAdminController> _logger;
        public SuperAdminController(ISuperAdminService superAdminService,
                                    ILogger<SuperAdminController> logger)
        {
            _superAdminService = superAdminService;
            _logger = logger;
        }

        /// <summary>
        /// Adds University Admin and Moderator.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If university not found</response>
        /// <response code="409">If email or password incorrect</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddUniversityAdmin")]
        public async Task<IActionResult> AddUniversityAdmin([FromBody] UniversityAdminApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            var result = await _superAdminService.AddUniversityAdmin(model);
            return Created("", result.Object);
        }

        /// <summary>
        /// Adds School Admin and Moderator returns token.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If school not found</response>
        /// <response code="409">If email or password incorrect</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddSchoolAdmin")]
        public async Task<IActionResult> AddSchoolAdmin([FromBody] SchoolAdminApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            var result = await _superAdminService.AddSchoolAdmin(model);
            return Created("", result.Object);
        }

        /// <summary>
        /// Delete University admin(sets its asp.net user IsDeleted to true.
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Sucesss message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPut("DeleteUniversityAdmin")]
        public async Task<IActionResult> DeleteUniversityAdmin([FromBody] SchoolUniAdminDeleteApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Модель не валідна");
            var result = await _superAdminService.DeleteUniversityAdmin(model);
            return Ok(result.Object);
        }

        /// <summary>
        /// Delete School admin(sets its asp.net user IsDeleted to true.
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Sucesss message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPut("DeleteSchoolAdmin")]
        public async Task<IActionResult> DeleteSchoolAdmin([FromBody] SchoolUniAdminDeleteApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            var result = await _superAdminService.DeleteSchoolAdmin(model);
            return Ok(result.Object);
        }

        /// <summary>
        /// Adds Universty Beta.
        /// </summary>
        /// <returns></returns>
        /// <response code="201"></response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="409">Uni Already exists</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(500)]
        [HttpPost("AddUniversity")]
        public async Task<IActionResult> AddUniversity([FromBody] UniversityPostApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            var result = await _superAdminService.AddUniversity(model);
            return Created("", result.Object);
        }
    }
}
