namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class InstitutionOfEducationToGraduateDTO
    {
        public string GraduateId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public virtual GraduateDTO Graduate { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
