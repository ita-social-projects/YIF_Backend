namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialityToInstitutionOfEducationDTO
    {
        public string Id { get; set; }
        public string SpecialityId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual SpecialtyDTO Speciality { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        public virtual SpecialtyToIoEDescriptionDTO SpecialtyToIoEDescription { get; set; }

    }
}
