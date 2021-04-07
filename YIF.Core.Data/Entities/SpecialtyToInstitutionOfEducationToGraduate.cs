namespace YIF.Core.Data.Entities
{
    public class SpecialtyToInstitutionOfEducationToGraduate
    {
        public string SpecialtyId { get; set; }
        public string InstitutionOfEducationId { get; set; }
        public string GraduateId { get; set; }

        public Specialty Specialty { get; set; }
        public InstitutionOfEducation InstitutionOfEducation { get; set; }
        public Graduate Graduate { get; set; }
    }
}
