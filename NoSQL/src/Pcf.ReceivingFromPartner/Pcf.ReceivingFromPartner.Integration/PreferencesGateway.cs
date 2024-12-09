using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class PreferencesGateway : IPreferencesGateway
    {
        private readonly HttpClient _httpClient;

        public PreferencesGateway( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        public async Task GetPreferenceById( Guid preferenceId )
        {
            var response = await _httpClient.GetAsync( "api/v1/preferences/" + preferenceId );

            response.EnsureSuccessStatusCode();
        }

        public Task GetPreferencesRangeByIds( List<Guid> preferenceIds )
        {
            throw new NotImplementedException();
        }
    }
}
