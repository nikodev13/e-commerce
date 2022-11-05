using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Shared.Exceptions
{
    public abstract class ECommerceException : Exception
    {
        public ECommerceException(string message) : base(message)
        {
        }
    }
}
