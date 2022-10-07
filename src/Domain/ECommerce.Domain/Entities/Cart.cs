using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public List<Product> Products { get; set; }
    }
}
