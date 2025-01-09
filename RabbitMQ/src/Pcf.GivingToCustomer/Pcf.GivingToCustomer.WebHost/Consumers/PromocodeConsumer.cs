using System.Threading.Tasks;
using System;
using Pcf.RabbitMQ_Events;
using MassTransit;

namespace Pcf.GivingToCustomer.WebHost.Consumers
{
    public sealed class PromocodeConsumer : IConsumer<PromocodeEvent>
    {
        public async Task Consume( ConsumeContext<PromocodeEvent> context )
        {
            //throw new ArgumentException("some error");
            //await Task.Delay( TimeSpan.FromSeconds( 2 ) );
            Console.WriteLine( "Value: {0}", context.Message.Content );
        }
    }
}
