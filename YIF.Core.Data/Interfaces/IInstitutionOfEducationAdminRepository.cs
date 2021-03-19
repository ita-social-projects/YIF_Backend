﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Entities.IdentityEntities;

namespace YIF.Core.Data.Interfaces
{
    public interface IInstitutionOfEducationAdminRepository<K> : IDisposable
        where K : class
    {
        Task<string> AddUniAdmin(InstitutionOfEducationAdmin institutionOfEducationAdmin);
        Task<string> Delete(DbUser user);
        Task<string> Disable(InstitutionOfEducationAdmin adminId);
        Task<string> Enable(InstitutionOfEducationAdmin adminId);
        Task<K> GetById(string id);
        Task<K> GetByInstitutionOfEducationId(string institutionOfEducationId);
        Task<K> GetByInstitutionOfEducationIdWithoutIsDeletedCheck(string institutionOfEducationId);
        Task<K> GetUserByAdminId(string id);
        Task<IEnumerable<K>> GetAllUniAdmins();
    }
}