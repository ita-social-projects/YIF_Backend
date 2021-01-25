using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetUniversitiesPage(
            string DirectionName, 
            string SpecialityName, 
            string UniversityName,
            string UniversityAbbreviation,
            int page = 1, 
            int pageSize = 10)
        {
            var userId = User.FindFirst("id")?.Value;

            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialityName = SpecialityName,
                UniversityName = UniversityName,
                UniversityAbbreviation = UniversityAbbreviation                
            };

            var pageModel = new PageApiModel
            {
                Page = page,
                PageSize = pageSize,
                Url = $"{Request.Scheme}://{Request.Host}{Request.Path}"
            };

            var result = await _universityService.GetUniversitiesPage(filterModel, pageModel, userId);
            return Ok(result);
        }

        /// <summary>
        /// Get all favorite universities.
        /// </summary>
        /// <returns>Returns all favorite universities</returns>
        /// <response code="200">Returns the page with universities</response>
        /// <response code="404">If user doesn't have favorite universites</response>
        [ProducesResponseType(typeof(IEnumerable<UniversityResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Favorites")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> GetFavoriteUniversities()
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _universityService.GetFavoriteUniversities(userId);
            return Ok(result);
        }

        /// <summary>
        /// Add university to favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="201">Returns if the university has been successfully added to the favorites list</response>
        /// <response code="400">If id is not valid or university has already been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("Favorites/{universityId}")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> AddUniversityToFavorite(string universityId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _universityService.AddUniversityToFavorite(universityId, userId);
            return Created($"{Request.Scheme}://{Request.Host}{Request.Path}", null);
        }

        /// <summary>
        /// Delete university from favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="204">Returns if the university has been successfully deleted from the favorites list</response>
        /// <response code="400">If id is not valid or university has not been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("Favorites/{universityId}")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> DeleteUniversityFromFavorite(string universityId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _universityService.DeleteUniversityFromFavorite(universityId, userId);
            return NoContent();
        }


        /// <summary>
        /// Get all univesity abbreviations.
        /// </summary>
        /// <returns>List of univesity abbreviations</returns>
        /// <response code="200">Returns a list of univesity abbreviations</response>
        /// <response code="404">If there are not univesities</response>
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Abbreviations")]
        public async Task<IActionResult> GetUniversityAbbreviations(
            string DirectionName,
            string SpecialityName,
            string UniversityName,
            string UniversityAbbreviation)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialityName = SpecialityName,
                UniversityName = UniversityName,
                UniversityAbbreviation = UniversityAbbreviation
            };

            var result = await _universityService.GetUniversityAbbreviations(filterModel);
            return Ok(result);
        }
    }
}