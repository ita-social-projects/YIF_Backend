using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
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
