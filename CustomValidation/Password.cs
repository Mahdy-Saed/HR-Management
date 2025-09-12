using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HR_Carrer.CustomValidation
{
    public class Password:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            var req = new Regex(@"^(?=.*[A-Za-z])\S{8,}$");


            if (password  is not null && !req.IsMatch(password)){
                return new ValidationResult("Password must be at least 8 characters long, contain at least one letter, and not contain spaces.");
            }

            return ValidationResult.Success;

        } 

     }
  }

