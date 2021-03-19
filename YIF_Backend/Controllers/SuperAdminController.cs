using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;
        private readonly ResourceManager _resourceManager;

        public SuperAdminController(
            ISuperAdminService superAdminService,
            ResourceManager resourceManager)
        {
            _superAdminService = superAdminService;
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// Adds InstitutionOfEducation Admin and Moderator.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If institutionOfEducation not found</response>
        /// <response code="409">If email or password incorrect</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddInstitutionOfEducationAdmin")]
        public async Task<IActionResult> AddInstitutionOfEducationAdmin([FromBody] InstitutionOfEducationAdminApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            var result = await _superAdminService.AddInstitutionOfEducationAdmin(model);
            return Created(string.Empty, result.Object);
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
            if (!ModelState.IsValid)
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            var result = await _superAdminService.AddSchoolAdmin(model);
            return Created(string.Empty, result.Object);
        }

        /// <summary>
        /// Delete InstitutionOfEducation admin (sets its asp.net user IsDeleted to true).
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("DeleteInstitutionOfEducationAdmin/{id}")]
        public async Task<IActionResult> DeleteInstitutionOfEducationAdmin(string id)
        {
            var result = await _superAdminService.DeleteInstitutionOfEducationAdmin(id);
            return Ok(result.Object);
        }

        /// <summary>
        /// Disable InstitutionOfEducation admin (sets its InstitutionOfEducation Admin IsBanned to true  or false).
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("DisableInstitutionOfEducationAdmin/{id}")]
        public async Task<IActionResult> DisableInstitutionOfEducationAdmin(string id)
        {
            var result = await _superAdminService.DisableInstitutionOfEducationAdmin(id);
            return Ok(result.Object);
        }

        /// <summary>
        /// Delete School admin(sets its asp.net user IsDeleted to true.
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPut("DeleteSchoolAdmin")]
        public async Task<IActionResult> DeleteSchoolAdmin([FromBody] SchoolUniAdminDeleteApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            var result = await _superAdminService.DeleteSchoolAdmin(model);
            return Ok(result.Object);
        }

        /// <summary>
        /// Adds InstitutionOfEducation and email for admin.
        /// </summary>
        /// <returns></returns>
        /// <response code="201"></response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="409">InstitutionOfEducation Already exists</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(500)]
        [HttpPost("AddInstitutionOfEducationAndAdmin")]
        public async Task<IActionResult> AddInstitutionOfEducationAndAdmin([FromBody] InstitutionOfEducationPostApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));

            ImageBase64Validator validator = new ImageBase64Validator();
            var validResults = validator.Validate(model.ImageApiModel);

            if (!validResults.IsValid) return BadRequest(new DescriptionResponseApiModel(validResults.ToString()));

            var result = await _superAdminService.AddInstitutionOfEducationAndAdmin(model, Request);
            return result.Success ? Ok(result.Object) : (IActionResult)BadRequest(result.Description);
        }


        /// <summary>
        /// Get all UniAdmins.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns a list of users</response>
        /// <response code="404">If there are no users</response>
        [HttpGet("GetAllInstitutionOfEducations")]
        [ProducesResponseType(typeof(IEnumerable<InstitutionOfEducationAdminResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllUniUsersAsync()
        {
            var result = await _superAdminService.GetAllInstitutionOfEducationAdmins();
            return Ok(result.Object);
        }

        /// <summary>
        /// Get all SchoolAdmins.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns a list of users</response>
        /// <response code="404">If there are no users</response>
        [HttpGet("GetAllSchools")]
        [ProducesResponseType(typeof(IEnumerable<InstitutionOfEducationAdminResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllSchoolUsersAsync()
        {
            var result = await _superAdminService.GetAllSchoolAdmins();
            return Ok(result.Object);
        }
    }
}
