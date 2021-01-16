using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService<University> _universityService;

        public UniversityController(IUniversityService<University> universityService)
        {
            _universityService = universityService;
        }

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUniversityById(string id)
        {
            var result = await _universityService.GetUniversityById(id);
            return result.Response(200);
        }

        [HttpGet]
        public async Task<IActionResult> GetUniversitiesPage(int page = 1, int pageSize = 10)
        {
            var result = new UniversitiesPageResponseApiModel();
            try
            {
                var url = $"{Request.Scheme}://{Request.Host}{Request.Path}";
                result = await _universityService.GetUniversitiesPage(page, pageSize, url);
            }
            catch (Exception e)
            {
                return BadRequest(new DescriptionResponseApiModel { Message = e.Message });
            }
            return Ok(result);
        }
    }
}