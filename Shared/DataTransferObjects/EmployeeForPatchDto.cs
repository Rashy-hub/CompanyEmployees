using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects
{
    public record EmployeeForPatchDto
    {

        [MaxLength(20, ErrorMessage = "Maximum length for the Name is 20 characters.")]
        public string? Name { get; init; }

        [Range(18, 120, ErrorMessage = "Employee age must be between 18 and 120")]
        public int? Age { get; init; }

        [MaxLength(20, ErrorMessage = "Maximum length for the Position is 20 characters.")]
        public string? Position { get; init; }
    }
}
