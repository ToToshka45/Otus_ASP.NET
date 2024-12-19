using Pcf.ReceivingFromPartner.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Gateways
{
    public interface IPreferencesGateway
    {
        Task<Preference> GetPreferenceById( Guid preferenceId );
        Task<List<Preference>> GetPreferencesRangeByIds( List<Guid> preferenceIds );
    }
}
