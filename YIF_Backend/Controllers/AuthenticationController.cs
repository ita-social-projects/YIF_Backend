﻿using Microsoft.AspNetCore.Mvc;
using System.Resources;
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
        private readonly ResourceManager _resourceManager;

        public AuthenticationController(
            IUserService<DbUser> userService,
            ResourceManager resourceManager)
        {
            _userService = userService;
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// Login user into system.
        /// </summary>
        /// <returns>Object with user token and refresh token.</returns>
        /// <response code="200">Returns object with tokens.</response>
        /// <response code="400">If email or password incorrect.</response>
        [HttpPost("LoginUser")]
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> LoginUser([FromBody] LoginApiModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            var result = await _userService.LoginUser(model);
            return Ok(result.Object);
        }

        /// <summary>
        /// Registers a new user and logs in.
        /// </summary>
        /// <returns>Object with user token and refresh token.</returns>
        /// <response code="201">Returns object with tokens.</response>
        /// <response code="400">If model state is not valid.</response>
        /// <response code="409">If email or password incorrect.</response>
        [HttpPost("RegisterUser")]
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(typeof(RedirectResponseApiModel), 409)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterApiModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            var result = await _userService.RegisterUser(model);
            if (!result.Success)
            {
                var actionUrl = Url.Action("Reset", "Users", new { userEmail = model.Email }, protocol: Request.Scheme);
                return Conflict(new RedirectResponseApiModel(actionUrl, result.Message));
            }
            return Created("", result.Object);
        }

        /// <summary>
        /// Refresh tokens.
        /// </summary>
        /// <returns>Object with user token and refresh token.</returns>
        /// <response code="200">Returns object with tokens.</response>
        /// <response code="400">If refresh token request incorrect.</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestApiModel tokenApiModel)
        {
            if (!ModelState.IsValid) 
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            var result = await _userService.RefreshToken(tokenApiModel);
            return Ok(result.Object);
        }


    }
}