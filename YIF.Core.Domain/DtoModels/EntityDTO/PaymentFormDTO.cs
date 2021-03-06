﻿using System.Collections.Generic;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class PaymentFormDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentFormToDescriptionDTO> PaymentFormToDescriptions { get; set; }
    }
}