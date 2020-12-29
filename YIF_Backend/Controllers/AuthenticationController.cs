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
        private readonly IRecaptchaService _recaptcha;

        public AuthenticationController(IUserService<DbUser> userService, IRecaptchaService recaptcha)
        {
            _userService = userService;
            _recaptcha = recaptcha;
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginApiModel model)
        {
            if (!_recaptcha.IsValid(model.RecaptchaToken))
            {
                return new ResponseApiModel<object>(400, "Роботи атакують!").Response();
            }

            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }
            var result = await _userService.LoginUser(model);
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