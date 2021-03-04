namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToUniversityToGraduateDTO
    {
        public string SpecialtyId { get; set; }
        public string UniversityId { get; set; }
        public string GraduateId { get; set; }

        public virtual SpecialtyDTO Specialty { get; set; }
        public virtual UniversityDTO University { get; set; }
        public virtual GraduateDTO Graduate { get; set; }
    }
}
