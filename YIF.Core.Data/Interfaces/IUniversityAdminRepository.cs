﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;
namespace YIF.Core.Data.Interfaces
{
    public interface IUniversityAdminRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddUniAdmin(UniversityAdmin universityAdmin);
        Task<bool> Delete(string id);
        Task<K> GetById(string id);
        Task<K> GetByUniversityId(string universityId);
        Task<IEnumerable<K>> GetAllUniAdmins();
    }
}