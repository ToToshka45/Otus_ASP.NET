using Pcf.GivingToCustomer.Core.Dtos;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Abstractions.Services
{
    public interface IPromocodesService
    {
        Task<int> GivePromoCodesToCustomersWithPreferenceAsync( GivePromoCodeDto request );
    }
}
