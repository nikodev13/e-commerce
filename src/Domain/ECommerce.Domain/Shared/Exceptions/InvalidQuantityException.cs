using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Shared.Exceptions
{
    public class InvalidQuantityException : ECommerceException
    {
        public InvalidQuantityException() : base("Quantity must be equal or greater than zero.")
        {
        }
    }
}
