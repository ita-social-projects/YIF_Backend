using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Identity;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Service.Concrete.Services
{
    class BaseService
    {
        protected readonly UserManager<DbUser> _userManager;
        protected readonly SignInManager<DbUser> _signInManager;
        protected readonly IConfiguration _configuration;
        protected readonly IMapper _mapper;

        public BaseService(
            UserManager<DbUser> userManager,
            SignInManager<DbUser> signInManager,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mapper = mapper;
        }
    }
}
