using MassTransit;
using Pcf.Administration.Core.Abstractions.Services;
using Pcf.RabbitMQ_Events;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Consumers
{
    public sealed class NotifyAdminConsumer : IConsumer<NotifyAdminAboutPartnerManagerPromoCodeEvent>
    {
        private readonly IEmployeeService _employeeService;

        public NotifyAdminConsumer( IEmployeeService employeeService )
        {
            _employeeService = employeeService;
        }

        public async Task Consume( ConsumeContext<NotifyAdminAboutPartnerManagerPromoCodeEvent> context )
        {
            var employeeId = context.Message.PartnerManagerId;

            var retCode = await _employeeService.UpdateAppliedPromocodesAsync( employeeId );
        }
    }
}
