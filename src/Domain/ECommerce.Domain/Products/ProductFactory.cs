using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Products
{
    public static class ProductFactory
    {
        public static Product CreateProduct(string name, string description)
        {
            return new Product(name, description);
        }
    }
}
