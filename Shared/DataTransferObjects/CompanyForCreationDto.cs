using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto
    {
        [Required(ErrorMessage ="Company Name is a required field")]
        [MaxLength(30,ErrorMessage ="Company Name must have max 30 chars")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Company Adress is a required field")]
        [MaxLength(250,ErrorMessage ="Company Adresse must not exceed 250 chars")]
        public string Adress { get; init; }
        [Required(ErrorMessage = "Company Adress is a required field")]
        [MaxLength(50, ErrorMessage = "Company Country must not exceed 50 chars")]
        public string? Country { get; init; }

        // if provided we can directly create children , EF core (and automapper) will understand it and take care of it 
        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
    }
}
