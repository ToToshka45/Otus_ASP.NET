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
        var host = CreateHostBuilder(args).Build();

        using ( var scope = host.Services.CreateScope() )
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            Seed( db );
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            { 
                webBuilder.UseStartup<Startup>();
            } );

    private static void Seed( DatabaseContext databaseContext )
    {
        foreach ( var item in FakeDataFactory.Employees )
        {
            databaseContext.Add<Employee>( item );
        }

        foreach ( var item in FakeDataFactory.Roles )
        {
            databaseContext.Add<Role>( item );
        }

        foreach ( var item in FakeDataFactory.Preferences )
        {
            databaseContext.Add<Preference>( item );
        }

        foreach ( var item in FakeDataFactory.Customers )
        {
            databaseContext.Add<Customer>( item );
        }

        foreach ( var item in FakeDataFactory.PromoCodes )
        {
            databaseContext.Add<PromoCode>( item );
        }

        databaseContext.SaveChanges();
    }
}