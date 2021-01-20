using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Service.Concrete.Services;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

        public UsersController(IUserService<DbUser> userService, ILogger<UsersController> logger, IMapper mapper)
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns a list of users</response>
        /// <response code="404">If there are no users</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsers();
            _logger.LogInformation("Getting all users");
            return Ok(result.Object);
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns a user</response>
        /// <response code="404">If user not found</response>
        /// <param name="id" example="01f75261-2feb-4a34-93fb-ab26bf16cbe7">User ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            Guid guid = Guid.Parse(id);
            var result = await _userService.GetUserById(guid.ToString("D"));
            _logger.LogInformation("Getting a user by Id");
            return Ok(result.Object);
        }

        /// <summary>
        /// Get information about authorized user
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="200">if get information about current user is correct</response>
        /// <response code="400">if get information about current user is incorrect.</response>
        /// <response code="401">If user is unauthorized or token is bad/expired</response>
        /// <response code="404">If user not found</response>
        [ProducesResponseType(typeof(UserProfileApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserFullInfo()
        {
            var userId = User.FindFirst("id")?.Value;
            var profile = await _userService.GetUserProfileInfoById(userId);
            return Ok(profile);
        }

        /// <summary>
        /// Creates user profile
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="200">If the user profile successfully created/updated.</response>
        /// <response code="400">If the request to set the user profile is incorrect.</response>
        /// <response code="401">If user is unauthorized or token is bad/expired.</response>
        [ProducesResponseType(typeof(UserProfileApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("SetCurrentProfile")]
        [Authorize]
        public async Task<IActionResult> SetUserProfile([FromBody] UserProfileApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            var id = User.FindFirst("id")?.Value;
            var result = await _userService.SetUserProfileInfoById(model, id);
            return Ok(result.Object);
        }

        /// <summary>
        /// Change authorized user photo. Size limit 20 MB
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="200">If change user photo request is correct</response>
        /// <response code="400">If change user photo request is incorrect.</response>
        /// <response code="401">If user is unauthorized or token is bad/expired</response>
        [ProducesResponseType(typeof(ImageApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("ChangePhoto")]
        [RequestFormLimits(MultipartBodyLengthLimit = 20971520)]
        [RequestSizeLimit(20971520)]
        [Authorize]
        public async Task<IActionResult> ChangeUserPhoto([FromBody] ImageApiModel model)
        {
            ImageBase64Validator validator = new ImageBase64Validator();
            var validResults = validator.Validate(model);

            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel(validResults.ToString()));

            var id = User.FindFirst("id")?.Value;

            var result = await _userService.ChangeUserPhoto(model, id);

            return Ok(result);
        }

        /// <summary>
        /// Send reset password mail
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When user exist and email have been sended</response>
        /// <response code="400">If current email is not correct</response>
        /// <response code="404">If user doesn`t exist</response>
        [ProducesResponseType(typeof(ResetPasswordByEmailApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByEmailApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            var result = await _userService.ResetPasswordByEmail(model, Request);
            return Ok(result.Object);
        }

        /// <summary>
        /// Change the user password
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Password have been updated</response>
        /// <response code="400">Password have not been updated</response>
        [ProducesResponseType(typeof(ChangePasswordApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            var result = await _userService.ChangeUserPassword(model);
            return Ok(result.Object);
        }

        /// <summary>
        /// Send confirm email mail
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When mail have been sended</response>
        /// <response code="400">When mail not have been sended</response>
        [ProducesResponseType(typeof(SendEmailConfirmApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [HttpPost("SendConfirmEmailMail")]
        public async Task<IActionResult> SendConfirmEmailMail([FromBody] SendEmailConfirmApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userService.SendEmailConfirmMail(model, Request);

            return Ok(result.Object);
        }

        /// <summary>
        /// Confirm user email
        /// </summary>
        /// <returns></returns>
        /// <response code="200">When email have been cofirm</response>
        /// <response code="404">When email not have been confirm</response>
        [ProducesResponseType(typeof(ConfirmEmailApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [HttpPut("ConfirmUserEmail")]
        public async Task<IActionResult> ConfirmUserEmail([FromBody] ConfirmEmailApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userService.ConfirmUserEmail(model);

            return Ok(result.Object);
        }
    }
}