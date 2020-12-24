using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.ApiModels.ResultApiModels;

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
            var result = new ResponseApiModel<LoginResultApiModel>();
            if (!ModelState.IsValid)
            {
                result.Set(false, "Model state is not valid.");
            }
            else
            {
                result = await _userService.LoginUser(model);
            }
            return result.Response();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterApiModel model)
        {
            var result = new ResponseApiModel<LoginResultApiModel>();
            if (!ModelState.IsValid)
            {
                result.Set(false, "Model state is not valid.");
            }
            else
            {
                result = await _userService.RegisterUser(model);
            }
            return result.Response();
        }
    }
}