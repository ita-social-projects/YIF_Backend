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
        /// Adds Institution Of Education Admin.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If institutionOfEducation not found</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddInstitutionOfEducationAdmin")]
        public async Task<IActionResult> AddInstitutionOfEducationAdmin([FromBody] InstitutionOfEducationAdminApiModel model)
        {
            var result = await _superAdminService.AddInstitutionOfEducationAdmin(model.InstitutionOfEducationId, model.AdminEmail, Request);
            return Ok(result.Object);
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
        [HttpPatch("DisableInstitutionOfEducationAdmin/{id}")]
        public async Task<IActionResult> DisableInstitutionOfEducationAdmin(string id)
        {
            var result = await _superAdminService.DisableInstitutionOfEducationAdmin(id);
            return Ok(result.Object);
        }

        /// <summary>
        /// Delete InstitutionOfEducation (sets its InstitutionOfEducation IsDeleted to true).
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="404">Not found message</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("DeleteInstitutionOfEducation/{id}")]
        public async Task<IActionResult> DeleteInstitutionOfEducation(string id)
        {
            var result = await _superAdminService.DeleteInstitutionOfEducation(id);
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
        public async Task<IActionResult> AddInstitutionOfEducationAndAdmin([FromBody] InstitutionOfEducationCreatePostApiModel model)
        {
            ImageBase64Validator validator = new ImageBase64Validator();
            var validResults = validator.Validate(model.ImageApiModel);

            if (!validResults.IsValid) return BadRequest(new DescriptionResponseApiModel(validResults.ToString()));

            var result = await _superAdminService.AddInstitutionOfEducationAndAdmin(model, Request);
            return result.Success ? Ok(result.Object) : (IActionResult)BadRequest(result.Description);
        }

        /// <summary>
        /// Get all admins.
        /// </summary>
        /// <returns>List of users and institution to which he belon</returns>
        /// <response code="200">Returns a list of users</response>
        /// <response code="404">If there are no users</response>
        [HttpGet("GetAllInstitutionOfEducationsAdmins")]
        [ProducesResponseType(typeof(IEnumerable<InstitutionOfEducationAdminResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllInstitutionOfEducationsAdmins(
            bool? UserName = null,
            bool? Email = null,
            bool? InstitutionOfEducationName = null,
            bool? IsBanned = null,
            int page = 1,
            int pageSize = 10)
        {
            var sortingModel = new InstitutionOfEducationAdminSortingModel
            {
                UserName = UserName,
                Email = Email,
                InstitutionOfEducationName = InstitutionOfEducationName,
                IsBanned = IsBanned
            };

            var pageModel = new PageApiModel
            {
                Page = page,
                PageSize = pageSize,
                Url = $"{Request.Scheme}://{Request.Host}{Request.Path}"
            };

            var result = await _superAdminService.GetAllInstitutionOfEducationAdmins(sortingModel, pageModel);
            return Ok(result);
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

        /// <summary>
        /// Add new specialty.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Specialty added</response>
        /// <response code="400">If request model isn't valid </response>
        /// <response code="403">If user is not super admin</response>
        [HttpPost("AddSpecialty")]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> AddSpecialtyToTheListOfAllSpecialties([FromBody] SpecialtyPostApiModel specialityPostApiModel)
        {
            var result = await _superAdminService.AddSpecialtyToTheListOfAllSpecialties(specialityPostApiModel);
            return Ok(result.Object);
        }

        /// <summary>
        /// Update specialty.
        /// </summary>
        /// <returns>Message</returns>
        /// <response code="200">If specialty description successfully updated</response>
        /// <response code="400">If request model isn't valid </response>
        [HttpPut("UpdateSpecialty")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> UpdateSpecialtyById([FromBody] SpecialtyPutApiModel model)
        {
            var result = await _superAdminService.UpdateSpecialtyById(model);
            return Ok(result.Object);
        }

        /// <summary>
        /// Get Institution of Education moderators by Institution of Education id.
        /// </summary>
        /// <returns>List of moderators</returns>
        /// <response code="200">Returns a list of moderators</response>
        /// <response code="403">If user is not super admin</response>
        [HttpGet("GetIoEModeratorsById")]
        [ProducesResponseType(typeof(IEnumerable<IoEModeratorsForSuperAdminResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetModeratorsByIoEId(string ioEId)
        {
            var result = await _superAdminService.GetIoEModeratorsByIoEId(ioEId);
            return Ok(result.Object);
        }

        /// <summary>
        /// Ban InstitutionOfEducation (sets its InstitutionOfEducation IsBanned to true  or false).
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">Success message</response>
        /// <response code="404">Institution Of Education wasn't found</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPatch("BanInstitutionOfEducation/{id}")]
        public async Task<IActionResult> BanInstituionOfEducation(string id)
        {
            var result = await _superAdminService.ChangeBannedStatusOfIoE(id);
            return Ok(result.Object);
        }

        /// <summary>
        /// Add new IoE Admin from IoE Moderators.
        /// </summary>
        /// <returns>Ok message</returns>
        /// <response code="200">IoE Admin added</response>
        /// <response code="400">If request model isn't valid</response>
        /// <response code="403">If user is not super admin</response>
        [HttpPut("ChooseIoEAdminFromModerators")]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> ChooseIoEAdminFromModerators([FromBody] IoEAdminAddFromModeratorsApiModel ioEAdminAddFromModeratorsApiModel)
        {
            var result = await _superAdminService.ChooseIoEAdminFromModerators(ioEAdminAddFromModeratorsApiModel);
            return Ok(result.Object);
        }
    }
}
