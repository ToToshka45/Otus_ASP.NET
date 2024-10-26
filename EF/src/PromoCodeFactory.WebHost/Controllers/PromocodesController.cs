using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IUnitOfWork _unitOfWork;

        public PromocodesController( IConfiguration configuration, IUnitOfWork unitOfWork )
        {
            _configuration = configuration;

            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _unitOfWork.PromocodeRepository.GetAllAsync( Request.HttpContext.RequestAborted );
            var response = promocodes.Select( x =>
                new PromoCodeResponse()
                {
                    Id = x.Id,
                    Code = x.Code,
                    ServiceInfo = x.ServiceInfo,
                    BeginDate = x.BeginDate.ToString(),
                    EndDate = x.EndDate.ToString(),
                    PartnerName = x.PartnerName,
                    Preference = new PreferenceResponse()
                    {
                        Id = x.Preference.Id,
                        Name = x.Preference.Name,
                    }
                } ).ToList();

            return Ok( response );
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            var preference = await _unitOfWork.PreferenceRepository.GetByNameAsync( request.Preference, Request.HttpContext.RequestAborted );
            if ( preference is null )
            {
                return NotFound("Заданное предпочтение не было найдено.");
            }

            // Создание промокода
            var newPromoCode = new PromoCode()
            {
                Code = request.PromoCode,
                ServiceInfo = request.ServiceInfo,
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now + TimeSpan.FromDays( _configuration.GetValue<int>( "defaultPromocodeDuration" ) ),
                PartnerName = request.PartnerName,
                PreferenceId = preference.Id
            };

            var createdPromoCode = await _unitOfWork.PromocodeRepository.AddAsync( newPromoCode, Request.HttpContext.RequestAborted );

            // Выдача промокода клиентам с указанным предпочтением
            var customersWithPreference = await _unitOfWork.CustomerRepository.GetAllWithGivenPreferenceAsync( preference.Id, Request.HttpContext.RequestAborted );
            foreach ( var customerWithPreference in customersWithPreference )
            {
                customerWithPreference.PromoCodes.Add( createdPromoCode );
                await _unitOfWork.CustomerRepository.UpdateAsync( customerWithPreference, Request.HttpContext.RequestAborted );
            }

            // Сохранение изменений
            await _unitOfWork.SaveChangesAsync( Request.HttpContext.RequestAborted );

            return Created();
        }
    }
}