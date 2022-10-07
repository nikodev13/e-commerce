using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime DateOfOrder { get; set; }
        public DateTime? DateOfDelivery { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
