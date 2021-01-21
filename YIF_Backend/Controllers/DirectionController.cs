using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectionController : ControllerBase
    {
        private readonly IDirectionService _directionService;

        public DirectionController(IDirectionService directionService)
        {
            _directionService = directionService;
        }

        /// <summary>
        /// Get all directions.
        /// </summary>
        /// <returns>List of directions</returns>
        /// <response code="200">Return a list of directions</response>
        [ProducesResponseType(typeof(PageResponseApiModel<DirectionResponseApiModel>), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet]
        public async Task<IActionResult> GetAllDirections(int page = 1, int pageSize = 10)
        {
            var url = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var directions = await _directionService.GetAllDirections(page, pageSize, url);
            return Ok(directions);
        }

        /// <summary>
        /// Get all direction names.
        /// </summary>
        /// <returns>List of direction names</returns>
        /// <response code="200">Returns a list of direction names</response>
        /// <response code="404">If there are not directions</response>
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Names")]
        public async Task<IActionResult> GetDirectionNames()
        {
            var result = await _directionService.GetDirectionNames();
            return Ok(result);
        }
    }
}
