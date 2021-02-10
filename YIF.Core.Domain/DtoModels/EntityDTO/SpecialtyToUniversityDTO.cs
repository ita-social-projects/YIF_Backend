namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToUniversityDTO
    {
        public string Id { get; set; }
        public string SpecialtyId { get; set; }
        public string UniversityId { get; set; }

        public SpecialtyDTO Specialty { get; set; }
        public UniversityDTO University { get; set; }
    }
}
