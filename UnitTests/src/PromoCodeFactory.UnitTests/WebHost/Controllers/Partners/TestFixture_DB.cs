using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PromoCodeFactory.WebHost;
using System;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class TestFixture_DB : IDisposable
    {
        public IServiceProvider ServiceProvider { get; set; }

        public IServiceCollection ServiceCollection { get; set; }

        /// <summary>
        /// Выполняется перед запуском тестов
        /// </summary>
        public TestFixture_DB()
        {
            var builder = new ConfigurationBuilder();
            var configuration = builder.Build();

            var startup = new Startup( configuration );

            ServiceCollection = new ServiceCollection();
            startup.ConfigureServices( ServiceCollection );

            var serviceProvider = startup.GetServiceProvider( ServiceCollection );
            ServiceProvider = serviceProvider;


        }

        public void Dispose()
        {

        }
    }
}
