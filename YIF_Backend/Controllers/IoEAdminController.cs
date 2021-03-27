using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "InstitutionOfEducationAdmin")]
    public class IoEAdminController : ControllerBase
    {
        private readonly IIoEAdminService _ioEAdminService;

        public IoEAdminController(IIoEAdminService ioEAdminService)
        {
            _ioEAdminService = ioEAdminService;
        }
    }
}
