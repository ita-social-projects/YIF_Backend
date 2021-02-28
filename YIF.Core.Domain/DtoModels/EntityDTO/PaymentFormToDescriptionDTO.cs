namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class PaymentFormToDescriptionDTO
    {
        public string Id { get; set; }
        public string PaymentFormId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }

        public virtual PaymentFormDTO PaymentForm { get; set; }
        public virtual SpecialtyInUniversityDescriptionDTO SpecialtyInUniversityDescription { get; set; }
    }
}