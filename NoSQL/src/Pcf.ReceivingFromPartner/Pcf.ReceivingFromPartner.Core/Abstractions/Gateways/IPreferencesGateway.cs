using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Gateways
{
    public interface IPreferencesGateway
    {
        Task GetPreferenceById( Guid preferenceId );
        Task GetPreferencesRangeByIds( List<Guid> preferenceIds );
    }
}
