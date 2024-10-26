using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface ICustomerRepository : IEfRepository<Customer>
    {
        Task<List<Customer>> GetAllWithGivenPreferenceAsync( Guid preferenceId, CancellationToken cancellationToken );
    }
}
