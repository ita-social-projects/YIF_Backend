namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class EducationFormToDescriptionDTO
    {
        public string Id { get; set; }
        public string EducationFormId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }

        public virtual EducationFormDTO EducationForm { get; set; }
        public virtual SpecialtyToIoEDescriptionDTO SpecialtyToIoEDescription { get; set; }
    }
}