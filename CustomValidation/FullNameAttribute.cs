using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.CustomValidation
{
    [AttributeUsageAttribute(AttributeTargets.Class| AttributeTargets.Property)]
    public class FullNameAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var fullName = value as string;

            string[] checkingFullNmae = fullName?.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];
            if (fullName is not null )
            {
                if (checkingFullNmae.Length < 3)
                {
                    return new ValidationResult("FullName must contain at least Three words.");
                }
                foreach (var namePart in checkingFullNmae)
                {
                    if (namePart.Length <2)
                    {
                        return new ValidationResult("Each part of the FullName must be at least two characters long.");
                    }
                }
            }

            return ValidationResult.Success;


        }


    }
}
