﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities.IdentityEntities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.Models.IdentityDTO;
using YIF.Core.Domain.ServiceInterfaces;
using YIF.Core.Domain.ViewModels;
using YIF.Core.Domain.ViewModels.IdentityViewModels;

namespace YIF.Core.Service.Concrete.Services
{
    public class UserService : IUserService<DbUser>
    {
        private readonly IRepository<DbUser, UserDTO> _userRepository;

        public UserService(IRepository<DbUser, UserDTO> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseModel<IEnumerable<UserViewModel>>> GetAllUsers()
        {
            var result = new ResponseModel<IEnumerable<UserViewModel>>();
            var users = await _userRepository.GetAll();
            if (users == null)
            {
                return result.Set(false, $"There are not users in database");
            }

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<UserDTO, UserViewModel>();
            }));

            result.Object = mapper.Map<IEnumerable<UserViewModel>>(users);
            return result.Set(true);
        }

        public async Task<ResponseModel<UserViewModel>> GetUserById(string id)
        {
            var result = new ResponseModel<UserViewModel>();
            try
            {
                var user = await _userRepository.Get(id);
                var mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()));
                result.Object = mapper.Map<UserViewModel>(user);
            }
            catch (KeyNotFoundException ex)
            {
                return result.Set(false, ex.Message);
            }
            return result.Set(true);
        }

        public async Task<ResponseModel<IEnumerable<UserViewModel>>> FindUser(Expression<Func<DbUser, bool>> predicate)
        {
            var result = new ResponseModel<IEnumerable<UserViewModel>>();
            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<UserDTO, UserViewModel>();
            }));

            result.Object = mapper.Map<IEnumerable<UserViewModel>>(await _userRepository.Find(predicate));
            return result.Set(true);
        }

        public async Task<ResponseModel<UserViewModel>> CreateUser(UserDTO userDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserById(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUser(UserDTO user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
