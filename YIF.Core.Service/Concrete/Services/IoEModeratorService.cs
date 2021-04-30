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
        private readonly IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> _institutionOfEducationModeratorRepository;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resourceManager;

        public IoEModeratorService(
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> ioERepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToIoERepository,
            ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> specialtyToIoEDescriptionRepository,
            IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> examRequirementRepository,
            IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModerator, InstitutionOfEducationModeratorDTO> institutionOfEducationModeratorRepository,
            IMapper mapper,
            ResourceManager resourceManager)
        {
            _specialtyRepository = specialtyRepository;
            _ioERepository = ioERepository;
            _specialtyToIoERepository = specialtyToIoERepository;
            _specialtyToIoEDescriptionRepository = specialtyToIoEDescriptionRepository;
            _examRequirementRepository = examRequirementRepository;
            _institutionOfEducationModeratorRepository = institutionOfEducationModeratorRepository;
            _mapper = mapper;
            _resourceManager = resourceManager;

        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddRangeSpecialtiesToIoE(string userId,
           IEnumerable<SpecialtyToInstitutionOfEducationAddRangePostApiModel> specialtyToIoE)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();
            string ioEId = (await _institutionOfEducationModeratorRepository.GetByUserId(userId)).Admin.InstitutionOfEducationId;

            foreach (var item in specialtyToIoE)
            {
                string speciltyId = (await _specialtyToIoERepository.GetById(item.SpecialtyId)).SpecialtyId;

                if (speciltyId == null)
                {
                    SpecialtyToInstitutionOfEducation specialtyToIoEducation = new SpecialtyToInstitutionOfEducation
                    {
                        SpecialtyId = item.SpecialtyId,
                        InstitutionOfEducationId = ioEId,
                        IsDeleted = false
                    };

                    await _specialtyToIoERepository.AddSpecialty(specialtyToIoEducation);
                }

                foreach (var desc in item.PaymentAndEducationForms)
                {
                    var description = await _specialtyToIoEDescriptionRepository.Find(s => s.PaymentForm == desc.PaymentForm && s.EducationForm == desc.EducationForm && s.SpecialtyToInstitutionOfEducationId == item.SpecialtyId);
                    if (description == null)
                    {
                        var toIoEDescription = new SpecialtyToIoEDescription
                        {
                            SpecialtyToInstitutionOfEducationId = item.SpecialtyId,
                            PaymentForm = desc.PaymentForm,
                            EducationForm = desc.EducationForm
                        };

                        await _specialtyToIoEDescriptionRepository.Add(toIoEDescription);
                    }
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
