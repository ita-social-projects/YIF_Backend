﻿using Microsoft.AspNetCore.Authorization;
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
        public UsersController(IUserService<DbUser> userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
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
        /// Change User Photo. Size limit 10 мб
        /// </summary>
        /// <returns>Status code</returns>
        /// <response code="200">if change user photo request correct</response>
        /// <response code="400">If change user photo request incorrect.</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(OkResult), 200)]
        [ProducesResponseType(typeof(ResponseErrorApiModel), 400)]
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
                return BadRequest(new ResponseErrorApiModel { Message = validResults.ToString() });
            }

            var id = User.FindFirst("id")?.Value;

            var result = await _userService.ChangeUserPhoto(model, id);

            if (result)
                return Ok();
            else
                return BadRequest();
        }
    }
}