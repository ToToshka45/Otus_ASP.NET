using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Contracts.Employee;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateEmployeeAsync( [FromBody] CreateEmployeeRequest dto )
        {
            var newEmployee = new Employee()
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Roles = new List<Role>(),
                AppliedPromocodesCount = 0,
            };

            var result = await _employeeRepository.AddAsync( newEmployee );
            return Created( string.Empty, result );
        }

        /// <summary>
        /// Обновить сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpPatch( "{id:guid}" )]
        public async Task<ActionResult> UpdateEmployeeByIdAsync( [FromRoute] Guid id, [FromBody] UpdateEmployeeRequest dto )
        {
            var employee = await _employeeRepository.GetByIdAsync( id );

            if ( employee == null )
                return NotFound();

            var updatedEmployee = new Employee()
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
            };

            var result = await _employeeRepository.UpdateAsync( updatedEmployee );
            return Ok( result );
        }

        /// <summary>
        /// Удалить сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete( "{id:guid}" )]
        public async Task<ActionResult> DeleteEmployeeByIdAsync( Guid id )
        {
            var isRemoved = await _employeeRepository.DeleteByIdAsync( id );
            if ( isRemoved )
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}