using Microsoft.AspNetCore.Hosting;

namespace Pcf.Preferences.WebHost
{
    public class Program
    {
        public static void Main( string[] args )
        {
            CreateHostBuilder( args ).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder( string[] args ) =>
        Host.CreateDefaultBuilder( args )
                .ConfigureWebHostDefaults( webBuilder => { webBuilder.UseStartup<Startup>(); } );
    }
}
