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
        private readonly IUnitOfWork _unitOfWork;

        public CustomersController( IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Получить данные всех клиентов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAllAsync( Request.HttpContext.RequestAborted );
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
            var entityItem = await _unitOfWork.CustomerRepository.GetAsync( id, Request.HttpContext.RequestAborted );

            var preferenceResponses = new List<PreferenceResponse>();
            var preferences = await _unitOfWork.PreferenceRepository.GetAsyncByIds( entityItem.CustomerPreferences.Select( cp => cp.PreferenceId ), Request.HttpContext.RequestAborted );
            foreach ( var preference in preferences )
            {
                var preferenceResponse = new PreferenceResponse()
                {
                    Id = preference.Id,
                    Name = preference.Name,
                };

                preferenceResponses.Add( preferenceResponse );
            }

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
                Preferences = preferenceResponses,
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
            var customerPreferences = new List<CustomerPreference>();
            var preferences = await _unitOfWork.PreferenceRepository.GetAsyncByIds( request.PreferenceIds, Request.HttpContext.RequestAborted );
            foreach ( var preference in preferences )
            {
                var customerPreference = new CustomerPreference()
                {
                    PreferenceId = preference.Id,
                    Preference = preference,
                };

                customerPreferences.Add( customerPreference );
            }

            var newCustomer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CustomerPreferences = customerPreferences,
            };

            var createdCustomer = await _unitOfWork.CustomerRepository.AddAsync( newCustomer, Request.HttpContext.RequestAborted );
            await _unitOfWork.SaveChangesAsync( Request.HttpContext.RequestAborted );

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
            var entityItem = await _unitOfWork.CustomerRepository.GetAsync( id, Request.HttpContext.RequestAborted );

            var customerPreferences = new List<CustomerPreference>();
            var preferences = await _unitOfWork.PreferenceRepository.GetAsyncByIds( request.PreferenceIds, Request.HttpContext.RequestAborted );
            foreach ( var preference in preferences )
            {
                var customerPreference = new CustomerPreference()
                {
                    PreferenceId = preference.Id,
                    Preference = preference,
                };

                customerPreferences.Add( customerPreference );
            }

            entityItem.FirstName = request.FirstName;
            entityItem.LastName = request.LastName;
            entityItem.Email = request.Email;
            entityItem.CustomerPreferences = customerPreferences;

            await _unitOfWork.CustomerRepository.UpdateAsync( entityItem, Request.HttpContext.RequestAborted );
            await _unitOfWork.SaveChangesAsync( Request.HttpContext.RequestAborted );

            return NoContent();
        }

        /// <summary>
        /// Удалить клиента по идентификатору вместе с выданными ему промокодами. Промокоды удаляются каскадно
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer( Guid id )
        {
            var wasDeleted = await _unitOfWork.CustomerRepository.DeleteAsync( id, Request.HttpContext.RequestAborted );

            if ( wasDeleted )
            {
                await _unitOfWork.SaveChangesAsync( Request.HttpContext.RequestAborted );
            }

            return Ok( wasDeleted );
        }
    }
}