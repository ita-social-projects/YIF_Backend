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
    public class InstitutionOfEducationController : ControllerBase
    {
        private readonly IInstitutionOfEducationService<InstitutionOfEducation> _institutionOfEducationService;

        public InstitutionOfEducationController(IInstitutionOfEducationService<InstitutionOfEducation> institutionOfEducationService)
        {
            _institutionOfEducationService = institutionOfEducationService;
        }

        /// <summary>
        /// Get institutionOfEducation by id.
        /// </summary>
        /// <returns>Returns institutionOfEducation by id</returns>
        /// <response code="200">Returns institutionOfEducation</response>
        /// <response code="404">If a institutionOfEducation with this id is not found</response>
        [ProducesResponseType(typeof(InstitutionOfEducationResponseApiModel), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstitutionOfEducationById(string id)
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _institutionOfEducationService.GetInstitutionOfEducationById(id, Request, userId);
            return Ok(result);
        }

        /// <summary>
        /// Get all institutionOfEducations with pagination for anonymous user, without checking of its favorites.
        /// </summary>
        /// <returns>Returns the page with institutionOfEducations</returns>
        /// <response code="200">Returns the page with institutionOfEducations</response>
        /// <response code="400">If page size or page number is incorrect</response>
        [ProducesResponseType(typeof(PageResponseApiModel<InstitutionOfEducationResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Anonymous")]
        public async Task<IActionResult> GetInstitutionOfEducationsPageForAnonym(
            string DirectionName, 
            string SpecialtyName, 
            string InstitutionOfEducationName,
            string InstitutionOfEducationAbbreviation, 
            string PaymentForm,
            string EducationForm,
            int page = 1, 
            int pageSize = 10)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                InstitutionOfEducationName = InstitutionOfEducationName,
                InstitutionOfEducationAbbreviation = InstitutionOfEducationAbbreviation,
                PaymentForm = PaymentForm,
                EducationForm = EducationForm
            };

            var pageModel = new PageApiModel
            {
                Page = page,
                PageSize = pageSize,
                Url = $"{Request?.Scheme}://{Request?.Host}{Request?.Path}"
            };

            var result = await _institutionOfEducationService.GetInstitutionOfEducationsPage(filterModel, pageModel);
            return Ok(result);
        }

        /// <summary>
        /// Get all institutionOfEducations with pagination for authorized user with checking of its favorites.
        /// </summary>
        /// <returns>Returns the page with institutionOfEducations</returns>
        /// <response code="200">Returns the page with institutionOfEducations</response>
        /// <response code="400">If page size or page number is incorrect</response>
        [ProducesResponseType(typeof(PageResponseApiModel<InstitutionOfEducationResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Authorized")]
        [Authorize]
        public async Task<IActionResult> GetInstitutionOfEducationsPageForAuthorizedUser(
            string DirectionName,
            string SpecialtyName,
            string InstitutionOfEducationName,
            string InstitutionOfEducationAbbreviation,
            string PaymentForm,
            string EducationForm,
            int page = 1,
            int pageSize = 10)
        {
            var userId = User.FindFirst("id").Value;

            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                InstitutionOfEducationName = InstitutionOfEducationName,
                InstitutionOfEducationAbbreviation = InstitutionOfEducationAbbreviation,
                PaymentForm = PaymentForm,
                EducationForm = EducationForm
            };

            var pageModel = new PageApiModel
            {
                Page = page,
                PageSize = pageSize,
                Url = $"{Request?.Scheme}://{Request?.Host}{Request?.Path}"
            };

            var result = await _institutionOfEducationService.GetInstitutionOfEducationsPageForUser(filterModel, pageModel, userId);
            return Ok(result);
        }

        /// <summary>
        /// Get all favorite institutionOfEducations.
        /// </summary>
        /// <returns>Returns all favorite institutionOfEducations</returns>
        /// <response code="200">Returns the page with institutionOfEducations</response>
        /// <response code="404">If user doesn't have favorite institutionOfEducations</response>
        [ProducesResponseType(typeof(IEnumerable<InstitutionOfEducationResponseApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Favorites")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> GetFavoriteInstitutionOfEducations()
        {
            var userId = User.FindFirst("id")?.Value;
            var result = await _institutionOfEducationService.GetFavoriteInstitutionOfEducations(userId);
            return Ok(result);
        }
        
        /// <summary>
        /// Add institutionOfEducation to favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="201">Returns if the institutionOfEducation has been successfully added to the favorites list</response>
        /// <response code="400">If id is not valid or institutionOfEducation has already been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpPost("Favorites/{institutionOfEducationId}")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> AddInstitutionOfEducationToFavorite(string institutionOfEducationId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _institutionOfEducationService.AddInstitutionOfEducationToFavorite(institutionOfEducationId, userId);
            return Created($"{Request.Scheme}://{Request.Host}{Request.Path}", institutionOfEducationId);
        }

        /// <summary>
        /// Delete institutionOfEducation from favorite.
        /// </summary>
        /// <returns>None</returns>
        /// <response code="200">Returns if the institutionOfEducation has been successfully deleted from the favorites list</response>
        /// <response code="400">If id is not valid or institutionOfEducation has not been added to favorites</response>
        /// <response code="401">If user is unauthorized, token is bad/expired</response>
        /// <response code="403">If user is not graduate</response>
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 403)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpDelete("Favorites/{institutionOfEducationId}")]
        [Authorize(Roles = "Graduate")]
        public async Task<IActionResult> DeleteInstitutionOfEducationFromFavorite(string institutionOfEducationId)
        {
            var userId = User.FindFirst("id")?.Value;
            await _institutionOfEducationService.DeleteInstitutionOfEducationFromFavorite(institutionOfEducationId, userId);
            return Ok(value: institutionOfEducationId);
        }


        /// <summary>
        /// Get all institutionOfEducation abbreviations.
        /// </summary>
        /// <returns>List of institutionOfEducation abbreviations</returns>
        /// <response code="200">Returns a list of institutionOfEducation abbreviations</response>
        /// <response code="404">If there are not institutionOfEducations</response>
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 404)]
        [ProducesResponseType(typeof(ErrorDetails), 500)]
        [HttpGet("Abbreviations")]
        public async Task<IActionResult> GetInstitutionOfEducationAbbreviations(
            string DirectionName,
            string SpecialtyName,
            string InstitutionOfEducationName,
            string InstitutionOfEducationAbbreviation)
        {
            var filterModel = new FilterApiModel
            {
                DirectionName = DirectionName,
                SpecialtyName = SpecialtyName,
                InstitutionOfEducationName = InstitutionOfEducationName,
                InstitutionOfEducationAbbreviation = InstitutionOfEducationAbbreviation
            };

            var result = await _institutionOfEducationService.GetInstitutionOfEducationAbbreviations(filterModel);
            return Ok(result);
        }
    }
}