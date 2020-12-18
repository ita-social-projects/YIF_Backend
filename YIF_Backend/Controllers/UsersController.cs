using Microsoft.AspNetCore.Mvc;
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

        public UsersController(IUserService<DbUser> userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
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
                return ReturnResult(result.Success, result.Object, result.Message);
            }
            catch (ArgumentNullException)
            {
                return BadRequest("The string to be parsed is null.");
            }
            catch (FormatException)
            {
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
