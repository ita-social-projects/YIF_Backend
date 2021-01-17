using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.DtoModels.School;

namespace YIF.Core.Domain.Repositories
{
    public class SchoolGraduateRepository : ISchoolGraduateRepository<SchoolDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SchoolGraduateRepository(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [ExcludeFromCodeCoverage]
        public void Dispose() => _context.Dispose();


        public async Task<SchoolDTO> GetSchoolByUserId(string userId)
        {
            var gradute = await _context.Graduates.Include(s => s.School).FirstOrDefaultAsync(x => x.UserId == userId);
            var school = gradute?.School;
            return _mapper.Map<SchoolDTO>(school);
        }
    }
}
