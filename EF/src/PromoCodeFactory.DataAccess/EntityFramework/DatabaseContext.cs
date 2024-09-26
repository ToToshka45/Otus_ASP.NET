using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System;

namespace PromoCodeFactory.DataAccess.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// Работники.
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Роли.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Покупатели.
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Предпочтения.
        /// </summary>
        public DbSet<Preference> Preferences { get; set; }

        /// <summary>
        /// Промо коды.
        /// </summary>
        public DbSet<PromoCode> PromoCodes { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.Entity<CustomerPreference>()
                .HasKey( cp => new { cp.CustomerId, cp.PreferenceId } );

            modelBuilder.Entity<Employee>().Property( c => c.FirstName ).HasMaxLength( 100 );
            modelBuilder.Entity<Employee>().Property( c => c.LastName ).HasMaxLength( 100 );
            modelBuilder.Entity<Employee>().Property( c => c.Email ).HasMaxLength( 100 );

            modelBuilder.Entity<Role>().Property( c => c.Name ).HasMaxLength( 100 );
            modelBuilder.Entity<Role>().Property( c => c.Description ).HasMaxLength( 300 );

            modelBuilder.Entity<Customer>().Property( c => c.FirstName ).HasMaxLength( 100 );
            modelBuilder.Entity<Customer>().Property( c => c.LastName ).HasMaxLength( 100 );
            modelBuilder.Entity<Customer>().Property( c => c.Email ).HasMaxLength( 100 );

            modelBuilder.Entity<Preference>().Property( c => c.Name ).HasMaxLength( 100 );

            modelBuilder.Entity<PromoCode>().Property( c => c.Code ).HasMaxLength( 100 );
            modelBuilder.Entity<PromoCode>().Property( c => c.ServiceInfo ).HasMaxLength( 300 );
            modelBuilder.Entity<PromoCode>().Property( c => c.PartnerName ).HasMaxLength( 100 );
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            optionsBuilder.LogTo( Console.WriteLine, LogLevel.Information );
        }
    }
}
