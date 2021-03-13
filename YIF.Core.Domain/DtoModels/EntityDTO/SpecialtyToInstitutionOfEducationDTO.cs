namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToInstitutionOfEducationDTO
    {
        public string Id { get; set; }
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string SpecialtyInInstitutionOfEducationDescriptionId { get; set; }

        public virtual SpecialtyInInstitutionOfEducationDescriptionDTO SpecialtyInInstitutionOfEducationDescription { get; set; }
        public virtual SpecialtyDTO Specialty { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
