using System.Collections.Generic;

namespace YIF.Core.Data.Entities
{
    public class PaymentForm : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<PaymentFormToDescription> PaymentFormToDescriptions { get; set; }
    }
}
