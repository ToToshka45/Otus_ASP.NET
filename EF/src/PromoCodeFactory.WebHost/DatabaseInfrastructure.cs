using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.EntityFramework;
using System.Runtime.CompilerServices;

namespace PromoCodeFactory.WebHost
{
    public static class DatabaseInfrastructure
    {
        public static void MigrateDB( this IHost host )
        {
            using ( var scope = host.Services.CreateScope() )
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.EnsureDeleted();
                db.Database.Migrate();
            }
        }

        public static void SeedDB( this IHost host )
        {
            using ( var scope = host.Services.CreateScope() )
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                foreach ( var item in FakeDataFactory.Employees )
                {
                    db.Add<Employee>( item );
                }

                foreach ( var item in FakeDataFactory.Roles )
                {
                    db.Add<Role>( item );
                }

                foreach ( var item in FakeDataFactory.Preferences )
                {
                    db.Add<Preference>( item );
                }

                foreach ( var item in FakeDataFactory.Customers )
                {
                    db.Add<Customer>( item );
                }

                foreach ( var item in FakeDataFactory.PromoCodes )
                {
                    db.Add<PromoCode>( item );
                }

                db.SaveChanges();
            }
        }
    }
}
