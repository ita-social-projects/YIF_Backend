using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;
        private readonly ILogger<SpecialtyController> _logger;
        public SpecialtyController(ISpecialtyService specialtyService, ILogger<SpecialtyController> logger)
        {
            _specialtyService = specialtyService;
            _logger = logger;
        }



        #region SpecialtyRequests
        /// <summary>
        /// Get all specialties.
        /// </summary>
        /// <returns>List of specialties</returns>
        /// <response code="200">Returns a list of specialties</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SpecialtyApiModel>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSpecialtiesAsync()
        {
            var result = await _specialtyService.GetAllSpecialties();
            _logger.LogInformation("Getting all spetialties");
            return result.Response();
        }

        /// <summary>
        /// Get all specialties names.
        /// </summary>
        /// <returns>List of specialties names</returns>
        /// <response code="200">Returns a list of specialties names</response>
        [HttpGet("names")]
        [ProducesResponseType(typeof(SpecialtyNamesApiModel), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllSpecialtiesNamesAsync()
        {
            var result = await _specialtyService.GetAllSpecialtiesNames();
            _logger.LogInformation("Getting all spetialties");
            return result.Response();
        }

        /// <summary>
        /// Get specialty by id.
        /// </summary>
        /// <returns>A specialty</returns>
        /// <response code="200">Returns a specialty</response>
        /// <param name="id" example="28bf4f2e-6c43-42c0-8391-cbbaba6b5a5a">Specialty ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<SpecialtyApiModel>), 200)]
        [ProducesResponseType(typeof(DescriptionResponseApiModel), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSpecialtyAsync(string id)
        {
            try
            {
                Guid guid = Guid.Parse(id);
                var result = await _specialtyService.GetSpecialtyById(guid.ToString("D"));
                _logger.LogInformation("Trying to get a specialty");
                return result.Response();
            }
            catch (ArgumentNullException)
            {
                _logger.LogError("Null specialty is not allowed");
                return BadRequest(new DescriptionResponseApiModel("The string to be parsed is null."));
                //return new ResponseApiModel<object> { StatusCode = 400, Message = "The string to be parsed is null." }.Response();
            }
            catch (FormatException)
            {
                _logger.LogError("There is a problem with format");
                return BadRequest(new DescriptionResponseApiModel($"Bad format:  {id}."));
                //return new ResponseApiModel<object> { StatusCode = 400, Message = $"Bad format:  {id}." }.Response();
            }
        }
        #endregion




    }
}
