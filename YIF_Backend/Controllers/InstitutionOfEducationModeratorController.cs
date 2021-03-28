using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "InstitutionOfEducationModerator")]
    public class InstitutionOfEducationModeratorController : Controller
    {
        private readonly IIoEModeratorService _ioEModeratorService;
        private readonly ResourceManager _resourceManager;

        public InstitutionOfEducationModeratorController(
            IIoEModeratorService ioEModeratorService,
            ResourceManager resourceManager)
        {
            _ioEModeratorService = ioEModeratorService;
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// Adds Specialty to the Institution of Education.
        /// </summary>
        /// <returns>Object with user token and refresh token</returns>
        /// <response code="201">Returns object with tokens</response>
        /// <response code="400">If model state is not valid</response>
        /// <response code="404">If specialty not found</response>
        [ProducesResponseType(typeof(AuthenticateResponseApiModel), 201)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("AddSpecialtyToInstitutionOfEducation")]
        public async Task<IActionResult> AddSpecialtyToIoE([FromBody] SpecialtyToInstitutionOfEducationPostApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new DescriptionResponseApiModel(_resourceManager.GetString("ModelIsInvalid")));
            var result = await _ioEModeratorService.AddSpecialtyToIoe(model);
            return Created(string.Empty, result.Object);
        }
    }
}
