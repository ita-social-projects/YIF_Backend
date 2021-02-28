namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToUniversityDTO
    {
        public string Id { get; set; }
        public string SpecialtyId { get; set; }
        public string UniversityId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }

        public virtual SpecialtyInUniversityDescriptionDTO SpecialtyInUniversityDescription { get; set; }
        public virtual SpecialtyDTO Specialty { get; set; }
        public virtual UniversityDTO University { get; set; }
    }
}
