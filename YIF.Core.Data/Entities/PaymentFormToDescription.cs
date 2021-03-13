namespace YIF.Core.Data.Entities
{
    public class PaymentFormToDescription : BaseEntity
    {
        public string PaymentFormId { get; set; }
        public string SpecialtyInInstitutionOfEducationDescriptionId { get; set; }
        
        public virtual PaymentForm PaymentForm { get; set; }
        public virtual SpecialtyInInstitutionOfEducationDescription SpecialtyInInstitutionOfEducationDescription { get; set; }
    }
}
