namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class InstitutionOfEducationToGraduateDTO
    {
        public string GraduateId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public GraduateDTO Graduate { get; set; }
        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
