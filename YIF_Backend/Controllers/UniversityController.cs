using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService<University> _universityService;

        public UniversityController(IUniversityService<University> universityService)
        {
            _universityService = universityService;
        }

        /// <summary>
        /// Get university by filter.
        /// </summary>
        /// <returns>Returns university by filter</returns>
        /// <response code="200">Returns university</response>
        /// <response code="400">If filter is incorrect</response>
        [ProducesResponseType(typeof(IEnumerable<UniversityFilterResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("GetUniversityByFilter")]
        public async Task<IActionResult> GetUniversityByFilter(string DirectionName, string SpecialityName, string UniversityName)
        {
            var model = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialityName = SpecialityName,
                UniversityName = UniversityName
            };

            var result = await _universityService.GetUniversityByFilter(model);

            return Ok(result.Object);
        }

        /// <summary>
        /// Get university by id.
        /// </summary>
        /// <returns>Returns university by id</returns>
        /// <response code="200">Returns university</response>
        /// <response code="404">If a university with this id is not found</response>
        [ProducesResponseType(typeof(UniversityResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversityById(string id)
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _universityService.GetUniversityById(id, userId);
            return Ok(result);
        }

        /// <summary>
        /// Get all universities with pagination.
        /// </summary>
        /// <returns>Returns the page with universities</returns>
        /// <response code="200">Returns the page with universities</response>
        /// <response code="400">If page size or page number is incorrect</response>
        [ProducesResponseType(typeof(PageResponseApiModel<UniversityResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet]
        public async Task<IActionResult> GetUniversitiesPage(int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirst("id")?.Value;
            var url = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var result = await _universityService.GetUniversitiesPage(page, pageSize, url, userId);
            return Ok(result);
        }

        /// <summary>
        /// Get all favorite universities.
        /// </summary>
        /// <returns>Returns all favorite universities</returns>
        /// <response code="200">Returns the page with universities</response>
        [ProducesResponseType(typeof(IEnumerable<UniversityResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Favorites")]
        [Authorize]
        public async Task<IActionResult> GetFavoriteUniversities()
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _universityService.GetFavoriteUniversities(userId);
            return Ok(result);
        }
    }
}