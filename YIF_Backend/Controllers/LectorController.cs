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
        /// <response code="404">If there are no departments</response>
        [HttpGet("GetAllDepartments")]
        [ProducesResponseType(typeof(IEnumerable<DepartmentApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllDepartments()
        {
            var result = await _lectorService.GetAllDepartments();
            return Ok(result.Object);
        }
    }
}
