namespace YIF.Core.Data.Entities
{
    public class SpecialtyToUniversityToGraduate
    {
        public string SpecialtyId { get; set; }
        public string UniversityId { get; set; }
        public string GraduateId { get; set; }

        public virtual Specialty Specialty { get; set; }
        public virtual University University { get; set; }
        public virtual Graduate Graduate { get; set; }
    }
}
