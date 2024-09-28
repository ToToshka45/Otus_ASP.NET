using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    /// <summary>
    /// UOW.
    /// </summary>
    public interface IUnitOfWork
    {
        IPromocodeRepository PromocodeRepository { get; }

        ICustomerRepository CustomerRepository { get; }

        IPreferenceRepository PreferenceRepository { get; }

        Task SaveChangesAsync( CancellationToken cancellationToken = default );
    }
}
