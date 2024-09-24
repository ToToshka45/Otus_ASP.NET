using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface ICustomerRepository : IEfRepository<Customer>
    {
        Task<List<Customer>> GetAllByPreferenceAsync( string preference, CancellationToken cancellationToken );
    }
}
