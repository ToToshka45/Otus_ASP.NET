using System.Threading.Tasks;
using System;
using Pcf.RabbitMQ_Events;
using MassTransit;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.WebHost.Mappers;
using System.Linq;

namespace Pcf.GivingToCustomer.WebHost.Consumers
{
    public sealed class PromocodeConsumer : IConsumer<PromocodeEvent>
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;

        public PromocodeConsumer( IRepository<PromoCode> promoCodesRepository,
            IRepository<Preference> preferencesRepository, IRepository<Customer> customersRepository )
        {
            _promoCodesRepository = promoCodesRepository;
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
        }

        public async Task Consume( ConsumeContext<PromocodeEvent> context )
        {
            var promocodeDto = context.Message;

            //Получаем предпочтение по имени
            var preference = await _preferencesRepository.GetByIdAsync( promocodeDto.PreferenceId );

            if ( preference == null )
            {
                return;
            }

            //  Получаем клиентов с этим предпочтением:
            var customers = await _customersRepository
                .GetWhere( d => d.Preferences.Any( x =>
                    x.Preference.Id == preference.Id ) );

            PromoCode promoCode = PromoCodeMapper.MapFromEvent( promocodeDto, preference, customers );

            await _promoCodesRepository.AddAsync( promoCode );
        }
    }
}
