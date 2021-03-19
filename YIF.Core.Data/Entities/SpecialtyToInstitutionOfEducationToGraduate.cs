namespace YIF.Core.Data.Entities
{
    public class SpecialtyToInstitutionOfEducationToGraduate
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string GraduateId { get; set; }

        public virtual Specialty Specialty { get; set; }
        public virtual InstitutionOfEducation InstitutionOfEducation { get; set; }
        public virtual Graduate Graduate { get; set; }
    }
}
