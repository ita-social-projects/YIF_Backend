﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface ISpecialtyService : IDisposable
    {
        Task<ResponseApiModel<SpecialtyResponseApiModel>> GetSpecialtyById(string id);
        Task<ResponseApiModel<IEnumerable<SpecialtyResponseApiModel>>> GetAllSpecialties();
        Task<IEnumerable<SpecialtyResponseApiModel>> GetAllSpecialtiesByFilter(FilterApiModel filterModel);
        Task<IEnumerable<string>> GetSpecialtiesNamesByFilter(FilterApiModel filterModel);
        Task<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>> GetAllSpecialtyDescriptionsById(string id);
        Task<SpecialtyDescriptionForEditPageResponseApiModel> GetFullSpecialtyDescriptionById(string specialtyId, string IoEId);
        Task AddSpecialtyAndInstitutionOfEducationToFavorite(string specialtyId, string institutionOfEducationId, string userId);
        Task DeleteSpecialtyAndInstitutionOfEducationFromFavorite(string specialtyId, string institutionOfEducationId, string userId);
        Task AddSpecialtyToFavorite(string specialtyId, string userId);
        Task DeleteSpecialtyFromFavorite(string specialtyId, string userId);
        Task<IEnumerable<ExamsResponseApiModel>> GetExamsNames();
        Task<IEnumerable<EducationFormsResponseApiModel>> GetEducationFormsNames();
        Task<IEnumerable<PaymentFormsResponseApiModel>> GetPaymentFormsNames();

    }
}
