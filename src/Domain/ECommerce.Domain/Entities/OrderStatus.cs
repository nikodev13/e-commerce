using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public enum OrderStatus
    {
        Created,
        Paid,
        InProcess,
        ReadyForDispatch,
        Dispatched,
        Delivered,
        Returned,
        Rejected,
        Canceled,
        Closed
    }
}
