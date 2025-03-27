using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Abstractions.Services;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Dtos;
using Pcf.GivingToCustomer.Core.Mappers;
using System.Linq;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Core.Services
{
    public class PromocodesService : IPromocodesService
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;

        public PromocodesService( IRepository<PromoCode> promoCodesRepository,
            IRepository<Preference> preferencesRepository, IRepository<Customer> customersRepository )
        {
            _promoCodesRepository = promoCodesRepository;
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
        }

        /// <summary>
        /// UpdateAppliedPromocodesAsync
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>0 If is OK, -2 if is BadRequest</returns>
        public async Task<int> GivePromoCodesToCustomersWithPreferenceAsync( GivePromoCodeDto dto )
        {
            //Получаем предпочтение по имени
            var preference = await _preferencesRepository.GetByIdAsync( dto.PreferenceId );

            if ( preference == null )
            {
                return -2;
            }

            //  Получаем клиентов с этим предпочтением:
            var customers = await _customersRepository
                .GetWhere( d => d.Preferences.Any( x =>
                    x.Preference.Id == preference.Id ) );

            PromoCode promoCode = PromoCodeCoreMapper.MapFromModel( dto, preference, customers );

            await _promoCodesRepository.AddAsync( promoCode );

            return 0;
        }
    }
}
