using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.RequestApiModels;
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

        [HttpPost("GetUniversityByFilter")] // FIX
        public async Task<IActionResult> GetUniversityByFilter([FromBody]FilterApiModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _universityService.GetUniversityByFilter(model);

            return result.Response(200);
        }
    }
}