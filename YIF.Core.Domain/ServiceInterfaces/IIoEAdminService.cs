using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;

namespace YIF.Core.Domain.ServiceInterfaces
{
    public interface IIoEAdminService
    {
        Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToIoe(SpecialtyAndInstitutionOfEducationPostApiModel specialtyAndInstitutionOfEducationPostApiModel);
    }
}
