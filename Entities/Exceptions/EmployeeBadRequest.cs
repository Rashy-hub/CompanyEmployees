using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class EmployeeBadRequest : BadRequestException
    {
        public EmployeeBadRequest() : base("Employee sent by client is null !")
        {
        }
    }
}
