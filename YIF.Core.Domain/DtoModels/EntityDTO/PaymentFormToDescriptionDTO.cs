namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class PaymentFormToDescriptionDTO
    {
        public string Id { get; set; }
        public string PaymentFormId { get; set; }
        public string SpecialtyInInstitutionOfEducationDescriptionId { get; set; }

        public virtual PaymentFormDTO PaymentForm { get; set; }
        public virtual SpecialtyInInstitutionOfEducationDescriptionDTO SpecialtyInInstitutionOfEducationDescription { get; set; }
    }
}