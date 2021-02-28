using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class SpecialtyToUniversityDTO
    {
        public string Id { get; set; }
        public string SpecialtyId { get; set; }
        public string UniversityId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }
        public SpecialtyInUniversityDescriptionDTO SpecialtyInUniversityDescription { get; set; }
        public SpecialtyDTO Specialty { get; set; }
        public UniversityDTO University { get; set; }
    }
}
