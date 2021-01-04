using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;
namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private readonly ISuperAdminService _superAdminService;
        private readonly ILogger<SuperAdminController> _logger;
        public SuperAdminController(ISuperAdminService superAdminService,
                                    ILogger<SuperAdminController> logger)
        {
            _superAdminService = superAdminService;
            _logger = logger;
        }
        /// <summary>
        /// Registers a new user and logs in.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="409">If email or password incorrect</response>
        [HttpPost("AddSuperAdmin")]
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddSuperAdmin([FromBody] UniversityAdminApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseApiModel<object>(400, "Model state is not valid.").Response();
            }
            var result = await _superAdminService.AddUniversityAdmin(model);
            return result.Response();
        }
    }
}
