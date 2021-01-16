using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    // =========================   For test authorize endpoint:   =========================

    /// <summary>
    /// TEST authorize:  Some methods for test with using token authorize.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;

        public TestController(IUserService<DbUser> userService)
        {
            _userService = userService;
        }



        /// <summary>
        /// TEST authorize:  Get current user id by using token authorize.
        /// </summary>
        /// <returns>List of current user id</returns>
        /// <response code="200">Returns current user id</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        [HttpGet("my_id")]
        [ProducesResponseType(typeof(RolesByTokenResponseApiModel), 200)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserIdUsingAuthorizeAsync()
        {
            var result = new ResponseApiModel<IdByTokenResponseApiModel>();
            result.Object = new IdByTokenResponseApiModel("Valid");

            var id = User.FindFirst("id")?.Value;
            result.Object.Id = id;

            return await Task.FromResult(Ok(result.Object));
        }



        /// <summary>
        /// TEST authorize:  Get current user roles by using token authorize.
        /// </summary>
        /// <returns>List of current user roles</returns>
        /// <response code="200">Returns current user roles</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        [HttpGet("my_roles")]
        [ProducesResponseType(typeof(RolesByTokenResponseApiModel), 200)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserRolesUsingAuthorizeAsync()
        {
            var id = User.FindFirst("id")?.Value;
            var result = await _userService.GetCurrentUserRolesUsingAuthorize(id);
            return Ok(result.Object);
        }



        /// <summary>
        /// TEST authorize:  Get all admins of the similar institution as a current user by using token authorize.
        /// </summary>
        /// <returns>List of admins of the similar institution as a current user</returns>
        /// <response code="200">Returns list of admins of the similar institution</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user doesn't have enough rights</response>
        /// <response code="404">If database is empty or current user not found</response>
        [HttpGet("admins")]
        [Authorize(Roles = "SuperAdmin,UniversityModerator,SchoolModerator")]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAdminsUsingAuthorizeAsync()
        {
            var id = User.FindFirst("id")?.Value;
            var result = await _userService.GetAdminsUsingAuthorize(id);
            return result.Success ? Ok(result.Object) : (IActionResult)NotFound(result.Description);
        }
    }
}
