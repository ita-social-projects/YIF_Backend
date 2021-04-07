namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToInstitutionOfEducationToGraduateDTO
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string GraduateId { get; set; }

        public SpecialtyDTO Specialty { get; set; }
        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
        public GraduateDTO Graduate { get; set; }
    }
}
