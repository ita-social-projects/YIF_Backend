using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMyMessageSender _messageSender;
        public UsersController(IUserService<DbUser> userService, ILogger<UsersController> logger,
            IMyMessageSender messageSender)
        {
            _userService = userService;
            _logger = logger;
            _messageSender = messageSender;
        }

        [HttpGet("azure")]
        public async Task<string> SendMessage([FromBody] BaseEntityApiModel model)
        {
            await _messageSender.Send(model.Id);
            return model.Id;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var result = await _userService.GetAllUsers();
            return result.Response();
        }

        [HttpGet("{id}")]
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
    }
}
