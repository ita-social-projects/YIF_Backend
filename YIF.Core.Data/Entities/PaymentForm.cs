using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Data.Entities
{
    public class PaymentForm : BaseEntity
    {
        public string Name { get; set; }
        public virtual SpecialtyInUniversityDescription SpecialtyInUniversityDescription { get; set; }
    }
}
