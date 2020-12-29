﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Data.Others;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Domain.Repositories
{
    public class UserRepository : IRepository<DbUser, UserDTO>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<DbUser> _userManager;

        public UserRepository(IApplicationDbContext context, IMapper mapper, UserManager<DbUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<string> Create(DbUser dbUser, Object entityUser, string userPassword)
        {
            var result = await _userManager.CreateAsync(dbUser, userPassword);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(dbUser, ProjectRoles.Graduate);
                await _context.AddAsync(entityUser);
                await _context.SaveChangesAsync();
                return string.Empty;
            }
            return result.Errors.First().Description;
        }

        public async Task<bool> Update(DbUser user)
        {
            if (user != null)
            {
                if (_context.Users.Find(user) != null)
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }       

        public async Task<bool> Delete(string id)
        {
            DbUser user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        [ExcludeFromCodeCoverage]
        public async Task<IEnumerable<UserDTO>> Find(Expression<Func<DbUser, bool>> predicate)
        {
            var user = await _context.Users.Where(predicate).ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(user);
        }

        public async Task<UserDTO> Get(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            throw new KeyNotFoundException("User not found:  " + id);
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var list = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(list);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}