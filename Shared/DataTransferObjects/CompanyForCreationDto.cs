using Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto : IValidatableObject
    {
        [Required(ErrorMessage = "Company Name is a required field")]
        [MaxLength(30, ErrorMessage = "Company Name must have max 30 chars")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Company Adress is a required field")]
        [MaxLength(250, ErrorMessage = "Company Adresse must not exceed 250 chars")]
        public string Adress { get; init; }
        [Required(ErrorMessage = "Company Adress is a required field")]
        [MaxLength(50, ErrorMessage = "Company Country must not exceed 50 chars")]
        //custom validation attribtue here
        [AllowedCountryEnum]
        public string? Country { get; init; }

        // if provided we can directly create children , EF core (and automapper) will understand it and take care of it 
        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
        // Validate the optional employees count to max 10 employees to create with company
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Employees != null)
            {
                const int maxEmployees = 10;

                if (Employees.Count() > maxEmployees)
                {
                    results.Add(new ValidationResult($"Employees count cannot exceed {maxEmployees}.",new[] { nameof(Employees) }));
                }

                foreach (var employee in Employees)
                {
                    var context = new ValidationContext(employee);
                    var employeeResults = new List<ValidationResult>();

                    if (!Validator.TryValidateObject(employee, context, employeeResults, true))
                    {
                        results.AddRange(employeeResults);
                    }
                }
            }

            return results;
        }

    }
}
