using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Pcf.Preferences.Core.Abstractions.Repositories;
using Pcf.Preferences.Core.Domain;
using Pcf.Preferences.WebHost.Models;
using Pcf.Preferences.WebHost.Services;
using Pcf.ReceivingFromPartner.WebHost.Models;
using System.Text.Json;

namespace Pcf.Preferences.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения клиентов
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController
        : ControllerBase
    {
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IDistributedCache _distributedCache;
        private readonly ICacheService _cacheService;

        public PreferencesController(IRepository<Preference> preferencesRepository, IDistributedCache distributedCache, ICacheService cacheService )
        {
            _preferencesRepository = preferencesRepository;
            _distributedCache = distributedCache;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Получить предпочтение по id
        /// </summary>
        /// <param name="id">Id предпочтения, например <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
        /// <returns></returns>
        [HttpGet( "{id:guid}" )]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceAsync( Guid id )
        {
            string nowKey = GetRedisKeyForPreference( id );

            var preference = await _cacheService.GetAsync<Preference>( nowKey );
            if ( preference is null )
            {
                preference = await _preferencesRepository.GetByIdAsync( id );

                await _cacheService.SetAsync<Preference>( nowKey, preference );
            }

            var response = new PreferenceResponse()
            {
                Id = preference.Id,
                Name = preference.Name
            };

            return Ok( response );
        }

        /// <summary>
        /// Получить список предпочтений
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
        {
            var preferences = await _preferencesRepository.GetAllAsync();

            var response = preferences.Select(x => new PreferenceResponse()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Создать промокод от партнера 
        /// </summary>
        /// <param name="request">Данные запроса/example></param>
        /// <returns></returns>
        [HttpPost( "range" )]
        public async Task<IActionResult> GetPreferencesRangeAsync( PreferencesRangeRequest request )
        {
            var preferences = new List<Preference>();
            var preferencesToRequest = new List<Guid>();

            foreach ( var preferenceId in request.PreferenceIds )
            {
                string nowKey = GetRedisKeyForPreference( preferenceId );

                var preference = await _cacheService.GetAsync<Preference>( nowKey );
                if ( preference is null )
                {
                    preferencesToRequest.Add( preferenceId );
                }
                else
                {
                    preferences.Add( preference );
                }
            }

            var requestedPreferences = await _preferencesRepository.GetRangeByIdsAsync( preferencesToRequest );
            preferences.AddRange( requestedPreferences );

            var response = preferences.Select( x => new PreferenceResponse()
            {
                Id = x.Id,
                Name = x.Name
            } ).ToList();

            foreach ( var requestedPreference in requestedPreferences )
            {
                string nowKey = GetRedisKeyForPreference( requestedPreference.Id );
                await _cacheService.SetAsync<Preference>( nowKey, requestedPreference );
            }

            return Ok( response );
        }

        private string GetRedisKeyForPreference(Guid preferenceId)
        {
            return $"Preferences:{preferenceId}";
        }
    }
}