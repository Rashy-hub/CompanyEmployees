using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public class CompanyCollectionBadRequest : BadRequestException
    {
        public CompanyCollectionBadRequest() : base("Company colleciton sent by client must not be null or contain more than 10 items !")
        {
        }
    }
}
