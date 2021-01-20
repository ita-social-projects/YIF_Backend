using Microsoft.AspNetCore.Mvc;
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
        [ProducesResponseType(typeof(DirectionsResponseApiModel), 200)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("All")]
        public async Task<IActionResult> GetAllDirections(int page = 1, int pageSize = 10)
        {
            var url = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var directions = await _directionService.GetAllDirections(page, pageSize, url);
            return Ok(new DirectionsResponseApiModel { Directions = directions });
        }
    }
}
