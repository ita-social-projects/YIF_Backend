using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.ResultApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;

        public AuthenticationController(IUserService<DbUser> userService)
        {
            _userService = userService;
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<LoginResultApiModel> { StatusCode = 400, Message = "Model state is not valid." }.Response();
            }
            var result = await _userService.LoginUser(model);
            return result.Response();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<LoginResultApiModel> { StatusCode = 400, Message = "Model state is not valid." }.Response();
            }
            var result = await _userService.RegisterUser(model);
            return result.Response();
        }
    }
}