namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class PaymentFormToDescriptionDTO
    {
        public string Id { get; set; }
        public string PaymentFormId { get; set; }

        public string SpecialtyInUniversityDescriptionId { get; set; }

        public PaymentFormDTO PaymentForm { get; set; }

        public SpecialtyInUniversityDescriptionDTO SpecialtyInUniversityDescription { get; set; }
    }
}