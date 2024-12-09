using Pcf.GivingToCustomer.Core.Abstractions.Gateways;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Integration.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.Integration
{
    public class PreferencesGateway : IPreferencesGateway
    {
        private readonly HttpClient _httpClient;

        public PreferencesGateway( HttpClient httpClient )
        {
            _httpClient = httpClient;
        }

        public async Task<Preference> GetPreferenceById( Guid preferenceId )
        {
            var response = await _httpClient.GetAsync( "api/v1/preferences/" + preferenceId );

            response.EnsureSuccessStatusCode();

            var preference = await response.Content.ReadFromJsonAsync<Preference>();
            return preference;
        }

        public async Task<List<Preference>> GetPreferencesRangeByIds( List<Guid> preferenceIds )
        {
            var dto = new GetPreferencesRangeDto()
            {
                PreferenceIds = preferenceIds,
            };

            var response = await _httpClient.PostAsJsonAsync( "api/v1/preferences/range", dto );

            response.EnsureSuccessStatusCode();

            var preferences = await response.Content.ReadFromJsonAsync<List<Preference>>();
            return preferences;
        }
    }
}
