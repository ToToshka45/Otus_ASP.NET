using Microsoft.AspNetCore.Mvc;
using Pcf.Preferences.Core.Abstractions.Repositories;
using Pcf.Preferences.Core.Domain;
using Pcf.Preferences.WebHost.Models;
using Pcf.ReceivingFromPartner.WebHost.Models;

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

        public PreferencesController(IRepository<Preference> preferencesRepository)
        {
            _preferencesRepository = preferencesRepository;
        }

        /// <summary>
        /// Получить предпочтение по id
        /// </summary>
        /// <param name="id">Id предпочтения, например <example>a6c8c6b1-4349-45b0-ab31-244740aaf0f0</example></param>
        /// <returns></returns>
        [HttpGet( "{id:guid}" )]
        public async Task<ActionResult<PreferenceResponse>> GetPreferenceAsync( Guid id )
        {
            var preference = await _preferencesRepository.GetByIdAsync( id );

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
            var preferences = await _preferencesRepository.GetRangeByIdsAsync( request.PreferenceIds );

            var response = preferences.Select( x => new PreferenceResponse()
            {
                Id = x.Id,
                Name = x.Name
            } ).ToList();

            return Ok( response );
        }
    }
}