using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class GivingPromoCodeToCustomerGrpcGateway : IGivingPromoCodeToCustomerGrpcGateway
    {
        private readonly string _givingToCustomerApiUrl;

        public GivingPromoCodeToCustomerGrpcGateway( IConfiguration configuration )
        {
            _givingToCustomerApiUrl = configuration[ "IntegrationSettings:GivingToCustomerApiUrl" ];
        }

        public async Task GivePromoCodeToCustomer( PromoCode promoCode )
        {
            using var channel = GrpcChannel.ForAddress( _givingToCustomerApiUrl );
            var client = new Promocoder.PromocoderClient( channel );

            var requestData = new GivePromoCodeToCustomerRequest()
            {
                PartnerId = promoCode.Partner.Id.ToString(),
                BeginDate = promoCode.BeginDate.ToShortDateString(),
                EndDate = promoCode.EndDate.ToShortDateString(),
                PreferenceId = promoCode.PreferenceId.ToString(),
                PromoCode = promoCode.Code,
                ServiceInfo = promoCode.ServiceInfo,
                PartnerManagerId = promoCode.PartnerManagerId == null ? "" : promoCode.PartnerManagerId.ToString()
            };

            var reply = await client.GivePromoCodeToCustomerAsync( requestData );
        }
    }
}
