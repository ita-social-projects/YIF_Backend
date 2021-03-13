namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class EducationFormToDescriptionDTO
    {
        public string Id { get; set; }
        public string EducationFormId { get; set; }
        public string SpecialtyInInstitutionOfEducationDescriptionId { get; set; }

        public virtual EducationFormDTO EducationForm { get; set; }
        public virtual SpecialtyInInstitutionOfEducationDescriptionDTO SpecialtyInInstitutionOfEducationDescription { get; set; }
    }
}