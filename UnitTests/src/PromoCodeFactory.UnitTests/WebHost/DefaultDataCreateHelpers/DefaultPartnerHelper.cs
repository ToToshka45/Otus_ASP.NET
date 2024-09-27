using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.UnitTests.WebHost.DefaultDataCreateHelpers
{
    public static class DefaultPartnerHelper
    {
        public static Partner CreateBasePartner()
        {
            var partner = new Partner()
            {
                Id = Guid.Parse( "FF87F725-1001-4873-ABC0-8FFC3413E11C" ),
                Name = "GameBy",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
                {
                    new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("E17C1E81-ED9C-4EEF-9857-FA29963813D7"),
                        CreateDate = DateTime.Now - TimeSpan.FromDays( 3 ),
                        EndDate = DateTime.Now + TimeSpan.FromDays( 7 ),
                        Limit = 10
                    }
                }
            };

            return partner;
        }
    }
}
