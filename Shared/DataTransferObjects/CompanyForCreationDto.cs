using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyForCreationDto
    {
        public string? Name { get; init; }
        public string? Adress { get; init; }
        public string? Country { get; init; }

        // if provided we can directly create children , EF core (and automapper) will understand it and take care of it 
        public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
    }
}
