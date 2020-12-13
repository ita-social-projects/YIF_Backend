using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ServicesInterfaces;
using YIF.Core.Domain.ViewModels.IdentityViewModels;
using YIF.Core.Service;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;

        public UsersController(IUserService<DbUser> userService)
        {
            _userService = userService;
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
                //return BadRequest("Wrong ID length");
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
