namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToInstitutionOfEducationDTO
    {
        public string Id { get; set; }
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }

        public virtual SpecialtyToIoEDescriptionDTO SpecialtyToIoEDescription { get; set; }
        public virtual SpecialtyDTO Specialty { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
