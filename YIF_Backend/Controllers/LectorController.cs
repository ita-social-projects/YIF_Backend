using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public LectorController(ILectorService lectorService)
        {
            _lectorService = lectorService;
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

        /// <summary>
        /// Get all disciplines.
        /// </summary>
        /// <returns>List of disciplines</returns>
        /// <response code="200">Returns a list of disciplines</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not lector</response>
        /// <response code="404">If there are no disciplines</response>
        [HttpGet("GetAllDisciplines")]
        [ProducesResponseType(typeof(IEnumerable<DisciplinePostApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 401)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllDiscipliness()
        {
            var result = await _lectorService.GetAllDisciplines();
            return Ok(result.Object);
        }
    }
}
