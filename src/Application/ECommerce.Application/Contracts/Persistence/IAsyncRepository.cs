using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T>
    {
        Task<T?> GetById(Guid id);

        T
    }
}
