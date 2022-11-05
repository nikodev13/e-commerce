using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Shared.Exceptions
{
    public class InvalidMoneyValueException : ECommerceException
    {
        public InvalidMoneyValueException() : base("Money value must be greater than zero.")
        {
        }
    }
}
