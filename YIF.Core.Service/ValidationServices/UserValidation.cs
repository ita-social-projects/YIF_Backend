using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Service.ValidationServices
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(1, 20)               
                .WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(1, 20)
                .WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.Net4xRegex)
                .WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.Age)
                .NotEmpty()
                .GreaterThan(1)
                .LessThan(120)
                .WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.Address)
                .NotEmpty()
                .Length(3,120)
                .WithMessage("{PropertyName} incorrect");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .Must(BeAValidDate)
                .WithMessage("{PropertyName} incorrect");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !(date >= DateTime.Now);
        }
    }
}
