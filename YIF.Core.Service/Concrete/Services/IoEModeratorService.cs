using System.Resources;
using System.Threading.Tasks;
using System.Collections.Generic;
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
    public class IoEModeratorService:IIoEModeratorService
    {
        private readonly ISpecialtyRepository<Specialty, SpecialtyDTO> _specialtyRepository;
        private readonly IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> _ioERepository;
        private readonly ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> _specialtyToIoERepository;
        private readonly ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> _specialtyToIoEDescriptionRepository;
        private readonly IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> _examRequirementRepository;
        private readonly IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO> _institutionOfEducationModeratorRepository;
        private readonly IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> _directionToIoERepository;
        private readonly IMapper _mapper;
        private readonly ResourceManager _resourceManager;

        public IoEModeratorService(
            ISpecialtyRepository<Specialty, SpecialtyDTO> specialtyRepository,
            IInstitutionOfEducationRepository<InstitutionOfEducation, InstitutionOfEducationDTO> ioERepository,
            ISpecialtyToInstitutionOfEducationRepository<SpecialtyToInstitutionOfEducation, SpecialtyToInstitutionOfEducationDTO> specialtyToIoERepository,
            ISpecialtyToIoEDescriptionRepository<SpecialtyToIoEDescription, SpecialtyToIoEDescriptionDTO> specialtyToIoEDescriptionRepository,
            IExamRequirementRepository<ExamRequirement, ExamRequirementDTO> examRequirementRepository,
            IInstitutionOfEducationModeratorRepository<InstitutionOfEducationModeratorDTO> institutionOfEducationModeratorRepository,
            IRepository<DirectionToInstitutionOfEducation, DirectionToInstitutionOfEducationDTO> directionToIoERepository,
            IMapper mapper,
            ResourceManager resourceManager)
        {
            _specialtyRepository = specialtyRepository;
            _ioERepository = ioERepository;
            _specialtyToIoERepository = specialtyToIoERepository;
            _specialtyToIoEDescriptionRepository = specialtyToIoEDescriptionRepository;
            _examRequirementRepository = examRequirementRepository;
            _institutionOfEducationModeratorRepository = institutionOfEducationModeratorRepository;
            _directionToIoERepository = directionToIoERepository;
            _mapper = mapper;
            _resourceManager = resourceManager;

        }

        public async Task<ResponseApiModel<DescriptionResponseApiModel>> AddSpecialtyToIoe(
            SpecialtyToInstitutionOfEducationPostApiModel specialtyToIoE)
        {
            var result = new ResponseApiModel<DescriptionResponseApiModel>();

            var specialty = await _specialtyRepository.ContainsById(specialtyToIoE.SpecialtyId);
            var institutionOfEducation = await _ioERepository.ContainsById(specialtyToIoE.InstitutionOfEducationId);
            var entity = new SpecialtyToInstitutionOfEducation()
            {
                SpecialtyId = specialtyToIoE.SpecialtyId,
                InstitutionOfEducationId = specialtyToIoE.InstitutionOfEducationId
            };

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            if (specialty == false)
                throw new BadRequestException(_resourceManager.GetString("SpecialtyNotFound"));

            await _specialtyToIoERepository.AddSpecialty(entity);
            return result.Set(new DescriptionResponseApiModel("Specialty was successfully added to the Institution of Education"), true);
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

        public async Task<IEnumerable<DirectionToIoEResponseApiModel>> GetAllDirectionsAndSpecialitiesOfModerator(string moderatorId)
        {
            var moderator = await _institutionOfEducationModeratorRepository.GetById(moderatorId);

            if (moderator == null)
                throw new BadRequestException(_resourceManager.GetString("IoEModeratorNotFound"));

            var institutionOfEducation = await _ioERepository.ContainsById(moderator.Admin.InstitutionOfEducationId);

            if (institutionOfEducation == false)
                throw new BadRequestException(_resourceManager.GetString("InstitutionOfEducationNotFound"));

            var ioEdirections = _directionToIoERepository.Find(x => x.InstitutionOfEducationId == moderator.Admin.InstitutionOfEducationId);
            var response = _mapper.Map<IEnumerable<DirectionToIoEResponseApiModel>>(ioEdirections);
            foreach (DirectionToIoEResponseApiModel responseApiModel in response)
            {
                responseApiModel.Specialties = _mapper.Map<IEnumerable<SpecialtyToInstitutionOfEducationResponseApiModel>>
                    (_specialtyToIoERepository.Find(s => s.SpecialtyId == responseApiModel.Id && s.InstitutionOfEducationId == moderator.Admin.InstitutionOfEducationId));
            }
            return response;
        }
    }
}
