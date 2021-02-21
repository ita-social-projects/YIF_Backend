using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
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
        [HttpGet("All")]
        public async Task<IActionResult> GetAllDirections(string DirectionName,
            string SpecialtyName,
            string UniversityName,
            string UniversityAbbreviation,
            int page = 1, int pageSize = 10)
        {
            var pageModel = new PageApiModel
            {
                Page = page,
                PageSize = pageSize,
                Url = $"{Request?.Scheme}://{Request?.Host}{Request?.Path}"
            };
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                UniversityName = UniversityName,
                UniversityAbbreviation = UniversityAbbreviation
            };

            var directions = await _directionService.GetAllDirectionsByFilter(pageModel, filterModel);
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
        public async Task<IActionResult> GetDirectionNames(
            string DirectionName,
            string SpecialtyName,
            string UniversityName,
            string UniversityAbbreviation)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                UniversityName = UniversityName,
                UniversityAbbreviation = UniversityAbbreviation
            };

            var result = await _directionService.GetDirectionsNamesByFilter(filterModel);
            return Ok(result);
        }
    }
}
