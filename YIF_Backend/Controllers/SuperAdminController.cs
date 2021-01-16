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
        /// <response code="409">If email or password incorrect</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(500)]
        [HttpPost("AddUniversityAdmin")]
        public async Task<IActionResult> AddUniversityAdmin([FromBody] UniversityAdminApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Модель не валідна");
            var result = await _superAdminService.AddUniversityAdmin(model);
            if (result.Success) return Created("", result.Object);
            if (result.Message.Contains("немає університету")) return BadRequest(result.Description);
            return Conflict(result.Description);
        }
        /// <summary>
        /// Adds School Admin and Moderator returns token.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="409">If email or password incorrect</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(500)]
        [HttpPost("AddSchoolAdmin")]
        public async Task<IActionResult> AddSchoolAdmin([FromBody] SchoolAdminApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Модель не валідна");
            var result = await _superAdminService.AddSchoolAdmin(model);
            if (result.Success) return Created("", result.Object);
            if (result.Message.Contains("немає школи")) return BadRequest(result.Description);
            return Conflict(result.Description);
        }

        /// <summary>
        /// Delete University admin(sets its asp.net user IsDeleted to true.
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Sucesss message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(500)]
        [HttpPost("DeleteUniversityAdmin")]
        public async Task<IActionResult> DeleteUniversityAdmin([FromBody] SchoolUniAdminDeleteApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Модель не валідна");
            var result = await _superAdminService.DeleteUniversityAdmin(model);
            return result.Success ? Ok(result.Object) : (IActionResult)NotFound(result.Description);
        }

        /// <summary>
        /// Delete School admin(sets its asp.net user IsDeleted to true.
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="201">Sucesss message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(500)]
        [HttpPost("DeleteSchoolAdmin")]
        public async Task<IActionResult> DeleteSchoolAdmin([FromBody] SchoolUniAdminDeleteApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Модель не валідна");
            var result = await _superAdminService.DeleteSchoolAdmin(model);
            return result.Success ? Ok(result.Object) : (IActionResult)NotFound(result.Description);
        }
    }
}
