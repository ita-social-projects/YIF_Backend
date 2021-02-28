namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class EducationFormToDescriptionDTO
    {
        public string Id { get; set; }
        public string EducationFormId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }

        public EducationFormDTO EducationForm { get; set; }
        public SpecialtyInUniversityDescriptionDTO SpecialtyInUniversityDescription { get; set; }
    }
}