using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;
using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using YIF.Core.Data.Entities;
using YIF.Core.Data.Interfaces;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.ServiceInterfaces;

namespace YIF.Core.Service.Concrete.Services
{
    public class IoEModeratorService : IIoEModeratorService
    {
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _ioERepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToIoERepository;
        private readonly ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> _specialtyToIoEDescriptionRepository;
        private readonly IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> _examRequirementRepository;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resourceManager;

        public IoEModeratorService(
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> ioERepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToIoERepository,
            ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> specialtyToIoEDescriptionRepository,
            IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> examRequirementRepository,
            IMapper mapper,
            ResourceManager resourceManager)
        {
            _specialtyRepository = specialtyRepository;
            _ioERepository = ioERepository;
            _specialtyToIoERepository = specialtyToIoERepository;
            _specialtyToIoEDescriptionRepository = specialtyToIoEDescriptionRepository;
            _examRequirementRepository = examRequirementRepository;
            _mapper = mapper;
            _resourceManager = resourceManager;

        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddRangeSpecialtiesToIoE(
           IEnumerable<SpecialtyToInstitutionOfEducationPostApiModel> specialtyToIoE)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            foreach (var item in specialtyToIoE)
            {
                var specialtyToInstitutionOfEducationDTO = _mapper.Map<SpecialtyToInstitutionOfEducationDTO>(item);
                var specialtyToInstitutionOfEducation = _mapper.Map<SpecialtyToInstitutionOfEducation>(specialtyToInstitutionOfEducationDTO);

                SpecialtyToInstitutionOfEducation specialtyToIoEducation = new SpecialtyToInstitutionOfEducation
                {
                    SpecialtyId = specialtyToInstitutionOfEducation.SpecialtyId,
                    InstitutionOfEducationId = specialtyToInstitutionOfEducation.InstitutionOfEducationId,
                    IsDeleted = false
                };

                var specialtyId = await _specialtyToIoERepository.AddSpecialty(specialtyToIoEducation);

                foreach (var desc in item.PaymentAndEducationForms)
                {
                    var toIoEDescription = new SpecialtyToIoEDescription
                    {
                        SpecialtyToInstitutionOfEducationId = specialtyId,
                        PaymentForm = desc.PaymentForm,
                        EducationForm = desc.EducationForm
                    };

                    await _specialtyToIoEDescriptionRepository.Add(toIoEDescription);
                }
            }

            return result.Set(
                   new DescriptionResponseApiModel(_resourceManager.GetString("SpecialtiesWereAdded")), true);
        }

        public async Task DeleteSpecialtyToIoe(SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE)
        {
            var specialty = await _specialtyRepository.ContainsById(specialtyToIoE.SpecialtyId);
            var institutionOfEducation = await _ioERepository.ContainsById(specialtyToIoE.InstitutionOfEducationId);
            var entity = await _specialtyToIoERepository.Find(s => s.SpecialtyId == specialtyToIoE.SpecialtyId && s.InstitutionOfEducationId == specialtyToIoE.InstitutionOfEducationId);

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));

            if (entity == null)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyInInstitutionOfEducationNotFound"));

            await _specialtyToIoERepository.Update(new SpecialtyToInstitutionOfEducation
            {
                SpecialtyId = specialtyToIoE.SpecialtyId,
                InstitutionOfEducationId = specialtyToIoE.InstitutionOfEducationId,
                IsDeleted = true
            });
        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> UpdateSpecialtyDescription(SpecialtyDescriptionUpdateApiModel specialtyDescriptionUpdateApiModel)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            var specialtyToIoEDescriptionDTO = _mapper.Map<SpecialtyToIoEDescriptionDTO>(specialtyDescriptionUpdateApiModel);

            await _examRequirementRepository.DeleteRangeByDescriptionId(specialtyDescriptionUpdateApiModel.Id);

            return result.Set(
                new DescriptionResponseApiModel(_resourceManager.GetString("SpecialtyDescriptionUpdated")),
                await _specialtyToIoEDescriptionRepository.Update(_mapper.Map<SpecialtyToIoEDescription>(specialtyToIoEDescriptionDTO)));
        }
    }
}
