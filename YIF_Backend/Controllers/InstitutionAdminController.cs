using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "InstitutionOfEducationAdmin")]
    public class InstitutionAdminController : ControllerBase
    {
        private readonly IInstitutionAdminService _institutionAdminService;

        public InstitutionAdminController(
            IInstitutionAdminService institutionAdminService)
        {
            _institutionAdminService = institutionAdminService;
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
            if(institutionOfEducationPostApiModel.ImageApiModel != null)
            {
                ImageBase64Validator validator = new ImageBase64Validator();
                var validResults = validator.Validate(institutionOfEducationPostApiModel.ImageApiModel);

                if (!validResults.IsValid) return BadRequest(new DescriptionResponseApiModel(validResults.ToString()));
            }

            var userId = User.FindFirst("id")?.Value;
            var result = await _institutionAdminService.ModifyDescriptionOfInstitution(userId, institutionOfEducationPostApiModel);
            return Ok(result.Object);
        }
    }
}
