using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class IdParameterBadRequestException : BadRequestException
    {
        public IdParameterBadRequestException() : base("Parameter Ids is null !")
        {
        }
    }
}
