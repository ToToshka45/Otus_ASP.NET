using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.EntityFramework;
using System;

namespace PromoCodeFactory.WebHost;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args)
            .Build();

        host.MigrateDB();
        host.SeedDB();

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            { 
                webBuilder.UseStartup<Startup>();
            } );
}