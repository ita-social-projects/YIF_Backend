using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserService<DbUser> userService, IJwtService jwtService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsers();
            return ReturnResult(result.Success, result.Object, result.Message);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserAsync(string id)
        {
            try
            {
                Guid guid = Guid.Parse(id);
                var result = await _userService.GetUserById(guid.ToString("D"));
                _logger.LogInformation("Trying to get a user");
                return ReturnResult(result.Success, result.Object, result.Message);
                
            }
            catch (ArgumentNullException)
            {
                _logger.LogError("Null user is not allowed");
                return BadRequest("The string to be parsed is null.");
            }
            catch (FormatException)
            {
                _logger.LogError("There is a problem with format");
                return BadRequest($"Bad format:  {id}");
            }
        }

        private ActionResult ReturnResult(bool Success, object Object, string Message = null)
        {
            if (Success)
            {
                return Ok(Object);
            }
            return NotFound(Message);
        }
    }
}
