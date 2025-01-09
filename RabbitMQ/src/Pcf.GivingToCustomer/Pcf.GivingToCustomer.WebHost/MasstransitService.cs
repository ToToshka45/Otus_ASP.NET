using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;

namespace Pcf.GivingToCustomer.WebHost
{
    //public class MasstransitService : IHostedService
    //{
    //    private IBusControl _busControl;
    //    private readonly ILogger<MasstransitService> _logger;

    //    public MasstransitService( ILogger<MasstransitService> logger, IBusControl busControl )
    //    {
    //        _logger = logger;
    //        _busControl = busControl;
    //    }

    //    public async Task StartAsync( CancellationToken cancellationToken )
    //    {
    //        await _busControl.StartAsync( cancellationToken );
    //    }

    //    public async Task StopAsync( CancellationToken cancellationToken )
    //    {
    //        await _busControl.StopAsync( cancellationToken );
    //    }
    //}
}
