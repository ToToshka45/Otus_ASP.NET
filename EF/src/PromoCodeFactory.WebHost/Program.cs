using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace PromoCodeFactory.WebHost;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = CreateHostBuilder( args );
        builder = builder.ConfigureAppConfiguration( ( hostContext, builder ) =>
        {
            builder.Sources.Clear();

            var env = hostContext.HostingEnvironment;

            builder.AddEnvironmentVariables()
                   .AddJsonFile( "Configs/appsettings.json", false, true )
                   .AddJsonFile( "Configs/promoCodeSettings.json", true, true )
                   .AddJsonFile( $"Configs/appsettings.{env.EnvironmentName}.json", true, true );

        } );

        var host = builder.Build();

        host.MigrateDB();
        host.SeedDB();

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder( args )
                   .ConfigureWebHostDefaults( webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>();
                   } );
    }

}