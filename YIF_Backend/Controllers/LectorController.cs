using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SendGrid.Helpers.Errors.Model;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "Lector")]
    public class LectorController : ControllerBase
    {
        private readonly ILectorService _lectorService;

        public LectorController(ILectorService lectorService) {
            _lectorService = lectorService;
        }

        /// <summary>
        /// Modify Lector
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="403">If user is not lector</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPatch("ModifyLector")]
        public async Task<IActionResult> ModifyLector([FromBody] JsonPatchDocument<LectorApiModel> lectorApiModel)
        {
            var userId = User.FindFirst("id")?.Value;
            if (lectorApiModel == null)
                throw new BadRequestException();

            var result = await _lectorService.ModifyLector(userId, lectorApiModel);
            return Ok(result.Object);
        }

        /// <summary>
        /// Get all departments.
        /// </summary>
        /// <returns>List of departments</returns>
        /// <response code="200">Returns a list of departments</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not lector</response>
        /// <response code="404">If there are no departments</response>
        [HttpGet("GetAllDepartments")]
        [ProducesResponseType(typeof(IEnumerable<DepartmentApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 401)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _lectorService.GetAllDepartments();
            return Ok(result.Object);
        }
    }
}
