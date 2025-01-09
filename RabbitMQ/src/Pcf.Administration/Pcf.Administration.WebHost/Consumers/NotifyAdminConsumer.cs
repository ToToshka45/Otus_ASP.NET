using System.Threading.Tasks;
using System;
using Pcf.RabbitMQ_Events;
using MassTransit;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Administration.Core.Abstractions.Repositories;

namespace Pcf.Administration.WebHost.Consumers
{
    public sealed class NotifyAdminConsumer : IConsumer<NotifyAdminAboutPartnerManagerPromoCodeEvent>
    {
        private readonly IRepository<Employee> _employeeRepository;

        public NotifyAdminConsumer( IRepository<Employee> employeeRepository )
        {
            _employeeRepository = employeeRepository;
        }

        public async Task Consume( ConsumeContext<NotifyAdminAboutPartnerManagerPromoCodeEvent> context )
        {
            var employeeId = context.Message.PartnerManagerId;
            var employee = await _employeeRepository.GetByIdAsync( employeeId );

            if ( employee == null )
            {
                return;
            }

            employee.AppliedPromocodesCount++;

            await _employeeRepository.UpdateAsync( employee );
        }
    }
}
