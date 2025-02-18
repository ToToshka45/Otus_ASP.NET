using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Abstractions.Services;
using Pcf.Administration.Core.Domain.Administration;
using System;
using System.Threading.Tasks;

namespace Pcf.Administration.Core.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService( IRepository<Employee> employeeRepository )
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// UpdateAppliedPromocodesAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns>0 If is OK, -1 if is NotFound</returns>
        public async Task<int> UpdateAppliedPromocodesAsync( Guid id )
        {
            var employee = await _employeeRepository.GetByIdAsync( id );

            if ( employee == null )
                return -1;

            employee.AppliedPromocodesCount++;

            await _employeeRepository.UpdateAsync( employee );

            return 0;
        }
    }
}
