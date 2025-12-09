using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class CompanyBadRequest : BadRequestException
    {
        public CompanyBadRequest() : base("Company given by client is null")
        {
        }
    }
}
