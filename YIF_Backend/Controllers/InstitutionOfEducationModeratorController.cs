using Microsoft.AspNetCore.Mvc;
using System.Resources;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "InstitutionOfEducationModerator")]
    public class InstitutionOfEducationModeratorController : Controller
    {
        private readonly IIoEModeratorService _ioEModeratorService;
        private readonly ResourceManager _resourceManager;

        public InstitutionOfEducationModeratorController(
            IIoEModeratorService ioEModeratorService,
            ResourceManager resourceManager)
        {
            _ioEModeratorService = ioEModeratorService;
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// Adds Specialty to the Institution of Education.
        /// </summary>
        /// <response code="200">Specialty successfully added to the Institution of Education</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If specialty not found</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddSpecialtyToInstitutionOfEducation")]
        public async Task<IActionResult> AddSpecialtyToIoE([FromBody] SpecialtyToInstitutionOfEducationPostApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            await _ioEModeratorService.AddSpecialtyToIoe(model);
            return Ok();
        }

        /// <summary>
        /// Temporary delete specialty from institution of education.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="204">Returns if the specialty has been successfully deleted from institution of education.</response>
        /// <response code="400">If id is not valid.</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not institution of education admin or moderator.</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 401)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPatch("DeleteSpecialtyFromInstitutionOfEducation")]
        public async Task<IActionResult> DeleteSpecialtyFromIoE([FromBody] SpecialtyToInstitutionOfEducationPostApiModel model)
        {
            await _ioEModeratorService.DeleteSpecialtyToIoe(model);
            return NoContent();
        }

        /// <summary>
        /// Update specialty description in IoE.
        /// </summary>
        /// <returns>Message</returns>
        /// <response code="200">If specialty description successfully updated</response>
        /// <response code="400">If request model isn't valid </response>
        [HttpPut("Specialty/Description/Update")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> UpdateSpecialtyDescription([FromBody] SpecialtyDescriptionUpdateApiModel specialtyDescriptionUpdateApiModel)
        {
            var result = await _ioEModeratorService.UpdateSpecialtyDescription(specialtyDescriptionUpdateApiModel);
            return Ok(result.Object);
        }

        /// <summary>
        /// Get all directions and specialties of moderator
        /// </summary>
        /// <response code="200">Get all directions and specialties in institution of education</response>
        /// <response code="404">If there are no specialities</response>
        [ProducesResponseType(typeof(IEnumerable<DirectionToIoEResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("GetAllDirectionsAndSpecialtiesInIoE")]
        public async Task<IActionResult> GetAllDirectionsAndSpecialtiesInIoE()
        {
            var moderatorId = User.FindFirst("id")?.Value;
            var result = await _ioEModeratorService.GetAllDirectionsAndSpecialitiesOfModerator(moderatorId);
            return Ok(result);
        }
    }
}
