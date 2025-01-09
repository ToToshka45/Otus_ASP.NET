using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Dtos;
using Pcf.GivingToCustomer.WebHost.Models;
using Pcf.RabbitMQ_Events;
using System;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.WebHost.Mappers
{
    public class PromoCodeMapper
    {
        public static PromoCode MapFromModel( GivePromoCodeRequest request, Preference preference, IEnumerable<Customer> customers )
        {

            var promocode = new PromoCode();
            promocode.Id = request.PromoCodeId;

            promocode.PartnerId = request.PartnerId;
            promocode.Code = request.PromoCode;
            promocode.ServiceInfo = request.ServiceInfo;

            promocode.BeginDate = DateTime.Parse( request.BeginDate );
            promocode.EndDate = DateTime.Parse( request.EndDate );

            promocode.Preference = preference;
            promocode.PreferenceId = preference.Id;

            promocode.Customers = new List<PromoCodeCustomer>();

            foreach ( var item in customers )
            {
                promocode.Customers.Add( new PromoCodeCustomer()
                {

                    CustomerId = item.Id,
                    Customer = item,
                    PromoCodeId = promocode.Id,
                    PromoCode = promocode
                } );
            };

            return promocode;
        }

        public static PromoCode MapFromEvent( PromocodeEvent pEvent, Preference preference, IEnumerable<Customer> customers )
        {

            var promocode = new PromoCode();
            promocode.Id = Guid.Empty;

            promocode.PartnerId = pEvent.PartnerId;
            promocode.Code = pEvent.Code;
            promocode.ServiceInfo = pEvent.ServiceInfo;

            promocode.BeginDate = DateTime.Parse( pEvent.BeginDate );
            promocode.EndDate = DateTime.Parse( pEvent.EndDate );

            promocode.Preference = preference;
            promocode.PreferenceId = preference.Id;

            promocode.Customers = new List<PromoCodeCustomer>();

            foreach ( var item in customers )
            {
                promocode.Customers.Add( new PromoCodeCustomer()
                {

                    CustomerId = item.Id,
                    Customer = item,
                    PromoCodeId = promocode.Id,
                    PromoCode = promocode
                } );
            };

            return promocode;
        }

        internal static GivePromoCodeDto MapFromModelToDto( GivePromoCodeRequest request )
        {
            var dto = new GivePromoCodeDto();

            dto.ServiceInfo = request.ServiceInfo;
            dto.PartnerId = request.PartnerId;
            dto.PromoCodeId = request.PromoCodeId;
            dto.PromoCode = request.PromoCode;
            dto.PreferenceId = request.PreferenceId;
            dto.BeginDate = request.BeginDate;
            dto.EndDate = request.EndDate;

            return dto;
        }

        internal static GivePromoCodeDto MapFromEventToDto( PromocodeEvent pEvent )
        {
            var dto = new GivePromoCodeDto();

            dto.ServiceInfo = pEvent.ServiceInfo;
            dto.PartnerId = pEvent.PartnerId;
            dto.PromoCodeId = Guid.Empty;
            dto.PromoCode = pEvent.Code;
            dto.PreferenceId = pEvent.PreferenceId;
            dto.BeginDate = pEvent.BeginDate;
            dto.EndDate = pEvent.EndDate;

            return dto;
        }
    }
}
