namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToInstitutionOfEducationToGraduateDTO
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string GraduateId { get; set; }

        public virtual SpecialtyDTO Specialty { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        public virtual GraduateDTO Graduate { get; set; }
    }
}
