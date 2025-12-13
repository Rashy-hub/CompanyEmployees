using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class AllowedCountryEnumAttribute : ValidationAttribute
    {
        // This validation uses a hard-coded enum for demo purposes.
        // In real-world scenarios, it's better to use a database table for permitted countries (only CRUD allowed for "admin" role for example).
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
                return ValidationResult.Success;

            if (!Enum.TryParse(typeof(CountryEnum), value.ToString(), true, out _))
            {
                return new ValidationResult(
                    $"'{value}' is not a valid country."
                );
            }

            return ValidationResult.Success;

        }
    }
}
