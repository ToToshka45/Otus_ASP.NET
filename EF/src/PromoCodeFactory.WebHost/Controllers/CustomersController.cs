using Microsoft.AspNetCore.Mvc;
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
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IPromocodeRepository _promocodeRepository;

        public CustomersController( ICustomerRepository customerRepository, IPreferenceRepository preferenceRepository, IPromocodeRepository promocodeRepository )
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
            _promocodeRepository = promocodeRepository;
        }

        /// <summary>
        /// Получить данные всех клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync( Request.HttpContext.RequestAborted );
            var response = customers.Select( x =>
                new CustomerShortResponse()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                } ).ToList();

            return Ok( response );
        }

        /// <summary>
        /// Получить данные клиента по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var entityItem = await _customerRepository.GetAsync( id, Request.HttpContext.RequestAborted );

            var response = new CustomerResponse()
            {
                Id = entityItem.Id,
                FirstName = entityItem.FirstName,
                LastName = entityItem.LastName,
                Email = entityItem.Email,
                PromoCodes = entityItem.PromoCodes.Select(pc => {
                    return new PromoCodeResponse()
                    {
                        Id = pc.Id,
                        Code = pc.Code,
                        ServiceInfo = pc.ServiceInfo,
                        BeginDate = pc.BeginDate.ToString(),
                        EndDate = pc.EndDate.ToString(),
                        PartnerName = pc.PartnerName,
                    };
                } ).ToList(),
                Preferences = entityItem.CustomerPreferences.Select( p =>
                {
                    var preference = _preferenceRepository.Get( p.PreferenceId );

                    return new PreferenceResponse()
                    {
                        Id = preference.Id,
                        Name = preference.Name,
                    };
                } ).ToList(),
            };

            return Ok( response );
        }

        /// <summary>
        /// Создать нового клиента
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync( CreateOrEditCustomerRequest request )
        {
            var newCustomer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CustomerPreferences = ( await Task.WhenAll( request.PreferenceIds.Select( async pId =>
                {
                    var preference = await _preferenceRepository.GetAsync( pId, Request.HttpContext.RequestAborted );

                    return new CustomerPreference()
                    {
                        PreferenceId = pId,
                        Preference = preference,
                    };
                } ) ) ).ToList(),
            };

            var createdCustomer = await _customerRepository.AddAsync( newCustomer );
            await _customerRepository.SaveChangesAsync( Request.HttpContext.RequestAborted );

            return Created();
        }

        /// <summary>
        /// Редактировать клиента
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var entityItem = await _customerRepository.GetAsync( id, Request.HttpContext.RequestAborted );

            entityItem.FirstName = request.FirstName;
            entityItem.LastName = request.LastName;
            entityItem.Email = request.Email;
            entityItem.CustomerPreferences = request.PreferenceIds.Select( pId =>
            {
                var preference = _preferenceRepository.Get( pId );

                return new CustomerPreference()
                {
                    PreferenceId = pId,
                    Preference = preference,
                };
            } ).ToList();

            _customerRepository.Update( entityItem );
            await _customerRepository.SaveChangesAsync( Request.HttpContext.RequestAborted );

            return NoContent();
        }

        /// <summary>
        /// Удалить клиента по идентификатору вместе с выданными ему промокодами
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer( Guid id )
        {
            var wasDeleted = _customerRepository.Delete( id );

            if ( wasDeleted )
            {
                var entityItem = await _customerRepository.GetAsync( id, Request.HttpContext.RequestAborted );
                foreach ( var promocode in entityItem.PromoCodes )
                {
                    _promocodeRepository.Delete( promocode );
                }

                await _customerRepository.SaveChangesAsync( Request.HttpContext.RequestAborted );
            }

            return Ok( wasDeleted );
        }
    }
}