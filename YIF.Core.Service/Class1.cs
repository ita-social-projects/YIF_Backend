using FluentValidation;
using System;
using YIF.Core.Service.ValidationServices;

namespace YIF.Core.Service
{
    public class Class1
    {
        public Class1(IValidator<User> validationService)
        {
            var correctUser = new User
            {
                FirstName = "Vladyslav",
                LastName = "Negodiyk",
                Age = 18,
                Address = "Rivne",
                DateOfBirth = new DateTime(2002,05,07),
                Email = "pearmadn@gmail.com"
            };

            var wrongUser = new User 
            {
                FirstName = "V",
                LastName = "N",
                Age = -1,
                Address = "R",
                DateOfBirth = new DateTime(2021, 05, 07),
                Email = "pea@rmadn@gmail.com"
            };
            
            var validationResult = validationService.Validate(correctUser);
            Console.WriteLine($"Validation result - {validationResult.IsValid}\n\n");           

            validationResult = new UserValidation().Validate(wrongUser);
            Console.WriteLine($"Validation result - {validationResult.IsValid}");
            Console.WriteLine("-------Errors------");
            for (int i = 0; i < validationResult.Errors.Count; i++)
            {
                Console.WriteLine(validationResult.Errors[i]);
            }

        }
    }

    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
