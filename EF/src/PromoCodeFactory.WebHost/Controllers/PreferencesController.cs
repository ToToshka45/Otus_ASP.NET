using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Роли сотрудников
    /// </summary>
    [ApiController]
    [Route( "api/v1/[controller]" )]
    public class PreferencesController
        : ControllerBase
    {
        private readonly IPreferenceRepository _preferenceRepository;

        public PreferencesController( IPreferenceRepository preferenceRepository )
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить все доступные предпочтения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync( Request.HttpContext.RequestAborted );

            var preferencesModelList = preferences.Select( x =>
                new PreferenceResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                } ).ToList();

            return preferencesModelList;
        }
    }
}