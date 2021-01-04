using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService<DbUser> _userService;
        private readonly IEmailService _emailService;

        public AuthenticationController(IUserService<DbUser> userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }

            var result = await _userService.LoginUser(model);

            //await _emailService.SendAsync("stepansmetanskyy@gmail.com", "Sending email is Fun", "<strong>and easy to do anywhere, even with C# it's html content</strong>");

            return result.Response();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }
            var result = await _userService.RegisterUser(model);
            return result.Response();
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequestApiModel tokenApiModel)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }
            var result = await _userService.RefreshToken(tokenApiModel);
            return result.Response();
        }
    }
}