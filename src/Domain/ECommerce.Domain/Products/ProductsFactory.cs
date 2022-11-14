using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.Products.Categories.ValueObjects;
using ECommerce.Domain.Products.ValueObjects;

namespace ECommerce.Domain.Products
{
    public static class ProductsFactory
    {
        public static Product CreateProduct(ProductName name, Description description, Category category)
        {
            return new Product(name, description, category);
        }
    }
}
