﻿using FluentValidation;
using System.Linq;
using System.Resources;
using YIF.Core.Data;
using YIF.Core.Data.Entities;
using YIF.Core.Domain.ApiModels.ResponseApiModels.EntityForResponse;

namespace YIF.Core.Domain.ApiModels.Validators
{
    public class PaymentAndEducationFormsPostApiModelValidator : AbstractValidator<PaymentAndEducationFormsPostApiModel>
    {
        public PaymentAndEducationFormsPostApiModelValidator()
        {
            RuleFor(x => x.PaymentForm)
                .IsInEnum();

            RuleFor(x => x.EducationForm)
                .IsInEnum();
        }
    }
}
