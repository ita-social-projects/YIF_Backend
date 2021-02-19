using System.ComponentModel.DataAnnotations.Schema;

namespace YIF.Core.Data.Entities
{
    public class PaymentFormToDescription : BaseEntity
    {
        public string PaymentFormId { get; set; }

        public string SpecialtyInUniversityDescriptionId { get; set; }
        
        public PaymentForm PaymentForm { get; set; }
        
        public SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
