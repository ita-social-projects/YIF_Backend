using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using SendGrid.Helpers.Errors.Model;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class IoEAdminServiceAdminService:IIoEAdminService
    {
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _ioERepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToIoERepository;
        private readonly ResourceManager _resourceManager;

        public IoEAdminServiceAdminService(
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository, 
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> ioERepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToIoERepository,
            ResourceManager resourceManager)
        {
            _specialtyRepository = specialtyRepository;
            _ioERepository = ioERepository;
            _specialtyToIoERepository = specialtyToIoERepository;
            _resourceManager = resourceManager;

        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToIoe(
            SpecialtyAndInstitutionOfEducationPostApiModel specialtyToIoE)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            var specialty = await _specialtyRepository.ContainsById(specialtyToIoE.SpecialtyId);
            var institutionOfEducation = await _ioERepository.ContainsById(specialtyToIoE.InstitutionOfEducationId);
            var entity = new SpecialtyToInstitutionOfEducation()
            {
                SpecialtyId = specialtyToIoE.SpecialtyId,
                InstitutionOfEducationId = specialtyToIoE.InstitutionOfEducationId
            };
            var specialtyExists = await _specialtyToIoERepository.IsSpecialtyToIoEAlreadyExists(entity);

            if (specialtyExists == true)
            {
                throw new BadRequestException(_resourceManager.GetString("SpecialtyAndInstitutionOfEducationIsAlreadyExists"));
            }
            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));
            await _specialtyToIoERepository.AddSpecialty(entity);
            return result.Set(new DescriptionResponseApiModel("Specialty was successfully added to the Institution of Education"), true);
        }
    }
}
