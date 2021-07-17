using Microsoft.AspNetCore.Mvc;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

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
        /// Modify Institution Of Education
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPut("ModifyInstitution")]
        public async Task<IActionResult> ModifyInstitution([FromBody] InstitutionOfEducationPostApiModel institutionOfEducationPostApiModel)
        {
            if (institutionOfEducationPostApiModel == null)
                return BadRequest();

            var userId = User.FindFirst("id")?.Value;
            var result = await _ioEAdminService.ModifyInstitution(userId, institutionOfEducationPostApiModel);
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
        /// Get Institution of Education moderators.
        /// </summary>
        /// <returns>List of moderators</returns>
        /// <response code="200">Returns a list of moderators</response>
        /// <response code="403">If user is not Institution of Education admin</response>
        [HttpGet("GetIoEModerators")]
        [ProducesResponseType(typeof(IEnumerable<IoEModeratorsForIoEAdminResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetModeratorsByUserId()
        {
            string userId = User.FindFirst("id").Value;
            var result = await _ioEAdminService.GetIoEModeratorsByUserId(userId);
            return Ok(result.Object);
        }

        /// <summary>
        /// Get Institution Of Education Information
        /// </summary>
        /// <returns>Institution Of Education</returns>
        /// <response code="200">Returns Institution Of Education</response>
        /// <response code="403">If user is not Institution of Education admin</response>
        [HttpGet("GetIoEInfoByUserId")]
        [ProducesResponseType(typeof(IoEInformationResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetIoEInfoByUserId()
        {
            string userId = User.FindFirst("id").Value;
            var result = await _ioEAdminService.GetIoEInfoByUserId(userId);
            return Ok(result.Object);
        }

        /// <summary>
        /// Get specialty description in IoE.
        /// </summary>
        /// <response code="200">Get full description of specialty in IoE</response>
        /// <response code="400">If id is not valid.</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not institution of education admin</response>
        /// <response code="404">Specialty not found</response>
        [HttpGet("Specialty/Description/Get/{specialtyId}")]
        [ProducesResponseType(typeof(SpecialtyToInstitutionOfEducationResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 401)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetSpecialtyDescription(string specialtyId)
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _ioEAdminService.GetSpecialtyToIoEDescription(userId, specialtyId);
            return Ok(result.Object);
        }

        /// <summary>
        /// Soft delete Institution Of Education Moderator
        /// </summary>
        /// <returns>Whether Moderator was deleted or not</returns>
        /// <response code="200">Returns if the moderator has been successfully deleted from institution of education.</response>
        /// <response code="400">If id is not valid.</response>
        /// <response code="404">If Moderator with such Id wasn't found</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("DeleteIoEModerator")]
        public async Task<IActionResult> DeleteIoEModerator(string moderatorId)
        {
            string userId = User.FindFirst("id").Value;
            var result = await _ioEAdminService.DeleteIoEModerator(moderatorId, userId);
            return Ok(result.Object);
        }

        /// <summary>
        /// Ban IoE Moderator (sets its Moderator IsBanned to true or false).
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="404">IoE Moderator wasn't found</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPatch("BanIoEModerator/{Id}")]
        public async Task<IActionResult> BanIoEModerator(string Id)
        {
            string userId = User.FindFirst("id").Value;
            var result = await _ioEAdminService.ChangeBannedStatusOfIoEModerator(Id, userId);
            return Ok(result.Object);
        }

        /// <summary>
        /// Adds new Institution Of Education Moderator
        /// </summary>
        /// <response code="200">Institution Of Education Moderator was succesfully added</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If institutionOfEducation not found</response>
        /// <response code="409">If any issue appeared</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddIoEModerator")]
        public async Task<IActionResult> AddIoEModerator([FromBody] EmailApiModel model)
        {
            string userId = User.FindFirst("id").Value;
            var result = await _ioEAdminService.AddIoEModerator(model.UserEmail, userId, Request);
            return Ok(result.Object);
        }

        /// <summary>
        /// Adds Lector to IoE
        /// </summary>
        /// <response code="200">Lector successfully added to the Institution of Education</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If such user doesn't exist</response>
        /// <response code="409">If email incorrect</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddLectorToIoE")]
        public async Task<IActionResult> AddIoELector(EmailApiModel email)
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _ioEAdminService.AddLectorToIoE(userId, email, Request);
            return Ok(result.Object);
        }

        /// <summary>
        /// Get Institution of Education lectors.
        /// </summary>
        /// <returns>List of lectors</returns>
        /// <response code="200">Returns a list of lectors</response>
        /// <response code="403">If user is not Institution of Education admin</response>
        [HttpGet("GetIoELectors")]
        [ProducesResponseType(typeof(IEnumerable<LectorResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetLectorsByUserId()
        {
            string userId = User.FindFirst("id").Value;
            var result = await _ioEAdminService.GetIoELectorsByUserId(userId);
            return Ok(result.Object);
        }

        /// <summary>
        /// Soft delete Institution Of Education Lector
        /// </summary>
        /// <returns>Whether Lector was deleted or not</returns>
        /// <response code="200">Returns if the lector has been successfully deleted from institution of education.</response>
        /// <response code="400">If id is not valid.</response>
        /// <response code="403">>If user is not Institution of Education admin</response>
        /// <response code="404">If lector with such Id wasn't found</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("DeleteIoELector")]
        public async Task<IActionResult> DeleteIoELector(string lectorId)
        {
            string userId = User.FindFirst("id").Value;
            var result = await _ioEAdminService.DeleteIoELector(lectorId,userId);
            return Ok(result.Object);
        }
    }
}
