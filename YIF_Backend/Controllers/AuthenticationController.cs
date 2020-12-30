using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;

        public AuthenticationController(IUserService<DbUser> userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login user into system.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="200">Returns object with tokens</response>
        /// <response code="400">If email or password incorrect</response>
        [HttpPost("LoginUser")]
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> LoginUser([FromBody] LoginApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }
            var result = await _userService.LoginUser(model);
            return result.Response();
        }

        /// <summary>
        /// Registers a new user and logs in.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="409">If email or password incorrect</response>
        [HttpPost("RegisterUser")]
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }
            var result = await _userService.RegisterUser(model);
            return result.Response();
        }

        /// <summary>
        /// Refresh tokens.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="200">Returns object with tokens</response>
        /// <response code="400">If refresh token incorrect</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestApiModel tokenApiModel)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }
            var result = await _userService.RefreshToken(tokenApiModel);
            return result.Response();
        }







        // For test authorize endpoint

        /// <summary>
        /// Get current user roles by using token authorize.
        /// </summary>
        /// <returns>List of current user roles</returns>
        /// <response code="200">Returns current user roles</response>
        [HttpGet("my_roles")]
        [ProducesResponseType(typeof(RolesByTokenResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        [Authorize]
        public async Task<IActionResult> GetRolesUsingAuthorizeAsync()
        {
            var id = User.FindFirst("id")?.Value;

            var result = await _userService.GetRoles(id);
            return result.Response();
        }
    }
}