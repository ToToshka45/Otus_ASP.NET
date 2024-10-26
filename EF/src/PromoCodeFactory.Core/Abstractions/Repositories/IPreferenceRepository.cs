using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Threading;
using System.Threading.Tasks;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IPreferenceRepository : IEfRepository<Preference>
    {
        Task<Preference> GetByNameAsync( string name, CancellationToken cancellationToken );
    }
}
