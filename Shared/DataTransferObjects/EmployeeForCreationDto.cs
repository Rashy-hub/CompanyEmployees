using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record EmployeeForCreationDto
    {
        [Required(ErrorMessage = "Employee name is a required field !")]
        public string Name { get; init; }
        [Required(ErrorMessage = "Employee age is a required field !")]
        [Range(18, 120, ErrorMessage = "Employee age must be between 18 and 120")]
        public int Age { get; init; }
        [Required(ErrorMessage = "Employee position is a required field.")]
        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
        public string? Position { get; init; }
    }
}
