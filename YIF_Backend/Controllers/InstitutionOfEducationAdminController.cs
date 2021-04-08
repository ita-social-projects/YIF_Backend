using Microsoft.AspNetCore.Mvc;
using System.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;


namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "InstitutionOfEducationAdmin")]
    public class InstitutionOfEducationAdminController : ControllerBase
    {
        private readonly IIoEAdminService _ioEAdminService;
        private readonly ResourceManager _resourceManager;

        public InstitutionOfEducationAdminController(
            IIoEAdminService ioEAdminService, 
            ResourceManager resourceManager)
        {
            _ioEAdminService = ioEAdminService;
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// Modify description of Institution
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("ModifyDescriptionOfInstitution")]
        public async Task<IActionResult> ModifyDescriptionOfInstitution([FromBody] InstitutionOfEducationPostApiModel institutionOfEducationPostApiModel)
        {
            if (institutionOfEducationPostApiModel.ImageApiModel != null)
            {
                ImageBase64Validator validator = new ImageBase64Validator();
                var validResults = validator.Validate(institutionOfEducationPostApiModel.ImageApiModel);

                if (!validResults.IsValid) return BadRequest(new DescriptionResponseApiModel(validResults.ToString()));
            }

            var userId = User.FindFirst("id")?.Value;
            var result = await _ioEAdminService.ModifyDescriptionOfInstitution(userId, institutionOfEducationPostApiModel);
            return Ok(result.Object);
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
            var result = await _ioEAdminService.AddSpecialtyToIoe(model);
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
            await _ioEAdminService.DeleteSpecialtyToIoe(model);
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
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> UpdateSpecialtyDescription([FromBody] SpecialtyDescriptionUpdateApiModel specialtyDescriptionUpdateApiModel)
        {
            var result = await _ioEAdminService.UpdateSpecialtyDescription(specialtyDescriptionUpdateApiModel);
            return Ok(result.Object);
        }

        /// <summary>
        /// Get all directions and specialties by admin id
        /// </summary>
        /// <response code="200">Get all directions and specialties in institution of education</response>
        /// <response code="400">If id is not valid.</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not institution of education admin or moderator.</response>
        [ProducesResponseType(typeof(IEnumerable<DirectionToIoEResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("GetAllDirectionsAndSpecialtiesInIoE")]
        public async Task<IActionResult> GetAllDirectionsAndSpecialtiesInIoE(string adminId)
        {
            var result = await _ioEAdminService.GetAllDirectionsAndSpecialitiesOfAdmin(adminId);
            return Ok(result);
        }
    }
}
