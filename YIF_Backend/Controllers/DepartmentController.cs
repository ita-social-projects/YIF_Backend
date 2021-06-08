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
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Add new department
        /// </summary>
        /// <returns>Success message</returns>
        /// <responce code="200">Returns if the department has been successfully added</responce>
        /// <responce code="400">Returns if such department already exist</responce>
        /// <responce code="401">Returns if user isn't authorized</responce>
        /// <responce code="403">Returns if user isn't IoEAdmin or IoEModerator</responce>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 401)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [Authorize(Roles = "InstitutionOfEducationAdmin, InstitutionOfEducationModerator")]
        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentApiModel department)
        {
            var result = await _departmentService.AddDepartment(department);
            return Ok(result.Object);
        }
    }
}
