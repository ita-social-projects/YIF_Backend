using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DisciplineController: Controller
    {
        private readonly IDisciplineService _disciplineService;
        
        public DisciplineController(IDisciplineService disciplineService)
        {
            this._disciplineService = disciplineService;
        }

        /// <summary>
        /// Add new discipline
        /// </summary>
        /// <returns>Success message</returns>
        /// <responce code="200">Returns if the discipline has been successfully added</responce>
        /// <responce code="400">Returns if such discipline already exist</responce>
        /// <responce code="401">Returns if user isn't authorized</responce>
        /// <responce code="403">Returns if user isn't IoEAdmin or IoEModerator</responce>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 401)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [Authorize(Roles = "InstitutionOfEducationAdmin, InstitutionOfEducationModerator")]
        [HttpPost("AddDiscipline")]
        public async Task<IActionResult> AddDiscipline([FromBody] DisciplineApiModel discipline)
        {
            var result = await _disciplineService.AddDiscipline(discipline);
            return Ok(result.Object);
        }
    }
}
