using System.Threading.Tasks;
using System;
using Pcf.RabbitMQ_Events;
using MassTransit;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.WebHost.Mappers;
using System.Linq;
using Pcf.GivingToCustomer.Core.Services;
using Pcf.GivingToCustomer.Core.Abstractions.Services;

namespace Pcf.GivingToCustomer.WebHost.Consumers
{
    public sealed class PromocodeConsumer : IConsumer<PromocodeEvent>
    {
        private readonly IPromocodesService _promocodesService;

        public PromocodeConsumer( IPromocodesService promocodesService )
        {
            _promocodesService = promocodesService;
        }

        public async Task Consume( ConsumeContext<PromocodeEvent> context )
        {
            var promocodeDto = PromoCodeMapper.MapFromEventToDto( context.Message );
            var retCode = await _promocodesService.GivePromoCodesToCustomersWithPreferenceAsync( promocodeDto );
        }
    }
}
