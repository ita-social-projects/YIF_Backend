using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF_Backend.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("GetRandomNumber")]
        public async Task<IActionResult> GetRandomNumber()
        {
            var result = new Random().Next(1, 1000);

            return Ok(result);
        }

        //private readonly EFDbContext _context;

        //public HomeController(EFDbContext context)
        //{
        //    _context = context;
        //}

        //[HttpGet("usersdb")]
        //public ActionResult<IEnumerable<DbUser>> GetUsersDB()
        //{
        //    return _context.Users;
        //}

        //[HttpGet("users")]
        //public ActionResult<IEnumerable<string>> GetUsers()
        //{
        //    return new string[] { "stepan", "oleg" };
        //}
    }
}
