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
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsers();
            return result.Response();
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Returns a user</response>
        /// <param name="id" example="01f75261-2feb-4a34-93fb-ab26bf16cbe7">User ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<UserApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUserAsync(string id)
        {
            try
            {
                Guid guid = Guid.Parse(id);
                var result = await _userService.GetUserById(guid.ToString("D"));
                _logger.LogInformation("Trying to get a user");
                return result.Response();

            }
            catch (ArgumentNullException)
            {
                _logger.LogError("Null user is not allowed");
                return new ResponseApiModel<object> { StatusCode = 400, Message = "The string to be parsed is null." }.Response();
            }
            catch (FormatException)
            {
                _logger.LogError("There is a problem with format");
                return new ResponseApiModel<object> { StatusCode = 400, Message = $"Bad format:  {id}" }.Response();
            }
        }

        /// <summary>
        /// Get information about authorized user
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="200">if get information about current user is correct</response>
        /// <response code="400">if get information about current user is incorrect.</response>
        /// <response code="401">If user is unauthorized or token is bad/expired</response>
        [ProducesResponseType(typeof(UserProfileApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        [HttpGet("Current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserFullInfo()
        {
            var userId = User.FindFirst("id")?.Value;
            var userDto = await _userService.GetUserProfileInfoById(userId);
            if (userDto == null)
                return BadRequest(new DescriptionResponseApiModel { Message = "Зазначеного юзера не існує." });
            var profile = _mapper.Map<UserProfileApiModel>(userDto);
            return Ok(profile);
        }

        /// <summary>
        /// Creates user profile
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="201">If the user profile successfully created/updated.</response>
        /// <response code="400">If the request to set the user profile is incorrect.</response>
        /// <response code="401">If user is unauthorized or token is bad/expired.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        [HttpPost("SetCurrentProfile")]
        [Authorize]
        public async Task<IActionResult> SetUserProfile([FromBody] UserProfileApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new DescriptionResponseApiModel("Модель не валідна."));
            }
            var id = User.FindFirst("id")?.Value;
            var result = await _userService.SetUserProfileInfoById(model, id);
            return result.Success ? Ok() : (IActionResult)BadRequest(new DescriptionResponseApiModel { Message = "Профіль користувача не встановлено." });
        }

        /// <summary>
        /// Change User Photo. Size limit 10 MB
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="200">If change user photo request is correct</response>
        /// <response code="400">If change user photo request is incorrect.</response>
        /// <response code="401">If user is unauthorized or token is bad/expired</response>
        [ProducesResponseType(typeof(ImageApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        [HttpPost("ChangePhoto")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        [Authorize]
        public async Task<IActionResult> ChangeUserPhoto([FromBody] ImageApiModel model)
        {
            ImageBase64Validator validator = new ImageBase64Validator();
            var validResults = validator.Validate(model);

            if (!validResults.IsValid)
            {
                return BadRequest(new DescriptionResponseApiModel { Message = validResults.ToString() });
            }

            var id = User.FindFirst("id")?.Value;

            var result = await _userService.ChangeUserPhoto(model, id);

            if (result != null)
                return Ok(new ImageApiModel { Photo = result.Photo });
            else
                return BadRequest(new DescriptionResponseApiModel { Message = "Фото не змінено." });
        }
    }
}