namespace YIF.Core.Data.Entities
{
    public class PaymentFormToDescription : BaseEntity
    {
        public string PaymentFormId { get; set; }
        public string SpecialtyToIoEDescriptionId { get; set; }
        
        public virtual PaymentForm PaymentForm { get; set; }
        public virtual SpecialtyToIoEDescription SpecialtyToIoEDescription { get; set; }
    }
}
