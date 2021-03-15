using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
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
        private readonly ResourceManager _resourceManager;
        private readonly IApplicationDbContext _context;

        public TestController(
            IUserService<DbUser> userService,
            ResourceManager resourceManager,
            IApplicationDbContext context)
        {
            _userService = userService;
            _resourceManager = resourceManager;
            _context = context;
        }

        /// <summary>
        /// TEST authorize:  Get current user id by using token authorize.
        /// </summary>
        /// <returns>List of current user id</returns>
        /// <response code="200">Returns current user id</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        [HttpGet("My_id")]
        [ProducesResponseType(typeof(RolesByTokenResponseApiModel), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
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
        [HttpGet("My_roles")]
        [ProducesResponseType(typeof(RolesByTokenResponseApiModel), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
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
        /// <response code="404">If users not found</response>
        [HttpGet("Admins")]
        [Authorize(Roles = "SuperAdmin,InstitutionOfEducationModerator,SchoolModerator")]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAdminsUsingAuthorizeAsync()
        {
            var id = User.FindFirst("id")?.Value;
            var result = await _userService.GetAdminsUsingAuthorize(id);
            return Ok(result.Object);
        }



        /// <summary>
        /// TEST exception middleware:  Creates server error for test of the exception middleware.
        /// </summary>
        /// <returns>Server error for exception middleware test</returns>
        /// <response code="500">Returns server error for exception middleware test</response>
        [HttpGet("ServerError")]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public void CreateServerError()
        {
            throw new Exception(_resourceManager.GetString("TestServerErrorMessage"));
        }

        /// <summary>
        /// Delete University
        /// /// </summary>
        /// <returns>Success delete</returns>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("DeleteUniversity/{id}")]
        public async Task<IActionResult> DeleteUniversity(string id)
        {
            var inst = _context.Universities.Find(id);
            _context.Universities.Remove(inst);
            _context.SaveChanges();
            return Ok();
        }
    }
}