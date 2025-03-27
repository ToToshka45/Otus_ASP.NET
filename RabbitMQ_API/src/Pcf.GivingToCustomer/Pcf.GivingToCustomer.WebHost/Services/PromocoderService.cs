using Grpc.Core;
using Microsoft.Extensions.Logging;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.WebHost.Mappers;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.Services
{
    public class PromocoderService : Promocoder.PromocoderBase
    {
        private readonly IPromocodesService _promocodesService;

        public PromocoderService( IPromocodesService promocodesService )
        {
            _promocodesService = promocodesService;
        }

        public async override Task<GivePromoCodeToCustomerReply> GivePromoCodeToCustomer( GivePromoCodeToCustomerRequest request, ServerCallContext context )
        {
            var promocodeDto = PromoCodeMapper.MapFromGrpcModelToDto( request );
            var retCode = await _promocodesService.GivePromoCodesToCustomersWithPreferenceAsync( promocodeDto );

            string message = string.Empty;
            if ( retCode == -2 )
            {
                message = "Bad Request";
            }
            else
            {
                message = "Promocode OK";
            }

            return  new GivePromoCodeToCustomerReply
            {
                Message = message
            };
        }
    }
}
