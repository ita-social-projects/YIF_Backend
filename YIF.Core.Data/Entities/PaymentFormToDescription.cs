namespace YIF.Core.Data.Entities
{
    public class PaymentFormToDescription : BaseEntity
    {
        public string PaymentFormId { get; set; }
        public string SpecialtyInUniversityDescriptionId { get; set; }
        
        public virtual PaymentForm PaymentForm { get; set; }
        public virtual SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
