using Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record CompanyForUpdateDto
    {
        [Required(ErrorMessage = "Company Name is a required field")]
        [MaxLength(30, ErrorMessage = "Company Name must have max 30 chars")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Company Adress is a required field")]
        [MaxLength(250, ErrorMessage = "Company Adresse must not exceed 250 chars")]
        public string Adress { get; init; }
        [Required(ErrorMessage = "Company Adress is a required field")]
        [MaxLength(50, ErrorMessage = "Company Country must not exceed 50 chars")]
        [AllowedCountryEnum]
        public string? Country { get; init; }


    }
}
