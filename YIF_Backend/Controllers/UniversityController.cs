using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
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
        [ProducesResponseType(500)]
        [HttpGet("GetUniversityByFilter")]
        public async Task<IActionResult> GetUniversityByFilter(string DirectionName, string SpecialityName, string UniversityName)
        {
            var model = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialityName = SpecialityName,
                UniversityName = UniversityName
            };

            //if(model.GetType().GetProperties().Any(x => x.GetValue(model) == null)) // FIX
            //{
            //    return BadRequest(new ResponseApiModel<Object>(400,"Property cannot be null"));
            //}

            var result = await _universityService.GetUniversityByFilter(model);

            return result.Response(200);
        }

        /// <summary>
        /// Get university by id.
        /// </summary>
        /// <returns>Returns university by id</returns>
        /// <response code="200">Returns university</response>
        /// <response code="404">If a university with this id is not found</response>
        [ProducesResponseType(typeof(UniversityResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 500)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversityById(string id)
        {
            UniversityResponseApiModel result = null;
            var userId = User.FindFirst("id")?.Value;
            try
            {
                result = await _universityService.GetUniversityById(id, userId);
            }
            catch (NotFoundException)
            {
                return NotFound(new DescriptionResponseApiModel { Message = "Університету з таким id не існує." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new DescriptionResponseApiModel { Message = e.Message });
            }
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
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 500)]
        [HttpGet]
        public async Task<IActionResult> GetUniversitiesPage(int page = 1, int pageSize = 10)
        {
            var result = new PageResponseApiModel<UniversityResponseApiModel>();
            var userId = User.FindFirst("id")?.Value;
            try
            {
                var url = $"{Request.Scheme}://{Request.Host}{Request.Path}";
                result = await _universityService.GetUniversitiesPage(page, pageSize, url, userId);
            }
            catch (NotFoundException)
            {
                return BadRequest(new DescriptionResponseApiModel { Message = "Університети не було знайдено." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new DescriptionResponseApiModel { Message = e.Message });
            }
            return Ok(result);
        }

        /// <summary>
        /// Get all favorite universities.
        /// </summary>
        /// <returns>Returns all favorite universities</returns>
        /// <response code="200">Returns the page with universities</response>
        [ProducesResponseType(typeof(IEnumerable<UniversityResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 500)]
        [HttpGet("Favorites")]
        [Authorize]
        public async Task<IActionResult> GetFavoriteUniversities()
        {
            IEnumerable<UniversityResponseApiModel> result = null;
            try
            {
                var userId = User.FindFirst("id")?.Value;
                result = await _universityService.GetFavoriteUniversities(userId);
            }
            catch (NotFoundException)
            {
                return NotFound(new DescriptionResponseApiModel { Message = "Немає вибраних університетів." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new DescriptionResponseApiModel { Message = e.Message });
            }
            return Ok(result);
        }
    }
}